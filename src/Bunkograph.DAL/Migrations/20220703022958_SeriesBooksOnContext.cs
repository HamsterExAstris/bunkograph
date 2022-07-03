using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkograph.DAL.Migrations
{
    public partial class SeriesBooksOnContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesBook_Books_BookId",
                table: "SeriesBook");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesBook_Series_SeriesId",
                table: "SeriesBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeriesBook",
                table: "SeriesBook");

            migrationBuilder.RenameTable(
                name: "SeriesBook",
                newName: "SeriesBooks");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesBook_BookId",
                table: "SeriesBooks",
                newName: "IX_SeriesBooks_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeriesBooks",
                table: "SeriesBooks",
                columns: new[] { "SeriesId", "BookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesBooks_Books_BookId",
                table: "SeriesBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesBooks_Series_SeriesId",
                table: "SeriesBooks",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "SeriesId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesBooks_Books_BookId",
                table: "SeriesBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesBooks_Series_SeriesId",
                table: "SeriesBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeriesBooks",
                table: "SeriesBooks");

            migrationBuilder.RenameTable(
                name: "SeriesBooks",
                newName: "SeriesBook");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesBooks_BookId",
                table: "SeriesBook",
                newName: "IX_SeriesBook_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeriesBook",
                table: "SeriesBook",
                columns: new[] { "SeriesId", "BookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesBook_Books_BookId",
                table: "SeriesBook",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesBook_Series_SeriesId",
                table: "SeriesBook",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "SeriesId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
