using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class ProfileId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                table: "Dishes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Dishes");
        }
    }
}
