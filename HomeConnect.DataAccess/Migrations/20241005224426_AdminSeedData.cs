using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations;

/// <inheritdoc />
public partial class AdminSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Roles_RoleName",
            table: "Users");

        migrationBuilder.AlterColumn<string>(
            name: "RoleName",
            table: "Users",
            type: "nvarchar(450)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "CreatedAt", "Email", "Name", "Password", "ProfilePicture", "RoleName", "Surname" },
            values: new object[] { new Guid("f1b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b"), new DateOnly(2024, 1, 1), "admin@admin.com", "Administrator", "Admin123@", null, "Admin", "Account" });

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Roles_RoleName",
            table: "Users",
            column: "RoleName",
            principalTable: "Roles",
            principalColumn: "Name",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Roles_RoleName",
            table: "Users");

        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("f1b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b"));

        migrationBuilder.AlterColumn<string>(
            name: "RoleName",
            table: "Users",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Roles_RoleName",
            table: "Users",
            column: "RoleName",
            principalTable: "Roles",
            principalColumn: "Name");
    }
}
