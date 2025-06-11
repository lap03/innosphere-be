using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployerWorker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactLocation",
                table: "Workers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Workers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Workers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "Workers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Workers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalWebsite",
                table: "Workers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCoverUrl",
                table: "Employers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoUrl",
                table: "Employers",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanySize",
                table: "Employers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyWebsite",
                table: "Employers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfIncorporation",
                table: "Employers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactLocation",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "PersonalWebsite",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "CompanyCoverUrl",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "CompanyLogoUrl",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "CompanySize",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "CompanyWebsite",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "DateOfIncorporation",
                table: "Employers");
        }
    }
}
