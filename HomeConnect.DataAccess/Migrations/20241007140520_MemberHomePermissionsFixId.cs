using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MemberHomePermissionsFixId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberHomePermissions_HomePermissions_HomePermissionsValue",
                table: "MemberHomePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberHomePermissions",
                table: "MemberHomePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HomePermissions",
                table: "HomePermissions");

            migrationBuilder.DropColumn(
                name: "HomePermissionsValue",
                table: "MemberHomePermissions");

            migrationBuilder.AddColumn<Guid>(
                name: "HomePermissionsId",
                table: "MemberHomePermissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "HomePermissions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "HomePermissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberHomePermissions",
                table: "MemberHomePermissions",
                columns: new[] { "HomePermissionsId", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomePermissions",
                table: "HomePermissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberHomePermissions_HomePermissions_HomePermissionsId",
                table: "MemberHomePermissions",
                column: "HomePermissionsId",
                principalTable: "HomePermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberHomePermissions_HomePermissions_HomePermissionsId",
                table: "MemberHomePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberHomePermissions",
                table: "MemberHomePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HomePermissions",
                table: "HomePermissions");

            migrationBuilder.DropColumn(
                name: "HomePermissionsId",
                table: "MemberHomePermissions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "HomePermissions");

            migrationBuilder.AddColumn<string>(
                name: "HomePermissionsValue",
                table: "MemberHomePermissions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "HomePermissions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberHomePermissions",
                table: "MemberHomePermissions",
                columns: new[] { "HomePermissionsValue", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomePermissions",
                table: "HomePermissions",
                column: "Value");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberHomePermissions_HomePermissions_HomePermissionsValue",
                table: "MemberHomePermissions",
                column: "HomePermissionsValue",
                principalTable: "HomePermissions",
                principalColumn: "Value",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
