using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Application.Interfaces;

public interface IEmpresaRepository
{
    Task<Domain.Entities.Empresa?> ObtenerPorRucAsync(string ruc);
    Task<Domain.Entities.Empresa?> ObtenerPorRucYUsuarioAsync(string ruc, string creadoPor);
    Task<Domain.Entities.Empresa?> ObtenerPorIdAsync(Guid id);
    Task<List<Domain.Entities.Empresa>> ObtenerTodosAsync();
    Task<List<Domain.Entities.Empresa>> ObtenerTodosPorUsuarioAsync(string creadoPor);
    Task<(List<Domain.Entities.Empresa> items, int total)> ObtenerConFiltrosYPaginacionAsync(
        string? ruc, string? razonSocial, string? codigoRegimenTributario,
        Guid idUsuario, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<bool> RucDisponibleAsync(string ruc, string creadoPor, Guid? excludeId);
    Task AgregarAsync(Domain.Entities.Empresa empresa);
    Task CommitAsync();
}