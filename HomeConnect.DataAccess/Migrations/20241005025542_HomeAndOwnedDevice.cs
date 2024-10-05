using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HomeAndOwnedDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Home_Users_OwnerId",
                table: "Home");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Home_HomeId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_OwnedDevice_OwnedDeviceHardwareId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedDevice_Devices_DeviceId",
                table: "OwnedDevice");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedDevice_Home_HomeId",
                table: "OwnedDevice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedDevice",
                table: "OwnedDevice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Home",
                table: "Home");

            migrationBuilder.RenameTable(
                name: "OwnedDevice",
                newName: "OwnedDevices");

            migrationBuilder.RenameTable(
                name: "Home",
                newName: "Homes");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedDevice_HomeId",
                table: "OwnedDevices",
                newName: "IX_OwnedDevices_HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedDevice_DeviceId",
                table: "OwnedDevices",
                newName: "IX_OwnedDevices_DeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_Home_OwnerId",
                table: "Homes",
                newName: "IX_Homes_OwnerId");

            migrationBuilder.AddColumn<bool>(
                name: "ConnectionState",
                table: "Devices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedDevices",
                table: "OwnedDevices",
                column: "HardwareId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Homes",
                table: "Homes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_Users_OwnerId",
                table: "Homes",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Homes_HomeId",
                table: "Member",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_OwnedDevices_OwnedDeviceHardwareId",
                table: "Notifications",
                column: "OwnedDeviceHardwareId",
                principalTable: "OwnedDevices",
                principalColumn: "HardwareId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedDevices_Devices_DeviceId",
                table: "OwnedDevices",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedDevices_Homes_HomeId",
                table: "OwnedDevices",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homes_Users_OwnerId",
                table: "Homes");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Homes_HomeId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_OwnedDevices_OwnedDeviceHardwareId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedDevices_Devices_DeviceId",
                table: "OwnedDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedDevices_Homes_HomeId",
                table: "OwnedDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedDevices",
                table: "OwnedDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Homes",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "ConnectionState",
                table: "Devices");

            migrationBuilder.RenameTable(
                name: "OwnedDevices",
                newName: "OwnedDevice");

            migrationBuilder.RenameTable(
                name: "Homes",
                newName: "Home");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedDevices_HomeId",
                table: "OwnedDevice",
                newName: "IX_OwnedDevice_HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedDevices_DeviceId",
                table: "OwnedDevice",
                newName: "IX_OwnedDevice_DeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_Homes_OwnerId",
                table: "Home",
                newName: "IX_Home_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedDevice",
                table: "OwnedDevice",
                column: "HardwareId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Home",
                table: "Home",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Home_Users_OwnerId",
                table: "Home",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Home_HomeId",
                table: "Member",
                column: "HomeId",
                principalTable: "Home",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_OwnedDevice_OwnedDeviceHardwareId",
                table: "Notifications",
                column: "OwnedDeviceHardwareId",
                principalTable: "OwnedDevice",
                principalColumn: "HardwareId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedDevice_Devices_DeviceId",
                table: "OwnedDevice",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedDevice_Home_HomeId",
                table: "OwnedDevice",
                column: "HomeId",
                principalTable: "Home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
