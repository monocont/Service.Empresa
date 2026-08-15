using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Application.Interfaces;

public interface ICredencialSunatRepository
{
    Task<CredencialSunat?> ObtenerPorIdAsync(Guid id);
    Task<CredencialSunat?> ObtenerPorEmpresaAsync(Guid idEmpresa);
    Task AgregarAsync(CredencialSunat credencial);
    Task CommitAsync();
}