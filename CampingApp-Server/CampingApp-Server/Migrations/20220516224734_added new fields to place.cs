using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampingApp_Server.Migrations
{
    public partial class addednewfieldstoplace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AnimalsAllowed",
                table: "Places",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GrillExist",
                table: "Places",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MedicExist",
                table: "Places",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReceptionExist",
                table: "Places",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RestaurantExist",
                table: "Places",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SwimmingpoolExist",
                table: "Places",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WifiExist",
                table: "Places",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimalsAllowed",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "GrillExist",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "MedicExist",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "ReceptionExist",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "RestaurantExist",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "SwimmingpoolExist",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "WifiExist",
                table: "Places");
        }
    }
}
