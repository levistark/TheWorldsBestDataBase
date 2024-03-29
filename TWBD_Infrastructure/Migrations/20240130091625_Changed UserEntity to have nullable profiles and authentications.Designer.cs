﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TWBD_Infrastructure.Contexts;

#nullable disable

namespace TWBD_Infrastructure.Migrations
{
    [DbContext(typeof(UserDataContext))]
    [Migration("20240130091625_Changed UserEntity to have nullable profiles and authentications")]
    partial class ChangedUserEntitytohavenullableprofilesandauthentications
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserAddressEntity", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserAuthenticationEntity", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Authentications");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserEntity", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastLogin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("RegistrationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserPhoneNumberEntity", b =>
                {
                    b.Property<int>("PhoneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhoneId"));

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PhoneId");

                    b.ToTable("PhoneNumbers");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserProfileEntity", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PhoneNumberPhoneId")
                        .HasColumnType("int");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("AddressId");

                    b.HasIndex("PhoneNumberPhoneId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserRoleEntity", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoleId");

                    b.HasIndex("RoleType")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserAuthenticationEntity", b =>
                {
                    b.HasOne("TWBD_Infrastructure.Entities.UserEntity", "User")
                        .WithOne("UserAuthentication")
                        .HasForeignKey("TWBD_Infrastructure.Entities.UserAuthenticationEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserEntity", b =>
                {
                    b.HasOne("TWBD_Infrastructure.Entities.UserRoleEntity", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserProfileEntity", b =>
                {
                    b.HasOne("TWBD_Infrastructure.Entities.UserAddressEntity", "Address")
                        .WithMany("UserProfiles")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TWBD_Infrastructure.Entities.UserPhoneNumberEntity", "PhoneNumber")
                        .WithMany("UserProfiles")
                        .HasForeignKey("PhoneNumberPhoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TWBD_Infrastructure.Entities.UserEntity", "User")
                        .WithOne("UserProfile")
                        .HasForeignKey("TWBD_Infrastructure.Entities.UserProfileEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("PhoneNumber");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserAddressEntity", b =>
                {
                    b.Navigation("UserProfiles");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserEntity", b =>
                {
                    b.Navigation("UserAuthentication");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserPhoneNumberEntity", b =>
                {
                    b.Navigation("UserProfiles");
                });

            modelBuilder.Entity("TWBD_Infrastructure.Entities.UserRoleEntity", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
