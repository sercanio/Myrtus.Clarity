using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Myrtus.CMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "outbox_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "jsonb", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    feature = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    identity_id = table.Column<string>(type: "text", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission_role",
                columns: table => new
                {
                    permissions_id = table.Column<Guid>(type: "uuid", nullable: false),
                    roles_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_role", x => new { x.permissions_id, x.roles_id });
                    table.ForeignKey(
                        name: "fk_permission_role_permission_permissions_id",
                        column: x => x.permissions_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_permission_role_role_roles_id",
                        column: x => x.roles_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blogs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    last_updated_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    update_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    deleted_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    delete_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_blogs", x => x.id);
                    table.ForeignKey(
                        name: "fk_blogs_users_deleted_by_id",
                        column: x => x.deleted_by_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_blogs_users_last_updated_by_id",
                        column: x => x.last_updated_by_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_blogs_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_user",
                columns: table => new
                {
                    roles_id = table.Column<Guid>(type: "uuid", nullable: false),
                    users_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_user", x => new { x.roles_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_role_user_role_roles_id",
                        column: x => x.roles_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_user_user_users_id",
                        column: x => x.users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    blog_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cover_image = table.Column<string>(type: "text", nullable: false),
                    card_image = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    reviewed = table.Column<bool>(type: "boolean", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_posts", x => x.id);
                    table.ForeignKey(
                        name: "fk_posts_blogs_blog_id",
                        column: x => x.blog_id,
                        principalTable: "blogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    post_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_comments_post_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_comments_post_post_id1",
                        column: x => x.post_id1,
                        principalTable: "posts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_blogs_deleted_by_id",
                table: "blogs",
                column: "deleted_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_blogs_id",
                table: "blogs",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_blogs_last_updated_by_id",
                table: "blogs",
                column: "last_updated_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_blogs_owner_id",
                table: "blogs",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_id",
                table: "comments",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_post_id",
                table: "comments",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_post_id1",
                table: "comments",
                column: "post_id1");

            migrationBuilder.CreateIndex(
                name: "ix_permission_role_roles_id",
                table: "permission_role",
                column: "roles_id");

            migrationBuilder.CreateIndex(
                name: "ix_posts_blog_id",
                table: "posts",
                column: "blog_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_user_users_id",
                table: "role_user",
                column: "users_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_identity_id",
                table: "users",
                column: "identity_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "outbox_messages");

            migrationBuilder.DropTable(
                name: "permission_role");

            migrationBuilder.DropTable(
                name: "role_user");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "blogs");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
