using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeConnect.DataAccess.Migrations;

/// <inheritdoc />
public partial class GetBusinessOwnerBusinessAndDevicesPermissions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Permissions",
            column: "Value",
            values: new object[]
            {
                "get-business-devices",
                "get-businesses",
                "get-camera"
            });

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "PermissionsValue", "RolesName" },
            values: new object[,]
            {
                { "get-business-devices", "BusinessOwner" },
                { "get-businesses", "BusinessOwner" },
                { "get-camera", "BusinessOwner" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "PermissionsValue", "RolesName" },
            keyValues: new object[] { "get-business-devices", "BusinessOwner" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "PermissionsValue", "RolesName" },
            keyValues: new object[] { "get-businesses", "BusinessOwner" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "PermissionsValue", "RolesName" },
            keyValues: new object[] { "get-camera", "BusinessOwner" });

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Value",
            keyValue: "get-business-devices");

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Value",
            keyValue: "get-businesses");

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Value",
            keyValue: "get-camera");
    }
}
