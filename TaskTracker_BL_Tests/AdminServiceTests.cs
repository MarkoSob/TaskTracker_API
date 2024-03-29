﻿using AutoFixture;
using Moq;
using FluentAssertions;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;
using TaskTracker_DAL.BasicGenericRepository;
using TaskTracker_DAL.RolesHelper;
using System.Linq.Expressions;
using TaskTracker_BL.Services.AdminService;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace TaskTracker_BL_Tests
{
    public class AdminServiceTests
    {
        private Fixture _fixture;
        private Mock<IRolesHelper> _rolesHelper;
        private Mock<IMapper> _mapper;
        private Mock<IBasicGenericRepository<UserRoles>> _userRolesRepository;
        private Mock<IGenericRepository<Role>> _rolesRepository;
        private Mock<IGenericRepository<User>> _userRepository;
        private Mock<ILogger<AdminService>> _logger;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _mapper = new Mock<IMapper>();
            _rolesHelper = new Mock<IRolesHelper>();
            _userRolesRepository = new Mock<IBasicGenericRepository<UserRoles>>();
            _rolesRepository = new Mock<IGenericRepository<Role>>();
            _userRepository = new Mock<IGenericRepository<User>>();
            _logger = new Mock<ILogger<AdminService>>();

        }

        public AdminService GetAdminService()
        {
            return new AdminService(
                _userRolesRepository.Object,
                _rolesRepository.Object,
                _userRepository.Object,
                _rolesHelper.Object,
                _mapper.Object,
                _logger.Object
                );
        }

        [Test]
        public async Task GiveRoleAsync_WhenCalled_ShouldGiveRoleToUser()
        {
            var roleTitle = _fixture.Create<string>();
            var roleId = _fixture.Create<Guid>();
            var userId = _fixture.Create<Guid>();
            var adminService = GetAdminService();

            _rolesHelper.Setup(x => x[roleTitle]).Returns(roleId).Verifiable();
            _userRolesRepository.Setup(x => x.CreateAsync(It.Is<UserRoles>(x => x.RoleId == roleId && x.UserId == userId))).Verifiable();
            adminService.GiveRoleAsync(userId, roleTitle);

            _rolesHelper.Verify();
            _userRolesRepository.Verify();
        }

        [Test]
        public async Task RemoveRoleAsync_IfUserHaveRole_ShouldReturnTrue()
        {
            var roleTitle = _fixture.Create<string>();
            var roleId = _fixture.Create<Guid>();
            var userId = _fixture.Create<Guid>();
            var userRole = _fixture.Create<UserRoles>();
            var adminService = GetAdminService();

            _rolesHelper.Setup(x => x[roleTitle]).Returns(roleId).Verifiable();
            _userRolesRepository.Setup(x => x.GetByPredicate(It.IsAny<Expression<Func<UserRoles, bool>>>())).Returns(new List<UserRoles> { userRole}.AsQueryable()).Verifiable();
            _userRolesRepository.Setup(x => x.DeleteAsync(It.Is<UserRoles>(x => x.RoleId == userRole.RoleId && x.UserId == userRole.UserId))).ReturnsAsync(true).Verifiable();
            var response = await adminService.RemoveRoleAsync(userId, roleTitle);

            response.Should().BeTrue();
            _rolesHelper.Verify();
            _userRolesRepository.VerifyAll();
        }

        [Test]
        public async Task RemoveRoleAsync_IfUserDoesntHaveRole_ShouldReturnFalse()
        {
            var roleTitle = _fixture.Create<string>();
            var roleId = _fixture.Create<Guid>();
            var userId = _fixture.Create<Guid>();
            var userRole = _fixture.Create<UserRoles>();
            var adminService = GetAdminService();

            _rolesHelper.Setup(x => x[roleTitle]).Returns(roleId).Verifiable();
            _userRolesRepository.Setup(x => x.GetByPredicate(It.IsAny<Expression<Func<UserRoles, bool>>>())).Returns(new List<UserRoles> { }.AsQueryable()).Verifiable();
            var response = await adminService.RemoveRoleAsync(userId, roleTitle);

            response.Should().BeFalse();
            _rolesHelper.Verify();
            _userRolesRepository.Verify();
            _userRolesRepository.Verify(x=>x.DeleteAsync(It.IsAny<UserRoles>()),Times.Never);
        }
    }
}
