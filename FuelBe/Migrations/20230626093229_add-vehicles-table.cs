using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelBe.Migrations
{
    public partial class addvehiclestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vehicle",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    register_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vehicle_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gearbox = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    production_year = table.Column<int>(type: "int", nullable: false),
                    fuel_type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vehicle",
                schema: "dbo");
        }
    }
}
