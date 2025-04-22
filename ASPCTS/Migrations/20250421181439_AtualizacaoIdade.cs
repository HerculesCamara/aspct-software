using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPCTS.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoIdade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Idade",
                table: "Criancas");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataNascimento",
                table: "Usuarios",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataNascimento",
                table: "Criancas",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Criancas");

            migrationBuilder.AddColumn<int>(
                name: "Idade",
                table: "Criancas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
