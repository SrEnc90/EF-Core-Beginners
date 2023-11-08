using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sol_EFCorePeliculas.Migrations
{
    /// <inheritdoc />
    public partial class Faltabarelaciónentreactorypelícula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PeliculasActores_ActorId",
                table: "PeliculasActores");

            migrationBuilder.UpdateData(
                table: "CinesOfertas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 11, 8, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CinesOfertas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2023, 11, 13, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 11, 8, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasActores_ActorId",
                table: "PeliculasActores",
                column: "ActorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PeliculasActores_ActorId",
                table: "PeliculasActores");

            migrationBuilder.UpdateData(
                table: "CinesOfertas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2023, 10, 29, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 10, 22, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CinesOfertas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaFin", "FechaInicio" },
                values: new object[] { new DateTime(2023, 10, 27, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 10, 22, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasActores_ActorId",
                table: "PeliculasActores",
                column: "ActorId",
                unique: true);
        }
    }
}
