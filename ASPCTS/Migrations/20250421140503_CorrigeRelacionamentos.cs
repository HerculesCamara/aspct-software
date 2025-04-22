using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPCTS.Migrations
{
    /// <inheritdoc />
    public partial class CorrigeRelacionamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criancas_Usuarios_PaiId1",
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

            migrationBuilder.AlterColumn<int>(
                name: "PsicologoId",
                table: "Criancas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<int>(
                name: "PsicologoId",
                table: "Criancas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "FK_Criancas_Usuarios_PaiId1",
                table: "Criancas",
                column: "PaiId1",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Criancas_Usuarios_PsicologoId1",
                table: "Criancas",
                column: "PsicologoId1",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
