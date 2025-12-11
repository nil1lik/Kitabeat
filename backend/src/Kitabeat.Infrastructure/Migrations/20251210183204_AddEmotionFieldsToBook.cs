using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kitabeat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmotionFieldsToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmotionLabel",
                table: "Books",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "EmotionScore",
                table: "Books",
                type: "double precision",
                precision: 10,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmotionLabel",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "EmotionScore",
                table: "Books");
        }
    }
}
