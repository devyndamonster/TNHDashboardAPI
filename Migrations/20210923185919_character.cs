using Microsoft.EntityFrameworkCore.Migrations;

namespace TNHDashboardAPI.Migrations
{
    public partial class character : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Character",
                table: "ScoreEntry",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Character",
                table: "ScoreEntry");
        }
    }
}
