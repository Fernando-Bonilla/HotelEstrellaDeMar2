using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelEstrellaDeMar.Migrations
{
    public partial class initialDos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumHabitacion",
                table: "Reservas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumHabitacion",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
