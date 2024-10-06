using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Camera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Devices",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: string.Empty);

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

            migrationBuilder.AddColumn<bool>(
                name: "MotionDetection",
                table: "Devices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PersonDetection",
                table: "Devices",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "IsExterior",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "IsInterior",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "MotionDetection",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "PersonDetection",
                table: "Devices");
        }
    }
}
