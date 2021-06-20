using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSocialNetworkApi.Migrations
{
    public partial class MovieRatingIdRemovedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieRatings_SystemEntities_OwnerId",
                table: "MovieRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieRatings",
                table: "MovieRatings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MovieRatings");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "MovieRatings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "MovieRatings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieRatings",
                table: "MovieRatings",
                columns: new[] { "MovieId", "OwnerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieRatings_SystemEntities_OwnerId",
                table: "MovieRatings",
                column: "OwnerId",
                principalTable: "SystemEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieRatings_SystemEntities_OwnerId",
                table: "MovieRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieRatings",
                table: "MovieRatings");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "MovieRatings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "MovieRatings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MovieRatings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieRatings",
                table: "MovieRatings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieRatings_SystemEntities_OwnerId",
                table: "MovieRatings",
                column: "OwnerId",
                principalTable: "SystemEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
