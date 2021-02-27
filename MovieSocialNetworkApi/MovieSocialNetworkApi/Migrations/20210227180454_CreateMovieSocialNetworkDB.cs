using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSocialNetworkApi.Migrations
{
    public partial class CreateMovieSocialNetworkDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbstractUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subtitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbstractUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbstractContents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: true),
                    CreatorId = table.Column<long>(type: "bigint", nullable: true),
                    OwnerGroupId = table.Column<long>(type: "bigint", nullable: true),
                    Post_CreatorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbstractContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbstractContents_AbstractContents_PostId",
                        column: x => x.PostId,
                        principalTable: "AbstractContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractContents_AbstractUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AbstractUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractContents_AbstractUsers_OwnerGroupId",
                        column: x => x.OwnerGroupId,
                        principalTable: "AbstractUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractContents_AbstractUsers_Post_CreatorId",
                        column: x => x.Post_CreatorId,
                        principalTable: "AbstractUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: true),
                    ContentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_AbstractContents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "AbstractContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reactions_AbstractUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AbstractUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbstractContents_CreatorId",
                table: "AbstractContents",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractContents_OwnerGroupId",
                table: "AbstractContents",
                column: "OwnerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractContents_Post_CreatorId",
                table: "AbstractContents",
                column: "Post_CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractContents_PostId",
                table: "AbstractContents",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ContentId",
                table: "Reactions",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_OwnerId",
                table: "Reactions",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "AbstractContents");

            migrationBuilder.DropTable(
                name: "AbstractUsers");
        }
    }
}
