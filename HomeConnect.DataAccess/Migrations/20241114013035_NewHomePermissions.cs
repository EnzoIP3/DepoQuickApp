using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeConnect.DataAccess.Migrations;

/// <inheritdoc />
public partial class NewHomePermissions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Permissions",
            column: "Value",
            values: new object[]
            {
                "get-homes",
                "name-home"
            });

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "PermissionsValue", "RolesName" },
            values: new object[,]
            {
                { "get-homes", "HomeOwner" },
                { "name-home", "HomeOwner" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "PermissionsValue", "RolesName" },
            keyValues: new object[] { "get-homes", "HomeOwner" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "PermissionsValue", "RolesName" },
            keyValues: new object[] { "name-home", "HomeOwner" });

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Value",
            keyValue: "get-homes");

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Value",
            keyValue: "name-home");
    }
}
