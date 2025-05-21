using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPCTS.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoResponsaveis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sexo",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaeId",
                table: "Criancas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResponsavelId",
                table: "Criancas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Criancas_MaeId",
                table: "Criancas",
                column: "MaeId");

            migrationBuilder.CreateIndex(
                name: "IX_Criancas_ResponsavelId",
                table: "Criancas",
                column: "ResponsavelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Criancas_Usuarios_MaeId",
                table: "Criancas",
                column: "MaeId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Criancas_Usuarios_ResponsavelId",
                table: "Criancas",
                column: "ResponsavelId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criancas_Usuarios_MaeId",
                table: "Criancas");

            migrationBuilder.DropForeignKey(
                name: "FK_Criancas_Usuarios_ResponsavelId",
                table: "Criancas");

            migrationBuilder.DropIndex(
                name: "IX_Criancas_MaeId",
                table: "Criancas");

            migrationBuilder.DropIndex(
                name: "IX_Criancas_ResponsavelId",
                table: "Criancas");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "MaeId",
                table: "Criancas");

            migrationBuilder.DropColumn(
                name: "ResponsavelId",
                table: "Criancas");
        }
    }
}
