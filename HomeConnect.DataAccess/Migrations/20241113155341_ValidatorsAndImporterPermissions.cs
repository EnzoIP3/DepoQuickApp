using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ValidatorsAndImporterPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                column: "Value",
                values: new object[]
                {
                    "get-device-import-files",
                    "get-device-importers",
                    "get-device-validators",
                    "import-devices",
                    "update-business-validator"
                });

            migrationBuilder.InsertData(
                table: "RoleSystemPermission",
                columns: new[] { "PermissionsValue", "RolesName" },
                values: new object[,]
                {
                    { "get-device-import-files", "BusinessOwner" },
                    { "get-device-importers", "BusinessOwner" },
                    { "get-device-validators", "BusinessOwner" },
                    { "import-devices", "BusinessOwner" },
                    { "update-business-validator", "BusinessOwner" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "PermissionsValue", "RolesName" },
                keyValues: new object[] { "get-device-import-files", "BusinessOwner" });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "PermissionsValue", "RolesName" },
                keyValues: new object[] { "get-device-importers", "BusinessOwner" });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "PermissionsValue", "RolesName" },
                keyValues: new object[] { "get-device-validators", "BusinessOwner" });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "PermissionsValue", "RolesName" },
                keyValues: new object[] { "import-devices", "BusinessOwner" });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "PermissionsValue", "RolesName" },
                keyValues: new object[] { "update-business-validator", "BusinessOwner" });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-device-import-files");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-device-importers");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-device-validators");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "import-devices");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "update-business-validator");
        }
    }
}
