using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkograph.DAL.Migrations
{
    public partial class PromoteLanguageToTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Language",
                table: "BookEditions",
                newName: "LanguageId");

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table => table.PrimaryKey("PK_Languages", x => x.LanguageId))
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData("Languages", "LanguageId", new object[] { "en", "jp", "ab" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Languages_LanguageId",
                table: "BookEditions");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_BookEditions_LanguageId",
                table: "BookEditions");

            migrationBuilder.RenameColumn(
                name: "LanguageId",
                table: "BookEditions",
                newName: "Language");
        }
    }
}
