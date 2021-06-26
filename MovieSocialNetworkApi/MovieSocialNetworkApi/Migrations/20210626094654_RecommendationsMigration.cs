using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSocialNetworkApi.Migrations
{
    public partial class RecommendationsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => new { x.MovieId, x.OwnerId });
                    table.ForeignKey(
                        name: "FK_Recommendations_SystemEntities_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "SystemEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_OwnerId",
                table: "Recommendations",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recommendations");
        }
    }
}
