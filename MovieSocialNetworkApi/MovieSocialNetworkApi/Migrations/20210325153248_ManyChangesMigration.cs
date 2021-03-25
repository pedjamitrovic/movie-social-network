using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSocialNetworkApi.Migrations
{
    public partial class ManyChangesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_SystemEntities_OwnerGroupId",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "OwnerGroupId",
                table: "Contents",
                newName: "ForGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_OwnerGroupId",
                table: "Contents",
                newName: "IX_Contents_ForGroupId");

            migrationBuilder.AddColumn<int>(
                name: "IssuedBanId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reviewed",
                table: "Reports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "GroupAdmins",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAdmins", x => new { x.GroupId, x.AdminId });
                    table.ForeignKey(
                        name: "FK_GroupAdmins_SystemEntities_AdminId",
                        column: x => x.AdminId,
                        principalTable: "SystemEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupAdmins_SystemEntities_GroupId",
                        column: x => x.GroupId,
                        principalTable: "SystemEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_IssuedBanId",
                table: "Reports",
                column: "IssuedBanId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAdmins_AdminId",
                table: "GroupAdmins",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_SystemEntities_ForGroupId",
                table: "Contents",
                column: "ForGroupId",
                principalTable: "SystemEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Bans_IssuedBanId",
                table: "Reports",
                column: "IssuedBanId",
                principalTable: "Bans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_SystemEntities_ForGroupId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Bans_IssuedBanId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "GroupAdmins");

            migrationBuilder.DropIndex(
                name: "IX_Reports_IssuedBanId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IssuedBanId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Reviewed",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "ForGroupId",
                table: "Contents",
                newName: "OwnerGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_ForGroupId",
                table: "Contents",
                newName: "IX_Contents_OwnerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_SystemEntities_OwnerGroupId",
                table: "Contents",
                column: "OwnerGroupId",
                principalTable: "SystemEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
