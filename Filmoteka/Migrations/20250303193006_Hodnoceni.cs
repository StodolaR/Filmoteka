using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filmoteka.Migrations
{
    /// <inheritdoc />
    public partial class Hodnoceni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgRating",
                table: "Movies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvgRating",
                table: "Movies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
