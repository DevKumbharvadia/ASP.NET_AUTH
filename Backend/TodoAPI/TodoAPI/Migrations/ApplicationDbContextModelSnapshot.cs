﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoAPI.Data;

#nullable disable

namespace TodoAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TodoAPI.Models.Domain.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.Role", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("406761eb-609d-48a6-ae1c-08a493378bb1"),
                            RoleName = "Admin"
                        },
                        new
                        {
                            RoleId = new Guid("4eb396e1-47a1-4442-b54f-0c6b18447114"),
                            RoleName = "User"
                        });
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.TodoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("f2527472-1335-4785-b2e7-a80ad0057ceb"),
                            CreatedAt = new DateTime(2024, 11, 16, 5, 47, 25, 617, DateTimeKind.Utc).AddTicks(7977),
                            Email = "admin@example.com",
                            PasswordHash = "$2a$11$qH7chvIUMS52OTllob7s8e7txOSeJyiR2O.GSorrcfmiRZqoHFkzS",
                            UpdatedAt = new DateTime(2024, 11, 16, 5, 47, 25, 617, DateTimeKind.Utc).AddTicks(7984),
                            Username = "admin"
                        },
                        new
                        {
                            UserId = new Guid("418ba6fd-0483-432a-9ef2-63ee373c2419"),
                            CreatedAt = new DateTime(2024, 11, 16, 5, 47, 25, 723, DateTimeKind.Utc).AddTicks(6957),
                            Email = "user@example.com",
                            PasswordHash = "$2a$11$wbGT/ihaJgR1GAvXffPiX.vA5GnXtiiF8zJIC49ekeiT6Gr95a3rO",
                            UpdatedAt = new DateTime(2024, 11, 16, 5, 47, 25, 723, DateTimeKind.Utc).AddTicks(6961),
                            Username = "user"
                        });
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.UserAudit", b =>
                {
                    b.Property<Guid>("UserAuditId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LogoutTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserAuditId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAudits");
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("f2527472-1335-4785-b2e7-a80ad0057ceb"),
                            RoleId = new Guid("406761eb-609d-48a6-ae1c-08a493378bb1")
                        },
                        new
                        {
                            UserId = new Guid("418ba6fd-0483-432a-9ef2-63ee373c2419"),
                            RoleId = new Guid("4eb396e1-47a1-4442-b54f-0c6b18447114")
                        });
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.RefreshToken", b =>
                {
                    b.HasOne("TodoAPI.Models.Domain.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.TodoItem", b =>
                {
                    b.HasOne("TodoAPI.Models.Domain.User", "User")
                        .WithMany("TodoItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.UserAudit", b =>
                {
                    b.HasOne("TodoAPI.Models.Domain.User", "User")
                        .WithMany("UserAudits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.UserRole", b =>
                {
                    b.HasOne("TodoAPI.Models.Domain.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoAPI.Models.Domain.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("TodoAPI.Models.Domain.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("TodoItems");

                    b.Navigation("UserAudits");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
