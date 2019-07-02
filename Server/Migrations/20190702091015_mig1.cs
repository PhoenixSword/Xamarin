using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Profiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Profiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Profiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Profiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Profiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Profiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Profiles");
        }
    }
}
