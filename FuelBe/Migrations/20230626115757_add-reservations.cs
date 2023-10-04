using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelBe.Migrations
{
    public partial class addreservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reservation",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    date_from = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_to = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservation_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservation_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalSchema: "dbo",
                        principalTable: "vehicle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reservation_user_id",
                schema: "dbo",
                table: "reservation",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_vehicle_id",
                schema: "dbo",
                table: "reservation",
                column: "vehicle_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservation",
                schema: "dbo");
        }
    }
}
