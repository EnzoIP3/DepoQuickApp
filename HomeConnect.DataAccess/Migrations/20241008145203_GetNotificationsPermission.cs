using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations;

/// <inheritdoc />
public partial class GetNotificationsPermission : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsExterior",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "IsInterior",
            table: "Devices");

        migrationBuilder.InsertData(
            table: "Permissions",
            column: "Value",
            value: "get-notifications");

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "PermissionsValue", "RolesName" },
            values: new object[] { "get-notifications", "HomeOwner" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "PermissionsValue", "RolesName" },
            keyValues: new object[] { "get-notifications", "HomeOwner" });

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Value",
            keyValue: "get-notifications");

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
    }
}
