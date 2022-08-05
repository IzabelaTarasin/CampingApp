using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampingApp_Server.Migrations
{
    public partial class addednewfacilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BikesExist",
                table: "Places",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LaundryRoomExist",
                table: "Places",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BikesExist",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "LaundryRoomExist",
                table: "Places");
        }
    }
}
