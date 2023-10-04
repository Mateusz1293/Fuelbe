using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelBe.Migrations
{
    public partial class addrefueling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "refueling",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    counter_status = table.Column<int>(type: "int", nullable: false),
                    add_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refueling", x => x.id);
                    table.ForeignKey(
                        name: "FK_refueling_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_refueling_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalSchema: "dbo",
                        principalTable: "vehicle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_refueling_user_id",
                schema: "dbo",
                table: "refueling",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_refueling_vehicle_id",
                schema: "dbo",
                table: "refueling",
                column: "vehicle_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refueling",
                schema: "dbo");
        }
    }
}
