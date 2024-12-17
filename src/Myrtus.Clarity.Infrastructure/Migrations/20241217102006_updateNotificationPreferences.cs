using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Myrtus.Clarity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNotificationPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailEnabled",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInAppEnabled",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPushNotificationEnabled",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailEnabled",
                table: "users");

            migrationBuilder.DropColumn(
                name: "IsInAppEnabled",
                table: "users");

            migrationBuilder.DropColumn(
                name: "IsPushNotificationEnabled",
                table: "users");
        }
    }
}
