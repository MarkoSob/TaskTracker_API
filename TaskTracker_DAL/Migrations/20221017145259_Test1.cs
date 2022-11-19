using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker_DAL.Migrations
{
    public partial class Test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "IsBlocked", "IsDeleted", "LastName", "Password" },
                values: new object[] { new Guid("9d25f40b-68de-4e7f-b76b-74f87f26f654"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin1@test.com", "Alex", false, false, "Reb", "FqVMUXnOTR7E35PcCLEFYLtpRq/fNRU6ceEA9DMhxvY=" });

            migrationBuilder.InsertData(
                table: "UserRolesList",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("9d25f40b-88de-4e7f-b76b-74f87f26f654"), new Guid("9d25f40b-68de-4e7f-b76b-74f87f26f654") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRolesList",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("9d25f40b-88de-4e7f-b76b-74f87f26f654"), new Guid("9d25f40b-68de-4e7f-b76b-74f87f26f654") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9d25f40b-68de-4e7f-b76b-74f87f26f654"));
        }
    }
}
