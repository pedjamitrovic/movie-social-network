using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSocialNetworkApi.Migrations
{
    public partial class MovieRatingsChangeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "MovieRatings",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "MovieRatings");
        }
    }
}
