using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sol_EFCorePeliculas.Migrations
{
    /// <inheritdoc />
    public partial class TipoSalaDeCineEnSalaDeCine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoSalaDeCine",
                table: "SalaDeCines",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoSalaDeCine",
                table: "SalaDeCines");
        }
    }
}
