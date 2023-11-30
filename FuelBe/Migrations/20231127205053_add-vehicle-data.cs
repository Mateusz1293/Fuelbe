using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelBe.Migrations
{
    public partial class addvehicledata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "inspection_to",
                schema: "dbo",
                table: "vehicle",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "insurance_to",
                schema: "dbo",
                table: "vehicle",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "tires_type",
                schema: "dbo",
                table: "vehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "inspection_to",
                schema: "dbo",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "insurance_to",
                schema: "dbo",
                table: "vehicle");

            migrationBuilder.DropColumn(
                name: "tires_type",
                schema: "dbo",
                table: "vehicle");
        }
    }
}
