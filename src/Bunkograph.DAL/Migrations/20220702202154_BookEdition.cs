using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkograph.DAL.Migrations
{
    public partial class BookEdition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEdition_Books_BookId",
                table: "BookEdition");

            migrationBuilder.DropForeignKey(
                name: "FK_BookEdition_Publisher_PublisherId",
                table: "BookEdition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookEdition",
                table: "BookEdition");

            migrationBuilder.RenameTable(
                name: "BookEdition",
                newName: "BookEditions");

            migrationBuilder.RenameIndex(
                name: "IX_BookEdition_PublisherId",
                table: "BookEditions",
                newName: "IX_BookEditions_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_BookEdition_BookId",
                table: "BookEditions",
                newName: "IX_BookEditions_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookEditions",
                table: "BookEditions",
                column: "BookEditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_Books_BookId",
                table: "BookEditions",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_Publisher_PublisherId",
                table: "BookEditions",
                column: "PublisherId",
                principalTable: "Publisher",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Books_BookId",
                table: "BookEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Publisher_PublisherId",
                table: "BookEditions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookEditions",
                table: "BookEditions");

            migrationBuilder.RenameTable(
                name: "BookEditions",
                newName: "BookEdition");

            migrationBuilder.RenameIndex(
                name: "IX_BookEditions_PublisherId",
                table: "BookEdition",
                newName: "IX_BookEdition_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_BookEditions_BookId",
                table: "BookEdition",
                newName: "IX_BookEdition_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookEdition",
                table: "BookEdition",
                column: "BookEditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEdition_Books_BookId",
                table: "BookEdition",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookEdition_Publisher_PublisherId",
                table: "BookEdition",
                column: "PublisherId",
                principalTable: "Publisher",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
