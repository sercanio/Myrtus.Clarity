using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Myrtus.CMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateBlogEntity1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "delete_reason",
                table: "blogs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by_id",
                table: "blogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "update_reason",
                table: "blogs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_blogs_deleted_by_id",
                table: "blogs",
                column: "deleted_by_id");

            migrationBuilder.AddForeignKey(
                name: "fk_blogs_users_deleted_by_id",
                table: "blogs",
                column: "deleted_by_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_blogs_users_deleted_by_id",
                table: "blogs");

            migrationBuilder.DropIndex(
                name: "ix_blogs_deleted_by_id",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "delete_reason",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "deleted_by_id",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "update_reason",
                table: "blogs");
        }
    }
}
