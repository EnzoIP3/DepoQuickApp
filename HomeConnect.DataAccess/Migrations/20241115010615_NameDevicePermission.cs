using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NameDevicePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OwnedDevices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Permissions",
                column: "Value",
                value: "name-device");

            migrationBuilder.InsertData(
                table: "RoleSystemPermission",
                columns: new[] { "PermissionsValue", "RolesName" },
                values: new object[] { "name-device", "HomeOwner" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "PermissionsValue", "RolesName" },
                keyValues: new object[] { "name-device", "HomeOwner" });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "name-device");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OwnedDevices");
        }
    }
}
