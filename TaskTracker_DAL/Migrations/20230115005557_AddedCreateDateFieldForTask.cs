using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker_DAL.Migrations
{
    public partial class AddedCreateDateFieldForTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRolesList",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("9d25f40b-88de-4e7f-b76b-74f87f26f654"), new Guid("9d25f40b-68de-4e7f-b76b-74f87f26f654") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("9d25f40b-88de-4e7f-b76b-74f87f26f654"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("4e506bed-0876-4e8b-a4ca-15d6167c5c97"), "Admin" });

            migrationBuilder.InsertData(
                table: "UserRolesList",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("4e506bed-0876-4e8b-a4ca-15d6167c5c97"), new Guid("9d25f40b-68de-4e7f-b76b-74f87f26f654") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRolesList",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("4e506bed-0876-4e8b-a4ca-15d6167c5c97"), new Guid("9d25f40b-68de-4e7f-b76b-74f87f26f654") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("4e506bed-0876-4e8b-a4ca-15d6167c5c97"));

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Tasks");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Title" },
                values: new object[] { new Guid("9d25f40b-88de-4e7f-b76b-74f87f26f654"), "Admin" });

            migrationBuilder.InsertData(
                table: "UserRolesList",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("9d25f40b-88de-4e7f-b76b-74f87f26f654"), new Guid("9d25f40b-68de-4e7f-b76b-74f87f26f654") });
        }
    }
}
