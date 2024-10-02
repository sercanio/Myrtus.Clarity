using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Myrtus.CMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTimeStampsBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_on_utc",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_on_utc",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_on_utc",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on_utc",
                table: "posts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_on_utc",
                table: "posts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_on_utc",
                table: "posts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on_utc",
                table: "comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_on_utc",
                table: "comments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_on_utc",
                table: "comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on_utc",
                table: "blogs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_on_utc",
                table: "blogs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_on_utc",
                table: "blogs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "ix_comments_id",
                table: "comments",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_blogs_id",
                table: "blogs",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_comments_id",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "ix_blogs_id",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "created_on_utc",
                table: "users");

            migrationBuilder.DropColumn(
                name: "deleted_on_utc",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_on_utc",
                table: "users");

            migrationBuilder.DropColumn(
                name: "created_on_utc",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "deleted_on_utc",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "updated_on_utc",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "created_on_utc",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "deleted_on_utc",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "updated_on_utc",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "created_on_utc",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "deleted_on_utc",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "updated_on_utc",
                table: "blogs");
        }
    }
}
