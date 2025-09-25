using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShort.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedLongurlchar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LongURL",
                table: "ShortenUrls",
                type: "VARCHAR(768)",
                maxLength: 768,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(500)",
                oldMaxLength: 2000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LongURL",
                table: "ShortenUrls",
                type: "VARCHAR(500)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(768)",
                oldMaxLength: 768)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
