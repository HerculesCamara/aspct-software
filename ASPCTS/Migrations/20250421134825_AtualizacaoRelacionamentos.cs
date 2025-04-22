using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPCTS.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoRelacionamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atividades_Criancas_CriancaId",
                table: "Atividades");

            migrationBuilder.DropForeignKey(
                name: "FK_Criancas_Usuarios_PaiId",
                table: "Criancas");

            migrationBuilder.DropForeignKey(
                name: "FK_Criancas_Usuarios_PsicologoId",
                table: "Criancas");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Usuarios",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaiId1",
                table: "Criancas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PsicologoId1",
                table: "Criancas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Criancas_PaiId1",
                table: "Criancas",
                column: "PaiId1");

            migrationBuilder.CreateIndex(
                name: "IX_Criancas_PsicologoId1",
                table: "Criancas",
                column: "PsicologoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Atividades_Criancas_CriancaId",
                table: "Atividades",
                column: "CriancaId",
                principalTable: "Criancas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Criancas_Usuarios_PaiId",
                table: "Criancas",
                column: "PaiId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Criancas_Usuarios_PaiId1",
                table: "Criancas",
                column: "PaiId1",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Criancas_Usuarios_PsicologoId",
                table: "Criancas",
                column: "PsicologoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Criancas_Usuarios_PsicologoId1",
                table: "Criancas",
                column: "PsicologoId1",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atividades_Criancas_CriancaId",
                table: "Atividades");

            migrationBuilder.DropForeignKey(
                name: "FK_Criancas_Usuarios_PaiId",
                table: "Criancas");

            migrationBuilder.DropForeignKey(
                name: "FK_Criancas_Usuarios_PaiId1",
                table: "Criancas");

            migrationBuilder.DropForeignKey(
                name: "FK_Criancas_Usuarios_PsicologoId",
                table: "Criancas");

            migrationBuilder.DropForeignKey(
                name: "FK_Criancas_Usuarios_PsicologoId1",
                table: "Criancas");

            migrationBuilder.DropIndex(
                name: "IX_Criancas_PaiId1",
                table: "Criancas");

            migrationBuilder.DropIndex(
                name: "IX_Criancas_PsicologoId1",
                table: "Criancas");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PaiId1",
                table: "Criancas");

            migrationBuilder.DropColumn(
                name: "PsicologoId1",
                table: "Criancas");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Usuarios",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Atividades_Criancas_CriancaId",
                table: "Atividades",
                column: "CriancaId",
                principalTable: "Criancas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Criancas_Usuarios_PaiId",
                table: "Criancas",
                column: "PaiId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Criancas_Usuarios_PsicologoId",
                table: "Criancas",
                column: "PsicologoId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
