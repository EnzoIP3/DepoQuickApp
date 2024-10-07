using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OwnedDeviceConnected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionState",
                table: "Devices");

            migrationBuilder.AddColumn<bool>(
                name: "Connected",
                table: "OwnedDevices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Connected",
                table: "OwnedDevices");

            migrationBuilder.AddColumn<bool>(
                name: "ConnectionState",
                table: "Devices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
