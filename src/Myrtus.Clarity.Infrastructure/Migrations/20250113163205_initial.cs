using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Myrtus.Clarity.Infrastructure.Migrations
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
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
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
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
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
                    in_app_notification = table.Column<bool>(type: "boolean", nullable: false),
                    email_notification = table.Column<bool>(type: "boolean", nullable: false),
                    push_notification = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_permission",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permission_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permission", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "FK_role_permission_permissions_PermissionId",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permission_roles_RoleId",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_user",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_user", x => new { x.role_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_role_user_roles_RoleId",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_user_users_UserId",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "created_by", "created_on_utc", "deleted_on_utc", "feature", "name", "updated_by", "updated_on_utc" },
                values: new object[,]
                {
                    { new Guid("0eeb5f27-10fd-430a-9257-a8457107141a"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1858), null, "permissions", "permissions:read", null, null },
                    { new Guid("25bb194c-ea15-4339-9f45-5a895c51b626"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1846), null, "users", "users:update", null, null },
                    { new Guid("3050d953-5dcf-4eb0-a18d-a3ce62a0dd3c"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1859), null, "auditlogs", "auditlogs:read", null, null },
                    { new Guid("33261a4a-c423-4876-8f15-e40068aea5ca"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(569), null, "users", "users:read", null, null },
                    { new Guid("346d3cc6-ac81-42b1-8539-cd53f42b6566"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1855), null, "roles", "roles:update", null, null },
                    { new Guid("386e40e9-da38-4d2f-8d02-ac4cbaddf760"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1856), null, "roles", "roles:delete", null, null },
                    { new Guid("559dd4ec-4d2e-479d-a0a9-5229ecc04fb4"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1849), null, "users", "users:delete", null, null },
                    { new Guid("940c88ad-24fe-4d86-a982-fa5ea224edba"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1853), null, "roles", "roles:create", null, null },
                    { new Guid("9f79a54c-0b54-4de5-94b9-8582a5f32e78"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1840), null, "users", "users:create", null, null },
                    { new Guid("a03a127b-9a03-46a0-b709-b6919f2598be"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1861), null, "notifications", "notifications:read", null, null },
                    { new Guid("a5585e9e-ec65-431b-9bb9-9bbc1663ebb8"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1862), null, "notifications", "notifications:update", null, null },
                    { new Guid("d066e4ee-6af2-4857-bd40-b9b058fa2201"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 797, DateTimeKind.Utc).AddTicks(1851), null, "roles", "roles:read", null, null }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "created_by", "created_on_utc", "deleted_on_utc", "is_default", "name", "updated_by", "updated_on_utc" },
                values: new object[] { new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 799, DateTimeKind.Utc).AddTicks(4150), null, false, "Admin", null, null });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_by", "created_on_utc", "deleted_on_utc", "identity_id", "updated_by", "updated_on_utc", "email", "first_name", "last_name", "email_notification", "in_app_notification", "push_notification" },
                values: new object[] { new Guid("55c7f429-0916-4d84-8b76-d45185d89aa7"), "System", new DateTime(2025, 1, 13, 16, 32, 4, 895, DateTimeKind.Utc).AddTicks(9898), null, "b3398ff2-1b43-4af7-812d-eb4347eecbb8", null, null, "sercanates91@gmail.com", "Sercan", "Ateş", true, true, true });

            migrationBuilder.InsertData(
                table: "role_permission",
                columns: new[] { "permission_id", "role_id" },
                values: new object[,]
                {
                    { new Guid("0eeb5f27-10fd-430a-9257-a8457107141a"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("25bb194c-ea15-4339-9f45-5a895c51b626"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("3050d953-5dcf-4eb0-a18d-a3ce62a0dd3c"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("33261a4a-c423-4876-8f15-e40068aea5ca"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("346d3cc6-ac81-42b1-8539-cd53f42b6566"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("386e40e9-da38-4d2f-8d02-ac4cbaddf760"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("559dd4ec-4d2e-479d-a0a9-5229ecc04fb4"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("940c88ad-24fe-4d86-a982-fa5ea224edba"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("9f79a54c-0b54-4de5-94b9-8582a5f32e78"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("a03a127b-9a03-46a0-b709-b6919f2598be"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("a5585e9e-ec65-431b-9bb9-9bbc1663ebb8"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") },
                    { new Guid("d066e4ee-6af2-4857-bd40-b9b058fa2201"), new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd") }
                });

            migrationBuilder.InsertData(
                table: "role_user",
                columns: new[] { "role_id", "user_id" },
                values: new object[] { new Guid("4b606d86-3537-475a-aa20-26aadd8f5cfd"), new Guid("55c7f429-0916-4d84-8b76-d45185d89aa7") });

            migrationBuilder.CreateIndex(
                name: "ix_role_permission_permission_id",
                table: "role_permission",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_user_user_id",
                table: "role_user",
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
                name: "outbox_messages");

            migrationBuilder.DropTable(
                name: "role_permission");

            migrationBuilder.DropTable(
                name: "role_user");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
