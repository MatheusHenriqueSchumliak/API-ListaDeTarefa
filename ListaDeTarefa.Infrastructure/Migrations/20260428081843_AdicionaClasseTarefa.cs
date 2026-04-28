using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ListaDeTarefa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaClasseTarefa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "char(1)", nullable: false),
                    DataConclusao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DataCriacao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DataAtualizacao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarefas");
        }
    }
}
