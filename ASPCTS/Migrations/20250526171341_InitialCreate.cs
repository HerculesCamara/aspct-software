using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPCTS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sexo = table.Column<int>(type: "int", nullable: false),
                    DataNascimento = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CRP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PsicologoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Usuarios_PsicologoId",
                        column: x => x.PsicologoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Criancas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    PaiId = table.Column<int>(type: "int", nullable: true),
                    MaeId = table.Column<int>(type: "int", nullable: true),
                    PsicologoId = table.Column<int>(type: "int", nullable: false),
                    ResponsavelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criancas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Criancas_Usuarios_MaeId",
                        column: x => x.MaeId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Criancas_Usuarios_PaiId",
                        column: x => x.PaiId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Criancas_Usuarios_PsicologoId",
                        column: x => x.PsicologoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Criancas_Usuarios_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Atividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Concluida = table.Column<bool>(type: "bit", nullable: true),
                    CriancaId = table.Column<int>(type: "int", nullable: false),
                    PsicologoId = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atividades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atividades_Criancas_CriancaId",
                        column: x => x.CriancaId,
                        principalTable: "Criancas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atividades_Usuarios_PsicologoId",
                        column: x => x.PsicologoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relatorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CriancaId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Resumo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarcosAlcancados = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecomendacoesCasa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecomendacoesEscola = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relatorios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relatorios_Criancas_CriancaId",
                        column: x => x.CriancaId,
                        principalTable: "Criancas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atividades_CriancaId",
                table: "Atividades",
                column: "CriancaId");

            migrationBuilder.CreateIndex(
                name: "IX_Atividades_PsicologoId",
                table: "Atividades",
                column: "PsicologoId");

            migrationBuilder.CreateIndex(
                name: "IX_Criancas_MaeId",
                table: "Criancas",
                column: "MaeId");

            migrationBuilder.CreateIndex(
                name: "IX_Criancas_PaiId",
                table: "Criancas",
                column: "PaiId");

            migrationBuilder.CreateIndex(
                name: "IX_Criancas_PsicologoId",
                table: "Criancas",
                column: "PsicologoId");

            migrationBuilder.CreateIndex(
                name: "IX_Criancas_ResponsavelId",
                table: "Criancas",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Relatorios_CriancaId",
                table: "Relatorios",
                column: "CriancaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PsicologoId",
                table: "Usuarios",
                column: "PsicologoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atividades");

            migrationBuilder.DropTable(
                name: "Relatorios");

            migrationBuilder.DropTable(
                name: "Criancas");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
