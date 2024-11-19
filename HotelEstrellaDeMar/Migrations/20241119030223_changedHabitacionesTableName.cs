using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelEstrellaDeMar.Migrations
{
    public partial class changedHabitacionesTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Habbitaciones_NumHabitacion",
                table: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Habbitaciones",
                table: "Habbitaciones");

            migrationBuilder.RenameTable(
                name: "Habbitaciones",
                newName: "Habitaciones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Habitaciones",
                table: "Habitaciones",
                column: "NumHabitacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Habitaciones_NumHabitacion",
                table: "Reservas",
                column: "NumHabitacion",
                principalTable: "Habitaciones",
                principalColumn: "NumHabitacion",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Habitaciones_NumHabitacion",
                table: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Habitaciones",
                table: "Habitaciones");

            migrationBuilder.RenameTable(
                name: "Habitaciones",
                newName: "Habbitaciones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Habbitaciones",
                table: "Habbitaciones",
                column: "NumHabitacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Habbitaciones_NumHabitacion",
                table: "Reservas",
                column: "NumHabitacion",
                principalTable: "Habbitaciones",
                principalColumn: "NumHabitacion",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
