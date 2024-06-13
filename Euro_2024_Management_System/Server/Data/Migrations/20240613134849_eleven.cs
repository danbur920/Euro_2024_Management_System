using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Euro_2024_Management_System.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class eleven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialBets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialBets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSpecialBets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialBetId = table.Column<int>(type: "int", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: true),
                    IsSettled = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSpecialBets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSpecialBets_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserSpecialBets_SpecialBets_SpecialBetId",
                        column: x => x.SpecialBetId,
                        principalTable: "SpecialBets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecialBets_ApplicationUserId",
                table: "UserSpecialBets",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecialBets_SpecialBetId",
                table: "UserSpecialBets",
                column: "SpecialBetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSpecialBets");

            migrationBuilder.DropTable(
                name: "SpecialBets");
        }
    }
}
