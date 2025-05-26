using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPCTS.Migrations
{
    /// <inheritdoc />
    public partial class AplicarParentesco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PsicologoId",
                table: "Atividades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Atividades_PsicologoId",
                table: "Atividades",
                column: "PsicologoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atividades_Usuarios_PsicologoId",
                table: "Atividades",
                column: "PsicologoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atividades_Usuarios_PsicologoId",
                table: "Atividades");

            migrationBuilder.DropIndex(
                name: "IX_Atividades_PsicologoId",
                table: "Atividades");

            migrationBuilder.DropColumn(
                name: "PsicologoId",
                table: "Atividades");
        }
    }
}
