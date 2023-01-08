using Microsoft.EntityFrameworkCore;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.RolesHelper;

namespace TaskTracker_DAL
{
    public class EfDbContext : DbContext
    {
        private IRolesHelper _rolesHelper;
        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRolesList { get; set; }
        public DbSet<EmailStatus> EmailStatuses { get; set; }
        public DbSet<UserImage> UserImages { get; set; }

        public EfDbContext(DbContextOptions options, IRolesHelper rolesHelper) : base(options)
        {
            _rolesHelper = rolesHelper;
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoles>()
                .HasKey(nameof(UserRoles.UserId), nameof(UserRoles.RoleId));

            foreach (var role in new RolesList())
            {
                modelBuilder.Entity<Role>().HasData
                   (new Role { Id = _rolesHelper[role], Title = role }) ;
            }

            modelBuilder.Entity<User>().HasData
               (new User
               {
                   Id = Guid.Parse("9d25f40b-68de-4e7f-b76b-74f87f26f654"),
                   CreationDate = new DateTime(2022, 01, 01),
                   Email = "admin1@test.com",
                   FirstName = "Alex",
                   LastName = "Reb",
                   IsBlocked = false,
                   IsDeleted = false,
                   Password = "FqVMUXnOTR7E35PcCLEFYLtpRq/fNRU6ceEA9DMhxvY=",
               });

            modelBuilder.Entity<UserRoles>().HasData
                (new UserRoles
                {
                    UserId = Guid.Parse("9d25f40b-68de-4e7f-b76b-74f87f26f654"),
                    RoleId = _rolesHelper[RolesList.Admin]
                });
        }
    }
}
