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
        IGeneratorService _generatorService;

        public AuthService(IGenericRepository<User> userRepository,
            IMapper mapper, ITokenService tokenService,
            IHashService hashService, ISmtpService googleSmtpService,
            IGenericRepository<EmailStatus> emailStatusRepository,
            IBasicGenericRepository<UserRoles> userRolesRepository,
            IGenericRepository<Role> roleRepository,
            IRolesHelper rolesHelper,
            IQueryService queryService,
            IGeneratorService generatorService)
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
        }

        public async Task RegisterAsync(RegistrationDto registartionDto, UriBuilder uriBuilder)
        {
            var dto = _mapper.Map<User>(registartionDto);
            dto.Password = _hashService.GetHash(dto.Password!);
            await _userRepository.CreateAsync(dto);
            string emailKey = _generatorService.GetRandomKey();
            _queryService.AddQueryParams(uriBuilder, _queryService.CreateQueryParams(dto.Email!, emailKey));
            await _emailStatusRepository.CreateAsync(
                new EmailStatus
                {
                    IsConfirmed = false,
                    UserId = dto.Id,
                    Key = emailKey
                });
            await _googleSmtpService.SendEmailAsync(dto.Email!, "Email confirmation", uriBuilder.Uri.ToString());

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
                    return _tokenService.GenerateToken(userWithRolesDto.User.Email!, userWithRolesDto.UserRoles);
            }
            return null;
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
                return true;
            }
            return false;
        }



    }

}
