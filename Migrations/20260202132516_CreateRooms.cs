using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Rooms",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Rooms",
                newName: "isActive");
        }
    }
}
