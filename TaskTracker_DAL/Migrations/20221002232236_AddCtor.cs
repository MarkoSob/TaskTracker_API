using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker_DAL.Migrations
{
    public partial class AddCtor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Staus",
                table: "Tasks",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tasks",
                newName: "Staus");
        }
    }
}
