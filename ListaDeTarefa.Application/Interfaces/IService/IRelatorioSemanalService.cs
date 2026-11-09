using ListaDeTarefa.Application.DTOs.RelatorioSemanal;

namespace ListaDeTarefa.Application.Interfaces.IService
{
    public interface IRelatorioSemanalService
    {
        Task<RelatorioSemanalDto> GerarRelatorioSemanal(DateTimeOffset referencia);
        Task<RelatorioSemanalDto?> BuscarUltimo();
    }
}
