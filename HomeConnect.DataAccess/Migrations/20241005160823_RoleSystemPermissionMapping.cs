using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RoleSystemPermissionMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Roles_RoleName",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_RoleName",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "Permissions");

            migrationBuilder.CreateTable(
                name: "RoleSystemPermission",
                columns: table => new
                {
                    PermissionsValue = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RolesName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleSystemPermission", x => new { x.PermissionsValue, x.RolesName });
                    table.ForeignKey(
                        name: "FK_RoleSystemPermission_Permissions_PermissionsValue",
                        column: x => x.PermissionsValue,
                        principalTable: "Permissions",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleSystemPermission_Roles_RolesName",
                        column: x => x.RolesName,
                        principalTable: "Roles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RoleSystemPermission",
                columns: new[] { "PermissionsValue", "RolesName" },
                values: new object[,]
                {
                    { "add-device", "HomeOwner" },
                    { "add-member", "HomeOwner" },
                    { "create-administrator", "Admin" },
                    { "create-business", "BusinessOwner" },
                    { "create-business-owner", "Admin" },
                    { "create-camera", "BusinessOwner" },
                    { "create-home", "HomeOwner" },
                    { "create-sensor", "BusinessOwner" },
                    { "delete-administrator", "Admin" },
                    { "get-all-businesses", "Admin" },
                    { "get-all-users", "Admin" },
                    { "get-devices", "HomeOwner" },
                    { "get-members", "HomeOwner" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleSystemPermission_RolesName",
                table: "RoleSystemPermission",
                column: "RolesName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleSystemPermission");

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "Permissions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "add-device",
                column: "RoleName",
                value: "HomeOwner");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "add-member",
                column: "RoleName",
                value: "HomeOwner");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-administrator",
                column: "RoleName",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-business",
                column: "RoleName",
                value: "BusinessOwner");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-business-owner",
                column: "RoleName",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-camera",
                column: "RoleName",
                value: "BusinessOwner");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-home",
                column: "RoleName",
                value: "HomeOwner");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-sensor",
                column: "RoleName",
                value: "BusinessOwner");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "delete-administrator",
                column: "RoleName",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-all-businesses",
                column: "RoleName",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-all-users",
                column: "RoleName",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-devices",
                column: "RoleName",
                value: "HomeOwner");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "get-members",
                column: "RoleName",
                value: "HomeOwner");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleName",
                table: "Permissions",
                column: "RoleName");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Roles_RoleName",
                table: "Permissions",
                column: "RoleName",
                principalTable: "Roles",
                principalColumn: "Name");
        }
    }
}
