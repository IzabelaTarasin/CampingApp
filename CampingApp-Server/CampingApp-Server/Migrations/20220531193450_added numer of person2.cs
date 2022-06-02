using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampingApp_Server.Migrations
{
    public partial class addednumerofperson2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfPeople",
                table: "Reservations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfPeople",
                table: "Reservations");
        }
    }
}
