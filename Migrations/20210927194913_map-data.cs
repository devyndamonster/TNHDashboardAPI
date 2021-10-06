using Microsoft.EntityFrameworkCore.Migrations;

namespace TNHDashboardAPI.Migrations
{
    public partial class mapdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoldPointLocations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplyPointLocations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapData");
        }
    }
}
