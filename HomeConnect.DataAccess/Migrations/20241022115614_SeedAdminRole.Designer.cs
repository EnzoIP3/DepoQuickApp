﻿// <auto-generated />
using System;
using HomeConnect.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HomeConnect.DataAccess.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20241022115614_SeedAdminRole")]
    partial class SeedAdminRole
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BusinessLogic.Auth.Entities.Token", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("BusinessLogic.BusinessOwners.Entities.Business", b =>
                {
                    b.Property<string>("Rut")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Rut");

                    b.HasIndex("OwnerId");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("BusinessLogic.Devices.Entities.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BusinessRut")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("MainPhoto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ModelNumber")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondaryPhotos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BusinessRut");

                    b.ToTable("Devices");

                    b.HasDiscriminator().HasValue("Device");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("BusinessLogic.Devices.Entities.OwnedDevice", b =>
                {
                    b.Property<Guid>("HardwareId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Connected")
                        .HasColumnType("bit");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HardwareId");

                    b.HasIndex("DeviceId");

                    b.HasIndex("HomeId");

                    b.ToTable("OwnedDevices");
                });

            modelBuilder.Entity("BusinessLogic.HomeOwners.Entities.Home", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<int>("MaxMembers")
                        .HasColumnType("int");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Homes");
                });

            modelBuilder.Entity("BusinessLogic.HomeOwners.Entities.HomePermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HomePermissions");
                });

            modelBuilder.Entity("BusinessLogic.HomeOwners.Entities.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HomeId");

                    b.HasIndex("UserId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("BusinessLogic.Notifications.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnedDeviceHardwareId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Read")
                        .HasColumnType("bit");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OwnedDeviceHardwareId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BusinessLogic.Roles.Entities.Role", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Name = "Admin"
                        },
                        new
                        {
                            Name = "HomeOwner"
                        },
                        new
                        {
                            Name = "BusinessOwner"
                        });
                });

            modelBuilder.Entity("BusinessLogic.Roles.Entities.SystemPermission", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Value");

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Value = "create-administrator"
                        },
                        new
                        {
                            Value = "delete-administrator"
                        },
                        new
                        {
                            Value = "create-business-owner"
                        },
                        new
                        {
                            Value = "get-all-users"
                        },
                        new
                        {
                            Value = "get-all-businesses"
                        },
                        new
                        {
                            Value = "create-home"
                        },
                        new
                        {
                            Value = "add-member"
                        },
                        new
                        {
                            Value = "add-device"
                        },
                        new
                        {
                            Value = "get-devices"
                        },
                        new
                        {
                            Value = "get-members"
                        },
                        new
                        {
                            Value = "get-notifications"
                        },
                        new
                        {
                            Value = "create-business"
                        },
                        new
                        {
                            Value = "create-camera"
                        },
                        new
                        {
                            Value = "create-sensor"
                        },
                        new
                        {
                            Value = "update-member"
                        });
                });

            modelBuilder.Entity("BusinessLogic.Users.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f1b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b"),
                            CreatedAt = new DateOnly(2024, 1, 1),
                            Email = "admin@admin.com",
                            Name = "Administrator",
                            Password = "Admin123@",
                            Surname = "Account"
                        });
                });

            modelBuilder.Entity("HomePermissionMember", b =>
                {
                    b.Property<Guid>("HomePermissionsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HomePermissionsId", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("MemberHomePermissions", (string)null);
                });

            modelBuilder.Entity("RoleSystemPermission", b =>
                {
                    b.Property<string>("PermissionsValue")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RolesName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PermissionsValue", "RolesName");

                    b.HasIndex("RolesName");

                    b.ToTable("RoleSystemPermission");

                    b.HasData(
                        new
                        {
                            PermissionsValue = "create-administrator",
                            RolesName = "Admin"
                        },
                        new
                        {
                            PermissionsValue = "delete-administrator",
                            RolesName = "Admin"
                        },
                        new
                        {
                            PermissionsValue = "create-business-owner",
                            RolesName = "Admin"
                        },
                        new
                        {
                            PermissionsValue = "get-all-users",
                            RolesName = "Admin"
                        },
                        new
                        {
                            PermissionsValue = "get-all-businesses",
                            RolesName = "Admin"
                        },
                        new
                        {
                            PermissionsValue = "create-home",
                            RolesName = "HomeOwner"
                        },
                        new
                        {
                            PermissionsValue = "add-member",
                            RolesName = "HomeOwner"
                        },
                        new
                        {
                            PermissionsValue = "add-device",
                            RolesName = "HomeOwner"
                        },
                        new
                        {
                            PermissionsValue = "get-devices",
                            RolesName = "HomeOwner"
                        },
                        new
                        {
                            PermissionsValue = "get-members",
                            RolesName = "HomeOwner"
                        },
                        new
                        {
                            PermissionsValue = "update-member",
                            RolesName = "HomeOwner"
                        },
                        new
                        {
                            PermissionsValue = "get-notifications",
                            RolesName = "HomeOwner"
                        },
                        new
                        {
                            PermissionsValue = "create-business",
                            RolesName = "BusinessOwner"
                        },
                        new
                        {
                            PermissionsValue = "create-camera",
                            RolesName = "BusinessOwner"
                        },
                        new
                        {
                            PermissionsValue = "create-sensor",
                            RolesName = "BusinessOwner"
                        });
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<string>("RolesName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RolesName", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserRole", (string)null);

                    b.HasData(
                        new
                        {
                            RolesName = "Admin",
                            UsersId = new Guid("f1b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b")
                        });
                });

            modelBuilder.Entity("BusinessLogic.Devices.Entities.Camera", b =>
                {
                    b.HasBaseType("BusinessLogic.Devices.Entities.Device");

                    b.Property<bool>("MotionDetection")
                        .HasColumnType("bit");

                    b.Property<bool>("PersonDetection")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("Camera");
                });

            modelBuilder.Entity("BusinessLogic.Auth.Entities.Token", b =>
                {
                    b.HasOne("BusinessLogic.Users.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessLogic.BusinessOwners.Entities.Business", b =>
                {
                    b.HasOne("BusinessLogic.Users.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("BusinessLogic.Devices.Entities.Device", b =>
                {
                    b.HasOne("BusinessLogic.BusinessOwners.Entities.Business", "Business")
                        .WithMany()
                        .HasForeignKey("BusinessRut");

                    b.Navigation("Business");
                });

            modelBuilder.Entity("BusinessLogic.Devices.Entities.OwnedDevice", b =>
                {
                    b.HasOne("BusinessLogic.Devices.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessLogic.HomeOwners.Entities.Home", "Home")
                        .WithMany()
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Home");
                });

            modelBuilder.Entity("BusinessLogic.HomeOwners.Entities.Home", b =>
                {
                    b.HasOne("BusinessLogic.Users.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("BusinessLogic.HomeOwners.Entities.Member", b =>
                {
                    b.HasOne("BusinessLogic.HomeOwners.Entities.Home", "Home")
                        .WithMany("Members")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessLogic.Users.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Home");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessLogic.Notifications.Entities.Notification", b =>
                {
                    b.HasOne("BusinessLogic.Devices.Entities.OwnedDevice", "OwnedDevice")
                        .WithMany()
                        .HasForeignKey("OwnedDeviceHardwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessLogic.Users.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("OwnedDevice");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HomePermissionMember", b =>
                {
                    b.HasOne("BusinessLogic.HomeOwners.Entities.HomePermission", null)
                        .WithMany()
                        .HasForeignKey("HomePermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessLogic.HomeOwners.Entities.Member", null)
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleSystemPermission", b =>
                {
                    b.HasOne("BusinessLogic.Roles.Entities.SystemPermission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsValue")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessLogic.Roles.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("BusinessLogic.Roles.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessLogic.Users.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BusinessLogic.HomeOwners.Entities.Home", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
