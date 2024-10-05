using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeConnect.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BusinessOwnerPermissionsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Value", "RoleName" },
                values: new object[,]
                {
                    { "create-business", "BusinessOwner" },
                    { "create-camera", "BusinessOwner" },
                    { "create-sensor", "BusinessOwner" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-business");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-camera");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Value",
                keyValue: "create-sensor");
        }
    }
}
