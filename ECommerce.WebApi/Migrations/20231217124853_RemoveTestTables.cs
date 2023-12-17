using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Demo.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTestTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherForecasts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemperatureC = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "CreatedOn", "Date", "ModifiedOn", "Summary", "TemperatureC" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 12, 16, 13, 31, 44, 873, DateTimeKind.Local).AddTicks(7418), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Freezing", 1 },
                    { 2, new DateTime(2023, 12, 16, 13, 31, 44, 873, DateTimeKind.Local).AddTicks(7475), new DateTime(2021, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bracing", 14 },
                    { 3, new DateTime(2023, 12, 16, 13, 31, 44, 873, DateTimeKind.Local).AddTicks(7479), new DateTime(2021, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Freezing", -13 },
                    { 4, new DateTime(2023, 12, 16, 13, 31, 44, 873, DateTimeKind.Local).AddTicks(7482), new DateTime(2021, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Balmy", -16 },
                    { 5, new DateTime(2023, 12, 16, 13, 31, 44, 873, DateTimeKind.Local).AddTicks(7486), new DateTime(2021, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Chilly", -2 }
                });
        }
    }
}
