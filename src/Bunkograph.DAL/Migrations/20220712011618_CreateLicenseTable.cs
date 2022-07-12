using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkograph.DAL.Migrations
{
    public partial class CreateLicenseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Languages_LanguageId",
                table: "BookEditions");

            migrationBuilder.DropIndex(
                name: "IX_BookEditions_LanguageId",
                table: "BookEditions");

            migrationBuilder.AddColumn<int>(
                name: "SeriesLicenseId",
                table: "BookEditions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SeriesLicenses",
                columns: table => new
                {
                    SeriesLicenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<string>(type: "varchar(2)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublisherId = table.Column<int>(type: "int", nullable: false),
                    CompletionStatus = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesLicenses", x => x.SeriesLicenseId);
                    table.ForeignKey(
                        name: "FK_SeriesLicenses_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesLicenses_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "PublisherId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesLicenses_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BookEditions_SeriesLicenseId",
                table: "BookEditions",
                column: "SeriesLicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesLicenses_LanguageId",
                table: "SeriesLicenses",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesLicenses_PublisherId",
                table: "SeriesLicenses",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesLicenses_SeriesId",
                table: "SeriesLicenses",
                column: "SeriesId");

            migrationBuilder.Sql(@"INSERT INTO `SeriesLicenses` (SeriesId, LanguageId, PublisherId, CompletionStatus)
SELECT DISTINCT sb.SeriesId, be.LanguageId, be.PublisherId, s.CompletionStatus FROM `SeriesBooks` sb
JOIN `Series` s ON s.SeriesId = sb.SeriesId
JOIN `BookEditions` be ON be.BookId = sb.BookId;");

            migrationBuilder.Sql(@"UPDATE `BookEditions` be
JOIN `SeriesBooks` sb ON sb.BookId = be.BookId
JOIN `SeriesLicenses` sl ON sl.SeriesId = sb.SeriesId AND sl.LanguageId = be.LanguageId
SET be.SeriesLicenseId = sl.SeriesLicenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_SeriesLicenses_SeriesLicenseId",
                table: "BookEditions",
                column: "SeriesLicenseId",
                principalTable: "SeriesLicenses",
                principalColumn: "SeriesLicenseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "CompletionStatus",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "BookEditions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_SeriesLicenses_SeriesLicenseId",
                table: "BookEditions");

            migrationBuilder.DropTable(
                name: "SeriesLicenses");

            migrationBuilder.DropIndex(
                name: "IX_BookEditions_SeriesLicenseId",
                table: "BookEditions");

            migrationBuilder.DropColumn(
                name: "SeriesLicenseId",
                table: "BookEditions");

            migrationBuilder.AddColumn<string>(
                name: "CompletionStatus",
                table: "Series",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LanguageId",
                table: "BookEditions",
                type: "varchar(2)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BookEditions_LanguageId",
                table: "BookEditions",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_Languages_LanguageId",
                table: "BookEditions",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
