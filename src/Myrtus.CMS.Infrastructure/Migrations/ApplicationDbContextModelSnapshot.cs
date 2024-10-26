﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Myrtus.CMS.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Myrtus.CMS.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Myrtus.CMS.Domain.Blogs.Blog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<string>("DeleteReason")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("delete_reason");

                    b.Property<Guid?>("DeletedById")
                        .HasColumnType("uuid")
                        .HasColumnName("deleted_by_id");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_on_utc");

                    b.Property<Guid?>("LastUpdatedById")
                        .HasColumnType("uuid")
                        .HasColumnName("last_updated_by_id");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.Property<string>("UpdateReason")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("update_reason");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_on_utc");

                    b.HasKey("Id")
                        .HasName("pk_blogs");

                    b.HasIndex("DeletedById")
                        .HasDatabaseName("ix_blogs_deleted_by_id");

                    b.HasIndex("Id")
                        .HasDatabaseName("ix_blogs_id");

                    b.HasIndex("LastUpdatedById")
                        .HasDatabaseName("ix_blogs_last_updated_by_id");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("ix_blogs_owner_id");

                    b.ToTable("blogs", (string)null);
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Blogs.Posts.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_on_utc");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid")
                        .HasColumnName("post_id");

                    b.Property<Guid?>("PostId1")
                        .HasColumnType("uuid")
                        .HasColumnName("post_id1");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_on_utc");

                    b.HasKey("Id")
                        .HasName("pk_comments");

                    b.HasIndex("Id")
                        .HasDatabaseName("ix_comments_id");

                    b.HasIndex("PostId")
                        .HasDatabaseName("ix_comments_post_id");

                    b.HasIndex("PostId1")
                        .HasDatabaseName("ix_comments_post_id1");

                    b.ToTable("comments", (string)null);
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Blogs.Posts.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BlogId")
                        .HasColumnType("uuid")
                        .HasColumnName("blog_id");

                    b.Property<Uri>("CardImage")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("card_image");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<Uri>("CoverImage")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("cover_image");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_on_utc");

                    b.Property<bool>("Reviewed")
                        .HasColumnType("boolean")
                        .HasColumnName("reviewed");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("summary");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_on_utc");

                    b.HasKey("Id")
                        .HasName("pk_posts");

                    b.HasIndex("BlogId")
                        .HasDatabaseName("ix_posts_blog_id");

                    b.ToTable("posts", (string)null);
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Users.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Feature")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("feature");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_permissions");

                    b.ToTable("permissions", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("33261a4a-c423-4876-8f15-e40068aea5ca"),
                            Feature = "users",
                            Name = "users:read"
                        },
                        new
                        {
                            Id = new Guid("9f79a54c-0b54-4de5-94b9-8582a5f32e78"),
                            Feature = "users",
                            Name = "users:create"
                        },
                        new
                        {
                            Id = new Guid("25bb194c-ea15-4339-9f45-5a895c51b626"),
                            Feature = "users",
                            Name = "users:update"
                        },
                        new
                        {
                            Id = new Guid("559dd4ec-4d2e-479d-a0a9-5229ecc04fb4"),
                            Feature = "users",
                            Name = "users:delete"
                        },
                        new
                        {
                            Id = new Guid("d066e4ee-6af2-4857-bd40-b9b058fa2201"),
                            Feature = "roles",
                            Name = "roles:read"
                        },
                        new
                        {
                            Id = new Guid("940c88ad-24fe-4d86-a982-fa5ea224edba"),
                            Feature = "roles",
                            Name = "roles:create"
                        },
                        new
                        {
                            Id = new Guid("346d3cc6-ac81-42b1-8539-cd53f42b6566"),
                            Feature = "roles",
                            Name = "roles:update"
                        },
                        new
                        {
                            Id = new Guid("386e40e9-da38-4d2f-8d02-ac4cbaddf760"),
                            Feature = "roles",
                            Name = "roles:delete"
                        },
                        new
                        {
                            Id = new Guid("0eeb5f27-10fd-430a-9257-a8457107141a"),
                            Feature = "permissions",
                            Name = "permissions:read"
                        });
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Users.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_on_utc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_on_utc");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("5dc6ec47-5b7c-4c2b-86cd-3a671834e56e"),
                            CreatedOnUtc = new DateTime(2024, 10, 25, 10, 14, 41, 899, DateTimeKind.Utc).AddTicks(2869),
                            Name = "Registered"
                        });
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Users.RolePermission", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uuid")
                        .HasColumnName("permission_id");

                    b.HasKey("RoleId", "PermissionId")
                        .HasName("pk_role_permissions");

                    b.HasIndex("PermissionId")
                        .HasDatabaseName("ix_role_permissions_permission_id");

                    b.ToTable("role_permissions", (string)null);

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("5dc6ec47-5b7c-4c2b-86cd-3a671834e56e"),
                            PermissionId = new Guid("33261a4a-c423-4876-8f15-e40068aea5ca")
                        });
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_on_utc");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("first_name");

                    b.Property<string>("IdentityId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("identity_id");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("last_name");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_on_utc");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.HasIndex("IdentityId")
                        .IsUnique()
                        .HasDatabaseName("ix_users_identity_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Myrtus.Clarity.Core.Infrastructure.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("content");

                    b.Property<string>("Error")
                        .HasColumnType("text")
                        .HasColumnName("error");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("occurred_on_utc");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_on_utc");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_outbox_messages");

                    b.ToTable("outbox_messages", (string)null);
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<Guid>("RolesId")
                        .HasColumnType("uuid")
                        .HasColumnName("roles_id");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid")
                        .HasColumnName("users_id");

                    b.HasKey("RolesId", "UsersId")
                        .HasName("pk_role_user");

                    b.HasIndex("UsersId")
                        .HasDatabaseName("ix_role_user_users_id");

                    b.ToTable("role_user", (string)null);
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Blogs.Blog", b =>
                {
                    b.HasOne("Myrtus.CMS.Domain.Users.User", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_blogs_users_deleted_by_id");

                    b.HasOne("Myrtus.CMS.Domain.Users.User", "LastUpdatedBy")
                        .WithMany()
                        .HasForeignKey("LastUpdatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_blogs_users_last_updated_by_id");

                    b.HasOne("Myrtus.CMS.Domain.Users.User", "Owner")
                        .WithMany("Blogs")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_blogs_users_owner_id");

                    b.Navigation("DeletedBy");

                    b.Navigation("LastUpdatedBy");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Blogs.Posts.Comment", b =>
                {
                    b.HasOne("Myrtus.CMS.Domain.Blogs.Posts.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comments_post_post_id");

                    b.HasOne("Myrtus.CMS.Domain.Blogs.Posts.Post", null)
                        .WithMany("Comments")
                        .HasForeignKey("PostId1")
                        .HasConstraintName("fk_comments_post_post_id1");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Blogs.Posts.Post", b =>
                {
                    b.HasOne("Myrtus.CMS.Domain.Blogs.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_posts_blogs_blog_id");

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Users.RolePermission", b =>
                {
                    b.HasOne("Myrtus.CMS.Domain.Users.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_permissions_permissions_permission_id");

                    b.HasOne("Myrtus.CMS.Domain.Users.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_permissions_roles_role_id");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("Myrtus.CMS.Domain.Users.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_user_role_roles_id");

                    b.HasOne("Myrtus.CMS.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_user_user_users_id");
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Blogs.Blog", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Blogs.Posts.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Users.User", b =>
                {
                    b.Navigation("Blogs");
                });
#pragma warning restore 612, 618
        }
    }
}
