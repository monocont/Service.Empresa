using Microsoft.EntityFrameworkCore;
using Service.Empresa.Application.Interfaces;
using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Infrastructure.Repositories;

public class UsuarioEmpresaRepository : IUsuarioEmpresaRepository
{
    private readonly Database.EmpresaDbContext _context;

    public UsuarioEmpresaRepository(Database.EmpresaDbContext context) => _context = context;

    public async Task<UsuarioEmpresa?> ObtenerPorUsuarioYEmpresaAsync(Guid idUsuario, Guid idEmpresa)
        => await _context.UsuarioEmpresa
            .FirstOrDefaultAsync(ue => ue.IdUsuario == idUsuario && ue.IdEmpresa == idEmpresa);

    public async Task<List<Guid>> ObtenerIdsEmpresasPorUsuarioAsync(Guid idUsuario)
        => await _context.UsuarioEmpresa
            .Where(ue => ue.IdUsuario == idUsuario)
            .Select(ue => ue.IdEmpresa)
            .ToListAsync();

    public async Task AgregarAsync(UsuarioEmpresa usuarioEmpresa)
    {
        await _context.UsuarioEmpresa.AddAsync(usuarioEmpresa);
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
