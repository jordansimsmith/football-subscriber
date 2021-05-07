using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballSubscriber.Infrastructure.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fixture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiId = table.Column<int>(type: "int", nullable: false),
                    CompetitionId = table.Column<int>(type: "int", nullable: false),
                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
                    HomeTeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeOrganisationId = table.Column<int>(type: "int", nullable: false),
                    HomeOrganisationLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayTeamId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AwayOrganisationId = table.Column<int>(type: "int", nullable: false),
                    AwayOrganisationLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    VenueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixture", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_ApiId",
                table: "Fixture",
                column: "ApiId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fixture");
        }
    }
}
