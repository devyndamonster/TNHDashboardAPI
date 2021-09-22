using Microsoft.EntityFrameworkCore.Migrations;

namespace TNHDashboardAPI.Migrations
{
    public partial class startdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoreEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Map = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GameLength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoldActions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoldStats = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreEntry", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreEntry");
        }
    }
}
