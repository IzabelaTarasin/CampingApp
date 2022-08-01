using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampingApp_Server.Migrations
{
    public partial class areas2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Reservations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AreaId",
                table: "Reservations",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Area_AreaId",
                table: "Reservations",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Area_AreaId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_AreaId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Reservations");
        }
    }
}
