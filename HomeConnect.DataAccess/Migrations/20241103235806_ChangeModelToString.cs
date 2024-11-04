using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeModelToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Homes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModelNumber",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Homes");

            migrationBuilder.AlterColumn<int>(
                name: "ModelNumber",
                table: "Devices",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
