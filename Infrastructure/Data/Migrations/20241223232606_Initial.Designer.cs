﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ParkingManagement.Infrastructure.Data;

#nullable disable

namespace ParkingManagement.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241223232606_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ParkingManagement.Domain.ParkingFee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParkingSpotId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalFee")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("ParkingSpotId")
                        .IsUnique();

                    b.ToTable("ParkingFees", (string)null);
                });

            modelBuilder.Entity("ParkingManagement.Domain.ParkingSpot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EntryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExitTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("RegionId")
                        .HasColumnType("uuid");

                    b.Property<int>("Size")
                        .HasColumnType("integer");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.HasIndex("VehicleId");

                    b.ToTable("ParkingSpots", (string)null);
                });

            modelBuilder.Entity("ParkingManagement.Domain.Region", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("HourlyRate")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("TotalCapacity")
                        .HasColumnType("integer");

                    b.Property<int>("VehicleSize")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Regions", (string)null);
                });

            modelBuilder.Entity("ParkingManagement.Domain.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int>("VehicleSize")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Vehicles", (string)null);
                });

            modelBuilder.Entity("ParkingManagement.Domain.ParkingFee", b =>
                {
                    b.HasOne("ParkingManagement.Domain.ParkingSpot", "ParkingSpot")
                        .WithOne()
                        .HasForeignKey("ParkingManagement.Domain.ParkingFee", "ParkingSpotId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ParkingSpot");
                });

            modelBuilder.Entity("ParkingManagement.Domain.ParkingSpot", b =>
                {
                    b.HasOne("ParkingManagement.Domain.Region", "Region")
                        .WithMany("ParkingSpots")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ParkingManagement.Domain.Vehicle", "Vehicle")
                        .WithMany("ParkingSpots")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Region");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("ParkingManagement.Domain.Region", b =>
                {
                    b.Navigation("ParkingSpots");
                });

            modelBuilder.Entity("ParkingManagement.Domain.Vehicle", b =>
                {
                    b.Navigation("ParkingSpots");
                });
#pragma warning restore 612, 618
        }
    }
}