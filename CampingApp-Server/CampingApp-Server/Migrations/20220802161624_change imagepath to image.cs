using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampingApp_Server.Migrations
{
    public partial class changeimagepathtoimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Places",
                newName: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Places",
                newName: "ImagePath");
        }
    }
}
