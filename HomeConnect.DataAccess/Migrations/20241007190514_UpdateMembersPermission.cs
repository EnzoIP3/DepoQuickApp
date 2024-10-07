using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMembersPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                column: "Value",
                value: "update-member");

            migrationBuilder.InsertData(
                table: "RoleSystemPermission",
                columns: new[] { "PermissionsValue", "RolesName" },
                values: new object[] { "update-member", "HomeOwner" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "PermissionsValue", "RolesName" },
                keyValues: new object[] { "update-member", "HomeOwner" });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "update-member");
        }
    }
}
