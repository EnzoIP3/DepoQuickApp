using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations;

/// <inheritdoc />
public partial class ModelNumberToString : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "ModelNumber",
            table: "Devices",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddColumn<string>(
            name: "Validator",
            table: "Businesses",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Validator",
            table: "Businesses");

        migrationBuilder.AlterColumn<int>(
            name: "ModelNumber",
            table: "Devices",
            type: "int",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");
    }
}
