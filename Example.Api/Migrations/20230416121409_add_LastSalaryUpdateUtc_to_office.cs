using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_LastSalaryUpdateUtc_to_office : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastSalaryUpdateUtc",
                table: "Offices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Offices",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastSalaryUpdateUtc",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSalaryUpdateUtc",
                table: "Offices");
        }
    }
}
