using AutoFixture;
using AutoMapper;
using Moq;
using FluentAssertions;
using TaskTracker_BL.DTOs;
using TaskTracker_BL.Services.GeneratorService;
using TaskTracker_BL.Services.QueryService;
using TaskTracker_BL.Services.HashService;
using TaskTracker_BL.Services.TokenService;
using TaskTracker_BL.Services;
using TaskTracker_BL.Services.SmtpService;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;
using TaskTracker_DAL.BasicGenericRepository;
using TaskTracker_DAL.RolesHelper;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace TaskTracker_BL_Tests
{
    public class AuthServiceTests
    {
        private Fixture _fixture;
        private Mock<IMapper> _mapper;
        private Mock<IHashService> _hashService;
        private Mock<IGenericRepository<User>> _userRepository;
        private Mock<ITokenService> _tokenService;
        private Mock<IGenericRepository<EmailStatus>> _emailStatusRepository;
        private Mock<ISmtpService> _smtpService;
        private Mock<IRolesHelper> _rolesHelper;
        private Mock<IBasicGenericRepository<UserRoles>> _userRolesRepository;
        private Mock<IGenericRepository<Role>> _rolesRepository;
        private Mock<IQueryService> _queryService;
        private Mock<IGeneratorService> _generatorService;
        private Mock<ILogger<AuthService>> _logger;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _mapper = new Mock<IMapper>();
            _hashService = new Mock<IHashService>();
            _userRepository = new Mock<IGenericRepository<User>>();
            _tokenService = new Mock<ITokenService>();
            _emailStatusRepository = new Mock<IGenericRepository<EmailStatus>>();
            _smtpService = new Mock<ISmtpService>();
            _rolesHelper = new Mock<IRolesHelper>();
            _userRolesRepository = new Mock<IBasicGenericRepository<UserRoles>>();
            _rolesRepository = new Mock<IGenericRepository<Role>>();
            _queryService = new Mock<IQueryService>();
            _generatorService = new Mock<IGeneratorService>();
            _logger = new Mock<ILogger<AuthService>>();
        }

        public AuthService GetAuthService()
        {
            return new AuthService(
                _userRepository.Object,
                _mapper.Object,
                _tokenService.Object,
                _hashService.Object,
                _smtpService.Object,
                _emailStatusRepository.Object,
                _userRolesRepository.Object,
                _rolesRepository.Object,
                _rolesHelper.Object,
                _queryService.Object,
                _generatorService.Object,
                _logger.Object);
        }

        [Test]
        public async Task RegisterAsync_WhenCalled_ShouldSendConfirmationEmail()
        {
            var registartionDto = _fixture.Create<RegistrationDto>();
            var user = _fixture.Create<User>();
            var uriBuilder = _fixture.Create<UriBuilder>();
            var emailStatus = _fixture.Create<EmailStatus>();

            var hashedPassword = _fixture.Create<string>();
            string emailKey = _fixture.Create<string>();
            var queryParams = _fixture.Create<Dictionary<string, string>>();
            emailStatus.IsConfirmed = false;
            emailStatus.UserId = user.Id;
            emailStatus.Key = emailKey;
            string emailMessage = "Email confirmation";

            _mapper.Setup(x => x.Map<User>(registartionDto))
                .Returns(user).Verifiable();

            _hashService.Setup(x => x.GetHash(user.Password)).Returns(hashedPassword).Verifiable();

            _generatorService.Setup(x
                => x.GetRandomKey()).Returns(emailKey).Verifiable();
            _queryService.Setup(x
                => x.CreateQueryParams(user.Email, emailKey)).Returns(queryParams).Verifiable();
            _queryService.Setup(x
                => x.AddQueryParams(uriBuilder, queryParams)).Verifiable();

            _userRepository.Setup(x => x.CreateAsync(user)).ReturnsAsync(user).Verifiable();

            _emailStatusRepository.Setup(x
                => x.CreateAsync(It.Is<EmailStatus>(x
                => x.IsConfirmed == false && x.UserId == user.Id && x.Key == emailKey))).Verifiable();

            _smtpService.Setup(x => x.SendEmailAsync(user.Email, emailMessage, uriBuilder.Uri.ToString())).Verifiable();


            var authService = GetAuthService();

            var response = authService.RegisterAsync(registartionDto, uriBuilder);

            _mapper.Verify();
            _queryService.VerifyAll();
            _hashService.Verify();
            _userRepository.Verify();
            _emailStatusRepository.Verify();
            _smtpService.Verify();
        }

        [Test]
        public async Task LoginAsync_WhenCalled_TokenReturned()
        {
            var userWithRolesDto = _fixture.Create<UserWithRolesDto>();
            var credentialsDto = _fixture.Create<CredentialsDto>();
           
            var user = _fixture.Create<User>();
            string token = _fixture.Create<string>();

            
            _userRepository.Setup(x => x.GetByPredicate(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User> { user }.AsQueryable()).Verifiable();

            _hashService.Setup(x => x.ValidateHash(credentialsDto.Password, user.Password)).Returns(true).Verifiable();
            var roleTitles = user.Roles.Select(x => x.Role.Title).ToList();


            _tokenService.Setup(x => x.GenerateToken(user.Email, It.Is<IEnumerable<string>>(x => x.SequenceEqual(roleTitles)))).Returns(token).Verifiable();

            var authService = GetAuthService();

            var result = await authService.LoginAsync(credentialsDto);
            result.Should().Be(token);
            _userRepository.Verify();
            _hashService.Verify();
            _tokenService.Verify();

        }

        [Test]
        public async Task LoginAsync_WhenInvalidCredentials_ShouldReturnNull()
        {
            var credentialsDto = _fixture.Create<CredentialsDto>();

            string token = _fixture.Create<string>();

            _userRepository.Setup(x => x.GetByPredicate(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User> { }.AsQueryable()).Verifiable();

            var authService = GetAuthService();

            var result = await authService.LoginAsync(credentialsDto);
            result.Should().BeNull();
            _userRepository.Verify();
            _hashService.Verify(x=>x.ValidateHash(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _tokenService.Verify(x=>x.GenerateToken(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);

        }

        [Test]
        public async Task ConfirmEmailAsync_WhenCalled_ReturnsTrue()
        {
            var emailStatus = _fixture.Create<EmailStatus>();
            var userRole = _fixture.Create<UserRoles>();
            string email = _fixture.Create<string>();
            string key = _fixture.Create<string>();
            var roleTitlle = "User";
            var roleId = _fixture.Create<Guid>();

            _emailStatusRepository.Setup(x => x.GetByPredicate(It.IsAny<Expression<Func<EmailStatus, bool>>>()))
                .Returns(new List<EmailStatus> { emailStatus}.AsQueryable()).Verifiable();

            _emailStatusRepository.Setup(x => x.UpdateAsync(It.Is<EmailStatus>(x
                  => x.Id == emailStatus.Id
                  && x.IsConfirmed == emailStatus.IsConfirmed
                  && x.Key == emailStatus.Key
                  && x.User == emailStatus.User
                  && x.UserId == emailStatus.UserId)))
                .ReturnsAsync(emailStatus).Verifiable();

            _rolesHelper.Setup(x => x[roleTitlle]).Returns(roleId).Verifiable();

            _userRolesRepository.Setup(x => x.CreateAsync(It.Is<UserRoles>(x 
                => x.RoleId == roleId
                && x.UserId == emailStatus.UserId)))
                .ReturnsAsync(userRole).Verifiable();

            var authService = GetAuthService();

            var response = await authService.ConfirmEmailAsync(email, key);
            response.Should().BeTrue();

            _emailStatusRepository.VerifyAll();
            _rolesHelper.Verify();
            _userRolesRepository.Verify();

        }

        [Test]
        public async Task ConfirmEmailAsync_WhenWrongKeyOrEmail_ReturnsFalse()
        {
            var userRole = _fixture.Create<UserRoles>();
            string email = _fixture.Create<string>();
            string key = _fixture.Create<string>();
            var roleTitlle = _fixture.Create<string>();

            _emailStatusRepository.Setup(x => x.GetByPredicate(It.IsAny<Expression<Func<EmailStatus, bool>>>()))
                .Returns(new List<EmailStatus> { }.AsQueryable()).Verifiable();

            var authService = GetAuthService();

            var response = await authService.ConfirmEmailAsync(email, key);
            response.Should().BeFalse();

            _emailStatusRepository.Verify();
            _rolesHelper.Verify(x=>x[roleTitlle], Times.Never);
            _userRolesRepository.Verify(x=>x.CreateAsync(It.IsAny<UserRoles>()), Times.Never);

        }
    }
}