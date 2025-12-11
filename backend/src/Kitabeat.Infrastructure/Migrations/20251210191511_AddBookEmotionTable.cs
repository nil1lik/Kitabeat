using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kitabeat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookEmotionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmotionLabel",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "EmotionScore",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "BookEmotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    Label = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Score = table.Column<double>(type: "double precision", precision: 5, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookEmotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookEmotions_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookEmotions_BookId_Label",
                table: "BookEmotions",
                columns: new[] { "BookId", "Label" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookEmotions");

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
    }
}
