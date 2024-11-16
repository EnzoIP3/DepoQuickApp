using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Rooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "OwnedDevices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                column: "Value",
                values: new object[]
                {
                    "add-device-to-room",
                    "create-room",
                    "move-device"
                });

            migrationBuilder.InsertData(
                table: "RoleSystemPermission",
                columns: new[] { "PermissionsValue", "RolesName" },
                values: new object[,]
                {
                    { "add-device-to-room", "HomeOwner" },
                    { "create-room", "HomeOwner" },
                    { "move-device", "HomeOwner" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OwnedDevices_RoomId",
                table: "OwnedDevices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HomeId",
                table: "Rooms",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedDevices_Rooms_RoomId",
                table: "OwnedDevices",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedDevices_Rooms_RoomId",
                table: "OwnedDevices");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_OwnedDevices_RoomId",
                table: "OwnedDevices");

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "PermissionsValue", "RolesName" },
                keyValues: new object[] { "add-device-to-room", "HomeOwner" });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "PermissionsValue", "RolesName" },
                keyValues: new object[] { "create-room", "HomeOwner" });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "PermissionsValue", "RolesName" },
                keyValues: new object[] { "move-device", "HomeOwner" });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "add-device-to-room");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-room");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "move-device");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "OwnedDevices");
        }
    }
}
