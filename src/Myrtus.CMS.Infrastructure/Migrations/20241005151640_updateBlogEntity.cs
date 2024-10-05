using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Myrtus.CMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateBlogEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "last_updated_by_id",
                table: "blogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_blogs_last_updated_by_id",
                table: "blogs",
                column: "last_updated_by_id");

            migrationBuilder.AddForeignKey(
                name: "fk_blogs_users_last_updated_by_id",
                table: "blogs",
                column: "last_updated_by_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_blogs_users_last_updated_by_id",
                table: "blogs");

            migrationBuilder.DropIndex(
                name: "ix_blogs_last_updated_by_id",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "last_updated_by_id",
                table: "blogs");
        }
    }
}
