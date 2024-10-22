using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations;

/// <inheritdoc />
public partial class SeedAdminRole : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "UserRole",
            columns: new[] { "RolesName", "UsersId" },
            values: new object[] { "Admin", new Guid("f1b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b") });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "UserRole",
            keyColumns: new[] { "RolesName", "UsersId" },
            keyValues: new object[] { "Admin", new Guid("f1b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b") });
    }
}
