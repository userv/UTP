﻿// <auto-generated />
using ECommerce.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Demo.API.Migrations
{
    [DbContext(typeof(ECommerceDbContext))]
    [Migration("20231216113145_AddProducts_Fix")]
    partial class AddProducts_Fix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ECommerce.Demo.API.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ECommerce.Demo.API.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ECommerce.Demo.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ECommerce.Demo.API.Models.WeatherForecast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Summary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TemperatureC")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("WeatherForecasts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedOn = new DateTime(2023, 12, 16, 13, 31, 44, 873, DateTimeKind.Local).AddTicks(7418),
                            Date = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Summary = "Freezing",
                            TemperatureC = 1
                        },
                        new
                        {
                            Id = 2,
                            CreatedOn = new DateTime(2023, 12, 16, 13, 31, 44, 873, DateTimeKind.Local).AddTicks(7475),
                            Date = new DateTime(2021, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Summary = "Bracing",
                            TemperatureC = 14
                        },
                        new
                        {
                            Id = 3,
                            CreatedOn = new DateTime(2023, 12, 16, 13, 31, 44, 873, DateTimeKind.Local).AddTicks(7479),
                            Date = new DateTime(2021, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Summary = "Freezing",
                            TemperatureC = -13
                        },
                        new
                        {
                            Id = 4,
                            CreatedOn = new DateTime(2023, 12, 16, 13, 31, 44, 873, DateTimeKind.Local).AddTicks(7482),
                            Date = new DateTime(2021, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Summary = "Balmy",
                            TemperatureC = -16
                        },
                        new
                        {
                            Id = 5,
                            CreatedOn = new DateTime(2023, 12, 16, 13, 31, 44, 873, DateTimeKind.Local).AddTicks(7486),
                            Date = new DateTime(2021, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Summary = "Chilly",
                            TemperatureC = -2
                        });
                });

            modelBuilder.Entity("ECommerce.Demo.API.Models.Product", b =>
                {
                    b.HasOne("ECommerce.Demo.API.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ECommerce.Demo.API.Models.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
