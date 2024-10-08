using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations;

/// <inheritdoc />
public partial class Member : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_HomePermission_Member_MemberId",
            table: "HomePermission");

        migrationBuilder.DropForeignKey(
            name: "FK_Member_Homes_HomeId",
            table: "Member");

        migrationBuilder.DropForeignKey(
            name: "FK_Member_Users_UserId",
            table: "Member");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Member",
            table: "Member");

        migrationBuilder.RenameTable(
            name: "Member",
            newName: "Members");

        migrationBuilder.RenameIndex(
            name: "IX_Member_UserId",
            table: "Members",
            newName: "IX_Members_UserId");

        migrationBuilder.RenameIndex(
            name: "IX_Member_HomeId",
            table: "Members",
            newName: "IX_Members_HomeId");

        migrationBuilder.AddColumn<string>(
            name: "Discriminator",
            table: "Devices",
            type: "nvarchar(8)",
            maxLength: 8,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<bool>(
            name: "IsExterior",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsInterior",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "MotionDetection",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "PersonDetection",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "HomeId",
            table: "Members",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_Members",
            table: "Members",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_HomePermission_Members_MemberId",
            table: "HomePermission",
            column: "MemberId",
            principalTable: "Members",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Members_Homes_HomeId",
            table: "Members",
            column: "HomeId",
            principalTable: "Homes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Members_Users_UserId",
            table: "Members",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_HomePermission_Members_MemberId",
            table: "HomePermission");

        migrationBuilder.DropForeignKey(
            name: "FK_Members_Homes_HomeId",
            table: "Members");

        migrationBuilder.DropForeignKey(
            name: "FK_Members_Users_UserId",
            table: "Members");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Members",
            table: "Members");

        migrationBuilder.DropColumn(
            name: "Discriminator",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "IsExterior",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "IsInterior",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "MotionDetection",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "PersonDetection",
            table: "Devices");

        migrationBuilder.RenameTable(
            name: "Members",
            newName: "Member");

        migrationBuilder.RenameIndex(
            name: "IX_Members_UserId",
            table: "Member",
            newName: "IX_Member_UserId");

        migrationBuilder.RenameIndex(
            name: "IX_Members_HomeId",
            table: "Member",
            newName: "IX_Member_HomeId");

        migrationBuilder.AlterColumn<Guid>(
            name: "HomeId",
            table: "Member",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Member",
            table: "Member",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_HomePermission_Member_MemberId",
            table: "HomePermission",
            column: "MemberId",
            principalTable: "Member",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Member_Homes_HomeId",
            table: "Member",
            column: "HomeId",
            principalTable: "Homes",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Member_Users_UserId",
            table: "Member",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
