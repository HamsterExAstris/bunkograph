using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkograph.DAL.Migrations
{
    public partial class CompletionStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompletionStatus",
                table: "Series",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionStatus",
                table: "Series");
        }
    }
}
