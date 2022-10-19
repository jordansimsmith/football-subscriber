using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FootballSubscriber.Infrastructure.Data.Migrations
{
    public partial class RemoveTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fixture");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Competition");

            migrationBuilder.DropTable(
                name: "Team");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApiId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApiId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fixture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AwayTeamId = table.Column<int>(type: "integer", nullable: false),
                    CompetitionId = table.Column<int>(type: "integer", nullable: false),
                    HomeTeamId = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    ApiId = table.Column<long>(type: "bigint", nullable: false),
                    AwayOrganisationId = table.Column<int>(type: "integer", nullable: false),
                    AwayOrganisationLogo = table.Column<string>(type: "text", nullable: true),
                    AwayTeamApiId = table.Column<long>(type: "bigint", nullable: false),
                    AwayTeamName = table.Column<string>(type: "text", nullable: false),
                    CompetitionApiId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HomeOrganisationId = table.Column<int>(type: "integer", nullable: false),
                    HomeOrganisationLogo = table.Column<string>(type: "text", nullable: true),
                    HomeTeamApiId = table.Column<long>(type: "bigint", nullable: false),
                    HomeTeamName = table.Column<string>(type: "text", nullable: false),
                    VenueId = table.Column<int>(type: "integer", nullable: false),
                    VenueName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fixture_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fixture_Team_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixture_Team_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeamId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Competition_ApiId",
                table: "Competition",
                column: "ApiId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_ApiId",
                table: "Fixture",
                column: "ApiId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_AwayTeamId",
                table: "Fixture",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_CompetitionId",
                table: "Fixture",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_HomeTeamId",
                table: "Fixture",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_TeamId_UserId",
                table: "Subscription",
                columns: new[] { "TeamId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_ApiId",
                table: "Team",
                column: "ApiId",
                unique: true);
        }
    }
}
