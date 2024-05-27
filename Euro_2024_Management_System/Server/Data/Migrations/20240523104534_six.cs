using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Euro_2024_Management_System.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class six : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointsScored",
                table: "Bets",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsScored",
                table: "Bets");
        }
    }
}
