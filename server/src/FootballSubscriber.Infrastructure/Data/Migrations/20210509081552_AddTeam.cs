using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballSubscriber.Infrastructure.Data.Migrations
{
    public partial class AddTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwayTeamApiId",
                table: "Fixture",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamApiId",
                table: "Fixture",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_AwayTeamId",
                table: "Fixture",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_HomeTeamId",
                table: "Fixture",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_ApiId",
                table: "Team",
                column: "ApiId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixture_Team_AwayTeamId",
                table: "Fixture",
                column: "AwayTeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixture_Team_HomeTeamId",
                table: "Fixture",
                column: "HomeTeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixture_Team_AwayTeamId",
                table: "Fixture");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixture_Team_HomeTeamId",
                table: "Fixture");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Fixture_AwayTeamId",
                table: "Fixture");

            migrationBuilder.DropIndex(
                name: "IX_Fixture_HomeTeamId",
                table: "Fixture");

            migrationBuilder.DropColumn(
                name: "AwayTeamApiId",
                table: "Fixture");

            migrationBuilder.DropColumn(
                name: "HomeTeamApiId",
                table: "Fixture");
        }
    }
}
