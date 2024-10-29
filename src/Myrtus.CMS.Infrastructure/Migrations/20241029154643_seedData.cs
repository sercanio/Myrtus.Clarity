using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Myrtus.CMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
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
                    name = table.Column<string>(type: "text", nullable: false)
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
                    first_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    last_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
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
                name: "role_permissions",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permission_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "fk_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permissions_roles_role_id",
                        column: x => x.role_id,
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
                name: "role_users",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_users", x => new { x.role_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_role_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_users_users_user_id",
                        column: x => x.user_id,
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
                        name: "fk_posts_blog_blog_id",
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

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "feature", "name" },
                values: new object[,]
                {
                    { new Guid("0eeb5f27-10fd-430a-9257-a8457107141a"), "permissions", "permissions:read" },
                    { new Guid("25bb194c-ea15-4339-9f45-5a895c51b626"), "users", "users:update" },
                    { new Guid("33261a4a-c423-4876-8f15-e40068aea5ca"), "users", "users:read" },
                    { new Guid("346d3cc6-ac81-42b1-8539-cd53f42b6566"), "roles", "roles:update" },
                    { new Guid("386e40e9-da38-4d2f-8d02-ac4cbaddf760"), "roles", "roles:delete" },
                    { new Guid("559dd4ec-4d2e-479d-a0a9-5229ecc04fb4"), "users", "users:delete" },
                    { new Guid("940c88ad-24fe-4d86-a982-fa5ea224edba"), "roles", "roles:create" },
                    { new Guid("9f79a54c-0b54-4de5-94b9-8582a5f32e78"), "users", "users:create" },
                    { new Guid("d066e4ee-6af2-4857-bd40-b9b058fa2201"), "roles", "roles:read" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "created_on_utc", "deleted_on_utc", "name", "updated_on_utc" },
                values: new object[,]
                {
                    { new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd"), new DateTime(2024, 10, 29, 15, 46, 43, 0, DateTimeKind.Utc).AddTicks(6907), null, "Admin", null },
                    { new Guid("5dc6ec47-5b7c-4c2b-86cd-3a671834e56e"), new DateTime(2024, 10, 29, 15, 46, 43, 0, DateTimeKind.Utc).AddTicks(6513), null, "Registered", null }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_on_utc", "deleted_on_utc", "email", "first_name", "identity_id", "last_name", "updated_on_utc" },
                values: new object[] { new Guid("69478a6a-d18c-4595-b913-ecd7be114fd2"), new DateTime(2024, 10, 29, 15, 46, 43, 399, DateTimeKind.Utc).AddTicks(1088), null, "admin@email.com", "Admin", "a67c921a-d8b5-4e1e-a741-ee021f6ba29f", "Admin", null });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "permission_id", "role_id" },
                values: new object[,]
                {
                    { new Guid("0eeb5f27-10fd-430a-9257-a8457107141a"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("25bb194c-ea15-4339-9f45-5a895c51b626"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("33261a4a-c423-4876-8f15-e40068aea5ca"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("346d3cc6-ac81-42b1-8539-cd53f42b6566"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("386e40e9-da38-4d2f-8d02-ac4cbaddf760"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("559dd4ec-4d2e-479d-a0a9-5229ecc04fb4"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("940c88ad-24fe-4d86-a982-fa5ea224edba"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("9f79a54c-0b54-4de5-94b9-8582a5f32e78"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("d066e4ee-6af2-4857-bd40-b9b058fa2201"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("33261a4a-c423-4876-8f15-e40068aea5ca"), new Guid("5dc6ec47-5b7c-4c2b-86cd-3a671834e56e") }
                });

            migrationBuilder.InsertData(
                table: "role_users",
                columns: new[] { "role_id", "user_id" },
                values: new object[,]
                {
                    { new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd"), new Guid("69478a6a-d18c-4595-b913-ecd7be114fd2") },
                    { new Guid("5dc6ec47-5b7c-4c2b-86cd-3a671834e56e"), new Guid("69478a6a-d18c-4595-b913-ecd7be114fd2") }
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
                name: "ix_posts_blog_id",
                table: "posts",
                column: "blog_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_users_user_id",
                table: "role_users",
                column: "user_id");

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
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "role_users");

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
