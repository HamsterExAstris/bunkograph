using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkograph.DAL.Migrations
{
    public partial class DropBookEditionPublisherId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Publishers_PublisherId",
                table: "BookEditions");

            migrationBuilder.DropIndex(
                name: "IX_BookEditions_PublisherId",
                table: "BookEditions");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "BookEditions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "BookEditions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookEditions_PublisherId",
                table: "BookEditions",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_Publishers_PublisherId",
                table: "BookEditions",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
