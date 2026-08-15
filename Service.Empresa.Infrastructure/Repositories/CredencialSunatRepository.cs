using Microsoft.EntityFrameworkCore;
using Service.Empresa.Application.Interfaces;
using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Infrastructure.Repositories;

public class CredencialSunatRepository : ICredencialSunatRepository
{
    private readonly Database.EmpresaDbContext _context;

    public CredencialSunatRepository(Database.EmpresaDbContext context) => _context = context;

    public async Task<CredencialSunat?> ObtenerPorIdAsync(Guid id)
        => await _context.CredencialSunat.FindAsync(id);

    public async Task<CredencialSunat?> ObtenerPorEmpresaAsync(Guid idEmpresa)
        => await _context.CredencialSunat.FirstOrDefaultAsync(c => c.IdEmpresa == idEmpresa);

    public async Task AgregarAsync(CredencialSunat credencial)
        => await _context.CredencialSunat.AddAsync(credencial);

    public async Task CommitAsync()
        => await _context.SaveChangesAsync();
}