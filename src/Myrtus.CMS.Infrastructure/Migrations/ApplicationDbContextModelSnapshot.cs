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

            modelBuilder.Entity("Myrtus.CMS.Domain.Roles.Permission", b =>
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

                    b.Property<string>("Feature")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("feature");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_on_utc");

                    b.HasKey("Id")
                        .HasName("pk_permissions");

                    b.ToTable("permissions", (string)null);
                });

            modelBuilder.Entity("Myrtus.CMS.Domain.Roles.Role", b =>
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

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.Property<Guid>("PermissionsId")
                        .HasColumnType("uuid")
                        .HasColumnName("permissions_id");

                    b.Property<Guid>("RolesId")
                        .HasColumnType("uuid")
                        .HasColumnName("roles_id");

                    b.HasKey("PermissionsId", "RolesId")
                        .HasName("pk_permission_role");

                    b.HasIndex("RolesId")
                        .HasDatabaseName("ix_permission_role_roles_id");

                    b.ToTable("permission_role", (string)null);
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

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.HasOne("Myrtus.CMS.Domain.Roles.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_permission_role_permission_permissions_id");

                    b.HasOne("Myrtus.CMS.Domain.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_permission_role_role_roles_id");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("Myrtus.CMS.Domain.Roles.Role", null)
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
