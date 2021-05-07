using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballSubscriber.Infrastructure.Data.Migrations
{
    public partial class AddCompetition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompetitionApiId",
                table: "Fixture",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Competition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competition", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_CompetitionId",
                table: "Fixture",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Competition_ApiId",
                table: "Competition",
                column: "ApiId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixture_Competition_CompetitionId",
                table: "Fixture",
                column: "CompetitionId",
                principalTable: "Competition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixture_Competition_CompetitionId",
                table: "Fixture");

            migrationBuilder.DropTable(
                name: "Competition");

            migrationBuilder.DropIndex(
                name: "IX_Fixture_CompetitionId",
                table: "Fixture");

            migrationBuilder.DropColumn(
                name: "CompetitionApiId",
                table: "Fixture");
        }
    }
}
