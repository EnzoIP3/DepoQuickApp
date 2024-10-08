using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations;

/// <inheritdoc />
public partial class MemberHomePermissions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_HomePermission_Members_MemberId",
            table: "HomePermission");

        migrationBuilder.DropPrimaryKey(
            name: "PK_HomePermission",
            table: "HomePermission");

        migrationBuilder.DropIndex(
            name: "IX_HomePermission_MemberId",
            table: "HomePermission");

        migrationBuilder.DropColumn(
            name: "MemberId",
            table: "HomePermission");

        migrationBuilder.RenameTable(
            name: "HomePermission",
            newName: "HomePermissions");

        migrationBuilder.AddPrimaryKey(
            name: "PK_HomePermissions",
            table: "HomePermissions",
            column: "Value");

        migrationBuilder.CreateTable(
            name: "MemberHomePermissions",
            columns: table => new
            {
                HomePermissionsValue = table.Column<string>(type: "nvarchar(450)", nullable: false),
                MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MemberHomePermissions", x => new { x.HomePermissionsValue, x.MemberId });
                table.ForeignKey(
                    name: "FK_MemberHomePermissions_HomePermissions_HomePermissionsValue",
                    column: x => x.HomePermissionsValue,
                    principalTable: "HomePermissions",
                    principalColumn: "Value",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MemberHomePermissions_Members_MemberId",
                    column: x => x.MemberId,
                    principalTable: "Members",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MemberHomePermissions_MemberId",
            table: "MemberHomePermissions",
            column: "MemberId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MemberHomePermissions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_HomePermissions",
            table: "HomePermissions");

        migrationBuilder.RenameTable(
            name: "HomePermissions",
            newName: "HomePermission");

        migrationBuilder.AddColumn<Guid>(
            name: "MemberId",
            table: "HomePermission",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_HomePermission",
            table: "HomePermission",
            column: "Value");

        migrationBuilder.CreateIndex(
            name: "IX_HomePermission_MemberId",
            table: "HomePermission",
            column: "MemberId");

        migrationBuilder.AddForeignKey(
            name: "FK_HomePermission_Members_MemberId",
            table: "HomePermission",
            column: "MemberId",
            principalTable: "Members",
            principalColumn: "Id");
    }
}
