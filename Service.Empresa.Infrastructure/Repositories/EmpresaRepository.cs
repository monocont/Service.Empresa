using Microsoft.EntityFrameworkCore;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Infrastructure.Repositories;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly Database.EmpresaDbContext _context;

    public EmpresaRepository(Database.EmpresaDbContext context) => _context = context;

    public async Task<Domain.Entities.Empresa?> ObtenerPorRucAsync(string ruc)
        => await _context.Empresa.FirstOrDefaultAsync(e => e.Ruc == ruc);

    public async Task<Domain.Entities.Empresa?> ObtenerPorRucYUsuarioAsync(string ruc, string creadoPor)
        => await _context.Empresa.FirstOrDefaultAsync(e => e.Ruc == ruc && e.CreadoPor == creadoPor);

    public async Task<Domain.Entities.Empresa?> ObtenerPorIdAsync(Guid id)
        => await _context.Empresa.FindAsync(id);

    public async Task<List<Domain.Entities.Empresa>> ObtenerTodosAsync()
        => await _context.Empresa.ToListAsync();

    public async Task<List<Domain.Entities.Empresa>> ObtenerTodosPorUsuarioAsync(string creadoPor)
        => await _context.Empresa.Where(e => e.CreadoPor == creadoPor).ToListAsync();

    public async Task<(List<Domain.Entities.Empresa> items, int total)> ObtenerConFiltrosYPaginacionAsync(
        string? ruc, string? razonSocial, string? codigoRegimenTributario,
        string creadoPor, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Empresa
            .Where(e => e.CreadoPor == creadoPor);

        if (!string.IsNullOrWhiteSpace(ruc))
        {
            query = query.Where(e => e.Ruc.Contains(ruc));
        }

        if (!string.IsNullOrWhiteSpace(razonSocial))
        {
            query = query.Where(e => e.RazonSocial.Contains(razonSocial));
        }

        if (!string.IsNullOrWhiteSpace(codigoRegimenTributario))
        {
            query = query.Where(e => e.CodigoRegimenTributario == codigoRegimenTributario);
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(e => e.RazonSocial)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<bool> RucDisponibleAsync(string ruc, string creadoPor, Guid? excludeId = null)
    {
        var query = _context.Empresa
            .Where(e => e.CreadoPor == creadoPor && e.Ruc == ruc);

        if (excludeId.HasValue)
        {
            query = query.Where(e => e.IdEmpresa != excludeId.Value);
        }

        return !await query.AnyAsync();
    }

    public async Task AgregarAsync(Domain.Entities.Empresa empresa)
    {
        await _context.Empresa.AddAsync(empresa);
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}