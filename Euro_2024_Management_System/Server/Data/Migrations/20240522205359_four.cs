using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Euro_2024_Management_System.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class four : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GoalsHome",
                table: "Teams",
                newName: "GoalsScored");

            migrationBuilder.RenameColumn(
                name: "GoalsCount",
                table: "Teams",
                newName: "GoalsConceded");

            migrationBuilder.RenameColumn(
                name: "GoalsAway",
                table: "Teams",
                newName: "GoalBalance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GoalsScored",
                table: "Teams",
                newName: "GoalsHome");

            migrationBuilder.RenameColumn(
                name: "GoalsConceded",
                table: "Teams",
                newName: "GoalsCount");

            migrationBuilder.RenameColumn(
                name: "GoalBalance",
                table: "Teams",
                newName: "GoalsAway");
        }
    }
}
