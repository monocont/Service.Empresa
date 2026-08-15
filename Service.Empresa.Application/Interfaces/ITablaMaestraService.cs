using Service.Empresa.Application.DTOs.Empresa;

namespace Service.Empresa.Application.Interfaces;

public interface ITablaMaestraService
{
    Task<List<RegimenTributarioDTO>> ListarRegimenTributarioAsync(CancellationToken cancellationToken = default);
    Task<List<EstadoContribuyenteDTO>> ListarEstadoContribuyenteAsync(CancellationToken cancellationToken = default);
    Task<List<CondicionContribuyenteDTO>> ListarCondicionContribuyenteAsync(CancellationToken cancellationToken = default);
}