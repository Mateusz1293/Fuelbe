﻿// <auto-generated />
using System;
using FuelBe.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FuelBe.Migrations
{
    [DbContext(typeof(ReservationDbContext))]
    [Migration("20231024163442_add-role")]
    partial class addrole
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FuelBe.Database.Models.Refueling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("AddDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("add_date");

                    b.Property<int>("CounterStatus")
                        .HasColumnType("int")
                        .HasColumnName("counter_status");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("price");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("quantity");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int")
                        .HasColumnName("vehicle_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleId");

                    b.ToTable("refueling", "dbo");
                });

            modelBuilder.Entity("FuelBe.Database.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateFrom")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_from");

                    b.Property<DateTime>("DateTo")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_to");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int")
                        .HasColumnName("vehicle_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleId");

                    b.ToTable("reservation", "dbo");
                });

            modelBuilder.Entity("FuelBe.Database.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("role", "dbo");
                });

            modelBuilder.Entity("FuelBe.Database.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("last_name");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.HasKey("Id");

                    b.ToTable("user", "dbo");
                });

            modelBuilder.Entity("FuelBe.Database.Models.UserRole", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("roleId");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("userRole", "dbo");
                });

            modelBuilder.Entity("FuelBe.Database.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FuelType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fuel_type");

                    b.Property<string>("Gearbox")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("gearbox");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("model");

                    b.Property<int>("ProductionYear")
                        .HasColumnType("int")
                        .HasColumnName("production_year");

                    b.Property<string>("RegisterNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("register_number");

                    b.Property<string>("VehicleType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("vehicle_type");

                    b.Property<string>("Vin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("vin");

                    b.HasKey("Id");

                    b.ToTable("vehicle", "dbo");
                });

            modelBuilder.Entity("FuelBe.Database.Models.Refueling", b =>
                {
                    b.HasOne("FuelBe.Database.Models.User", "User")
                        .WithMany("Refuelings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FuelBe.Database.Models.Vehicle", "Vehicle")
                        .WithMany("Refuelings")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("FuelBe.Database.Models.Reservation", b =>
                {
                    b.HasOne("FuelBe.Database.Models.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FuelBe.Database.Models.Vehicle", "Vehicle")
                        .WithMany("Reservations")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("FuelBe.Database.Models.UserRole", b =>
                {
                    b.HasOne("FuelBe.Database.Models.Role", "Role")
                        .WithMany("UsersRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FuelBe.Database.Models.User", "User")
                        .WithMany("UsersRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FuelBe.Database.Models.Role", b =>
                {
                    b.Navigation("UsersRoles");
                });

            modelBuilder.Entity("FuelBe.Database.Models.User", b =>
                {
                    b.Navigation("Refuelings");

                    b.Navigation("Reservations");

                    b.Navigation("UsersRoles");
                });

            modelBuilder.Entity("FuelBe.Database.Models.Vehicle", b =>
                {
                    b.Navigation("Refuelings");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
