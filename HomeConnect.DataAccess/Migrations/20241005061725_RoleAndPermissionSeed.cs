using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RoleAndPermissionSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Value", "RoleName" },
                values: new object[,]
                {
                    { "add-device", "HomeOwner" },
                    { "add-member", "HomeOwner" },
                    { "create-administrator", "Admin" },
                    { "create-business-owner", "Admin" },
                    { "create-home", "HomeOwner" },
                    { "delete-administrator", "Admin" },
                    { "get-all-businesses", "Admin" },
                    { "get-all-users", "Admin" },
                    { "get-devices", "HomeOwner" },
                    { "get-members", "HomeOwner" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "add-device");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "add-member");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-administrator");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-business-owner");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-home");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "delete-administrator");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-all-businesses");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-all-users");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-devices");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-members");
        }
    }
}
