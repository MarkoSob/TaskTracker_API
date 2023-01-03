using AutoMapper;
using TaskTracker_BL.DTOs;
using TaskTracker_BL.Services.HashService;
using TaskTracker_BL.Services.TokenService;
using TaskTracker_BL.Services.QueryService;
using TaskTracker_BL.Services.GeneratorService;
using TaskTracker_BL.Services.SmtpService;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.RolesHelper;
using TaskTracker_DAL.GenericRepository;
using TaskTracker_DAL.BasicGenericRepository;
using Microsoft.Extensions.Logging;

namespace TaskTracker_BL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IHashService _hashService;
        private readonly ISmtpService _googleSmtpService;
        private readonly IGenericRepository<EmailStatus> _emailStatusRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IBasicGenericRepository<UserRoles> _userRolesRepository;
        private readonly IRolesHelper _rolesHelper;
        private readonly IQueryService _queryService;
        private readonly IGeneratorService _generatorService;
        private readonly ILogger<AuthService> _logger;
        public AuthService(
            IGenericRepository<User> userRepository,
            IMapper mapper, 
            ITokenService tokenService,
            IHashService hashService, 
            ISmtpService googleSmtpService,
            IGenericRepository<EmailStatus> emailStatusRepository,
            IBasicGenericRepository<UserRoles> userRolesRepository,
            IGenericRepository<Role> roleRepository,
            IRolesHelper rolesHelper,
            IQueryService queryService,
            IGeneratorService generatorService,
            ILogger<AuthService> logger)
        {
            _googleSmtpService = googleSmtpService;
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _hashService = hashService;
            _emailStatusRepository = emailStatusRepository;
            _userRolesRepository = userRolesRepository;
            _roleRepository = roleRepository;
            _rolesHelper = rolesHelper;
            _queryService = queryService;
            _generatorService = generatorService;
            _logger = logger;
        }

        public async Task RegisterAsync(RegistrationDto registartionDto, UriBuilder uriBuilder)
        {
            var user = _mapper.Map<User>(registartionDto);
            user.CreationDate = DateTime.Now;
            user.Password = _hashService.GetHash(user.Password!);

            await _userRepository.CreateAsync(user);

            _logger.LogInformation($"User {user.Id} was registered");

            string emailKey = _generatorService.GetRandomKey();

            _queryService.AddQueryParams(uriBuilder, _queryService.CreateQueryParams(user.Email!, emailKey));

            await _emailStatusRepository.CreateAsync(
                new EmailStatus
                {
                    IsConfirmed = false,
                    UserId = user.Id,
                    Key = emailKey
                });

            await _googleSmtpService.SendEmailAsync(user.Email!, "Email confirmation", uriBuilder.Uri.ToString());

            _logger.LogInformation($"Confirmation email was sent to {user.Email}");
        }

        public async Task<bool> ChangePasswordAsync(string email, string currentPasswoord, string newPassword)
        {
            User currentUser =  _userRepository.GetByPredicate(x => x.Email == email).FirstOrDefault();

            if (currentUser.Password == _hashService.GetHash(currentPasswoord))
            {
                currentUser.Password = _hashService.GetHash(newPassword);

                await _userRepository.UpdateAsync(currentUser);

                _logger.LogInformation($"User {currentUser.Email} changed password");

                return true;
            }

            _logger.LogInformation($"User {currentUser.Email} tried to change the password without success");

            return false;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            User currentUser = _userRepository.GetByPredicate(x => x.Email == email).FirstOrDefault();

            if (currentUser != null)
            {
                string password = _generatorService.GetRandomKey();
                currentUser.Password = _hashService.GetHash(password);

                await _userRepository.UpdateAsync(currentUser);

                _googleSmtpService.SendEmailAsync(currentUser.Email, "New temporary password", "Your new temporary password: " + password);

                _logger.LogInformation($"User {email} reset password");

                return true;
            }

            _logger.LogInformation($"User {email} tried to reset password without success");

            return false;
        }

        public async Task<string> LoginAsync(CredentialsDto credentialsDto)
        {
            var userWithRolesDto = _userRepository
                .GetByPredicate(x => x.Email == credentialsDto.Login)
                .Select(x => new UserWithRolesDto
                {
                    User = x,
                    UserRoles = x.Roles!.Select(x => x.Role!.Title)!
                })
                .FirstOrDefault();

            if (userWithRolesDto != null)
            {
                if (_hashService.ValidateHash(credentialsDto.Password!, userWithRolesDto.User.Password!))
                {
                    _logger.LogInformation($"User {credentialsDto.Login} logged in");

                    return _tokenService.GenerateToken(userWithRolesDto.User.Email!, userWithRolesDto.UserRoles);
                }
            }

            _logger.LogInformation($"User {credentialsDto.Login} tried to log in without success");

            return string.Empty;
        }

        public async Task<bool> ConfirmEmailAsync(string email, string key)
        {
            var emailStatus = _emailStatusRepository
                .GetByPredicate(x => x.User.Email == email && x.Key == key)
                .FirstOrDefault();

            if (emailStatus != null)
            {
                emailStatus.IsConfirmed = true;

                await _emailStatusRepository.UpdateAsync(emailStatus);

                var roleId = _rolesHelper[RolesList.User];

                await _userRolesRepository.CreateAsync(new UserRoles
                {
                    UserId = emailStatus.UserId,
                    RoleId = roleId,
                });

                _logger.LogInformation($"User {email} confirmed email");

                return true;
            }

            _logger.LogInformation($"User {email} tried to confirm email without success");

            return false;
        }
    }
}
