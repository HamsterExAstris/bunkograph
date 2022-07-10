using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkograph.DAL.Migrations
{
    public partial class ParentPublisher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentPublisherPublisherId",
                table: "Publishers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_ParentPublisherPublisherId",
                table: "Publishers",
                column: "ParentPublisherPublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publishers_Publishers_ParentPublisherPublisherId",
                table: "Publishers",
                column: "ParentPublisherPublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publishers_Publishers_ParentPublisherPublisherId",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Publishers_ParentPublisherPublisherId",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "ParentPublisherPublisherId",
                table: "Publishers");
        }
    }
}
