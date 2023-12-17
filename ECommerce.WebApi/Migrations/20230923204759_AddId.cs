using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Demo.API.Migrations
{
    /// <inheritdoc />
    public partial class AddId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Freezing", 1 },
                    { 2, new DateTime(2021, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bracing", 14 },
                    { 3, new DateTime(2021, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Freezing", -13 },
                    { 4, new DateTime(2021, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Balmy", -16 },
                    { 5, new DateTime(2021, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chilly", -2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
