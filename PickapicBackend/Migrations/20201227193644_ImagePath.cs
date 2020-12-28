using Microsoft.EntityFrameworkCore.Migrations;

namespace PickapicBackend.Migrations
{
    public partial class ImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Images",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "Images",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "Images");
        }
    }
}
