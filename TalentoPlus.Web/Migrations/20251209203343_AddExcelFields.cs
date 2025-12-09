using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentoPlus.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddExcelFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "employees",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "employees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ProfessionalProfile",
                table: "employees",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "employees",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "ProfessionalProfile",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "employees");
        }
    }
}
