using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMediaItemBookAudiobookFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "MediaItems");

            migrationBuilder.DropColumn(
                name: "PageCount",
                table: "MediaItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "MediaItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PageCount",
                table: "MediaItems",
                type: "int",
                nullable: true);
        }
    }
}
