using ListaDeTarefa.Domain.Entities;

namespace ListaDeTarefa.Application.Interfaces.IRepository
{
    public interface IRelatorioSemanalRepository : IGenericRepository<RelatorioSemanal>
    {
        Task<RelatorioSemanal?> BuscarPorInicioSemana(DateTimeOffset inicioSemana);
        Task<RelatorioSemanal?> BuscarUltimo();
    }
}
