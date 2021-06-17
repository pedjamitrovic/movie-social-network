using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSocialNetworkApi.Migrations
{
    public partial class MovieRatingsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieRatings_SystemEntities_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "SystemEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieRatings_OwnerId",
                table: "MovieRatings",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieRatings");
        }
    }
}
