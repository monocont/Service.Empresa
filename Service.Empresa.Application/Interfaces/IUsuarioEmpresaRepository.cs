using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Application.Interfaces;

public interface IUsuarioEmpresaRepository
{
    Task<UsuarioEmpresa?> ObtenerPorUsuarioYEmpresaAsync(Guid idUsuario, Guid idEmpresa);
    Task<List<Guid>> ObtenerIdsEmpresasPorUsuarioAsync(Guid idUsuario);
    Task AgregarAsync(UsuarioEmpresa usuarioEmpresa);
    Task CommitAsync();
}
