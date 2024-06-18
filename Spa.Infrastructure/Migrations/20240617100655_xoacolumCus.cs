using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class xoacolumCus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployeeID",
                table: "Appointments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
