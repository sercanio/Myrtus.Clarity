using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Myrtus.Clarity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNotificationPreferences1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPushNotificationEnabled",
                table: "users",
                newName: "push_notification");

            migrationBuilder.RenameColumn(
                name: "IsInAppEnabled",
                table: "users",
                newName: "in_app_notification");

            migrationBuilder.RenameColumn(
                name: "IsEmailEnabled",
                table: "users",
                newName: "email_notification");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "push_notification",
                table: "users",
                newName: "IsPushNotificationEnabled");

            migrationBuilder.RenameColumn(
                name: "in_app_notification",
                table: "users",
                newName: "IsInAppEnabled");

            migrationBuilder.RenameColumn(
                name: "email_notification",
                table: "users",
                newName: "IsEmailEnabled");
        }
    }
}
