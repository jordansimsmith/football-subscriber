using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FootballSubscriber.Infrastructure.Data.Migrations
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competition",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        ApiId = table.Column<long>(type: "bigint", nullable: false),
                        Name = table.Column<string>(type: "text", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competition", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        UserId = table.Column<string>(type: "text", nullable: false),
                        TeamName = table.Column<string>(type: "text", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Fixture",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        ApiId = table.Column<long>(type: "bigint", nullable: false),
                        CompetitionId = table.Column<int>(type: "integer", nullable: false),
                        CompetitionApiId = table.Column<long>(type: "bigint", nullable: false),
                        HomeTeamName = table.Column<string>(type: "text", nullable: false),
                        HomeOrganisationLogo = table.Column<string>(type: "text", nullable: true),
                        HomeScore = table.Column<int>(type: "integer", nullable: true),
                        AwayTeamName = table.Column<string>(type: "text", nullable: false),
                        AwayOrganisationLogo = table.Column<string>(type: "text", nullable: true),
                        AwayScore = table.Column<int>(type: "integer", nullable: true),
                        Date = table.Column<DateTime>(
                            type: "timestamp with time zone",
                            nullable: false
                        ),
                        Status = table.Column<string>(type: "text", nullable: true),
                        VenueName = table.Column<string>(type: "text", nullable: false),
                        Address = table.Column<string>(type: "text", nullable: true),
                        Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                        Latitude = table.Column<decimal>(type: "numeric", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fixture_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Competition_ApiId",
                table: "Competition",
                column: "ApiId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_ApiId",
                table: "Fixture",
                column: "ApiId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_CompetitionId",
                table: "Fixture",
                column: "CompetitionId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_TeamName_UserId",
                table: "Subscription",
                columns: new[] { "TeamName", "UserId" },
                unique: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Fixture");

            migrationBuilder.DropTable(name: "Subscription");

            migrationBuilder.DropTable(name: "Competition");
        }
    }
}
