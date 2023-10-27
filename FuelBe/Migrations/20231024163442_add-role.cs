using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelBe.Migrations
{
    public partial class addrole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "role",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userRole",
                schema: "dbo",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userRole", x => new { x.roleId, x.userId });
                    table.ForeignKey(
                        name: "FK_userRole_role_roleId",
                        column: x => x.roleId,
                        principalSchema: "dbo",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userRole_user_userId",
                        column: x => x.userId,
                        principalSchema: "dbo",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userRole_userId",
                schema: "dbo",
                table: "userRole",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "role",
                schema: "dbo");
        }
    }
}
