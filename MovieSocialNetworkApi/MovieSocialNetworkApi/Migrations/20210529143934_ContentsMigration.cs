using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSocialNetworkApi.Migrations
{
    public partial class ContentsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_SystemEntities_Post_CreatorId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_Post_CreatorId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "Post_CreatorId",
                table: "Contents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Post_CreatorId",
                table: "Contents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Post_CreatorId",
                table: "Contents",
                column: "Post_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_SystemEntities_Post_CreatorId",
                table: "Contents",
                column: "Post_CreatorId",
                principalTable: "SystemEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
