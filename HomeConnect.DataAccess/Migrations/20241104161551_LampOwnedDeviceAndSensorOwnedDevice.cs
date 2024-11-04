using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeConnect.DataAccess.Migrations;

/// <inheritdoc />
public partial class LampOwnedDeviceAndSensorOwnedDevice : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "DeviceType",
            table: "OwnedDevices",
            type: "nvarchar(21)",
            maxLength: 21,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<bool>(
            name: "IsOpen",
            table: "OwnedDevices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "State",
            table: "OwnedDevices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NickName",
            table: "Homes",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.InsertData(
            table: "Permissions",
            column: "Value",
            values: new object[]
            {
                "create-lamp",
                "create-motion-sensor"
            });

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "PermissionsValue", "RolesName" },
            values: new object[,]
            {
                { "create-lamp", "BusinessOwner" },
                { "create-motion-sensor", "BusinessOwner" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "PermissionsValue", "RolesName" },
            keyValues: new object[] { "create-lamp", "BusinessOwner" });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "PermissionsValue", "RolesName" },
            keyValues: new object[] { "create-motion-sensor", "BusinessOwner" });

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Value",
            keyValue: "create-lamp");

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Value",
            keyValue: "create-motion-sensor");

        migrationBuilder.DropColumn(
            name: "DeviceType",
            table: "OwnedDevices");

        migrationBuilder.DropColumn(
            name: "IsOpen",
            table: "OwnedDevices");

        migrationBuilder.DropColumn(
            name: "State",
            table: "OwnedDevices");

        migrationBuilder.DropColumn(
            name: "NickName",
            table: "Homes");
    }
}
