using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations;

/// <inheritdoc />
public partial class UserRoles : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Roles_RoleName",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_RoleName",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "RoleName",
            table: "Users");

        migrationBuilder.CreateTable(
            name: "UserRole",
            columns: table => new
            {
                RolesName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRole", x => new { x.RolesName, x.UsersId });
                table.ForeignKey(
                    name: "FK_UserRole_Roles_RolesName",
                    column: x => x.RolesName,
                    principalTable: "Roles",
                    principalColumn: "Name",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserRole_Users_UsersId",
                    column: x => x.UsersId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_UserRole_UsersId",
            table: "UserRole",
            column: "UsersId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserRole");

        migrationBuilder.AddColumn<string>(
            name: "RoleName",
            table: "Users",
            type: "nvarchar(450)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("f1b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b"),
            column: "RoleName",
            value: "Admin");

        migrationBuilder.CreateIndex(
            name: "IX_Users_RoleName",
            table: "Users",
            column: "RoleName");

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Roles_RoleName",
            table: "Users",
            column: "RoleName",
            principalTable: "Roles",
            principalColumn: "Name",
            onDelete: ReferentialAction.Cascade);
    }
}
