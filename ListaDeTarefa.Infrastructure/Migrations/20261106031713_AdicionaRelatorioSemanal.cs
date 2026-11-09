using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ListaDeTarefa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaRelatorioSemanal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelatoriosSemanais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InicioSemana = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FimSemana = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TotalCriadas = table.Column<int>(type: "integer", nullable: false),
                    TotalConcluidas = table.Column<int>(type: "integer", nullable: false),
                    TotalPendentes = table.Column<int>(type: "integer", nullable: false),
                    TaxaConclusao = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    TempoMedioConclusaoHoras = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatoriosSemanais", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelatoriosSemanais_InicioSemana",
                table: "RelatoriosSemanais",
                column: "InicioSemana",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatoriosSemanais");
        }
    }
}
