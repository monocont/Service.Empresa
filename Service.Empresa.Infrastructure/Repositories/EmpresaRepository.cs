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
        Guid idUsuario, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var empresasDelUsuario = _context.UsuarioEmpresa
            .Where(ue => ue.IdUsuario == idUsuario)
            .Select(ue => ue.IdEmpresa);

        var query = _context.Empresa
            .Where(e => empresasDelUsuario.Contains(e.IdEmpresa));

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

    public async Task<List<Application.DTOs.Empresa.EmpresaConLimitesDTO>> ObtenerEmpresasConLimitesPorUsuarioAsync(
        Guid idUsuario, int anio, bool esAdmin, CancellationToken cancellationToken = default)
    {
        IQueryable<Domain.Entities.Empresa> empresasQuery;

        if (esAdmin)
        {
            empresasQuery = _context.Empresa.Where(e => e.Activo);
        }
        else
        {
            var empresasDelUsuario = _context.UsuarioEmpresa
                .Where(ue => ue.IdUsuario == idUsuario && ue.Activo)
                .Select(ue => ue.IdEmpresa);

            empresasQuery = _context.Empresa
                .Where(e => e.Activo && empresasDelUsuario.Contains(e.IdEmpresa));
        }

        var empresas = await empresasQuery
            .OrderBy(e => e.RazonSocial)
            .ToListAsync(cancellationToken);

        var codigosRegimen = empresas.Select(e => e.CodigoRegimenTributario).Distinct().ToList();

        var regimenes = await _context.RegimenTributario
            .Where(r => codigosRegimen.Contains(r.Codigo) && r.Activo)
            .ToDictionaryAsync(r => r.Codigo, r => r.Descripcion, cancellationToken);

        var limites = await _context.RegimenTributarioLimite
            .Where(l => codigosRegimen.Contains(l.CodigoRegimenTributario) && l.Anio == anio && l.Activo)
            .ToDictionaryAsync(l => l.CodigoRegimenTributario, cancellationToken);

        var resultado = new List<Application.DTOs.Empresa.EmpresaConLimitesDTO>();

        foreach (var emp in empresas)
        {
            var regDesc = regimenes.GetValueOrDefault(emp.CodigoRegimenTributario) ?? emp.CodigoRegimenTributario;
            limites.TryGetValue(emp.CodigoRegimenTributario, out var lim);

            var item = new Application.DTOs.Empresa.EmpresaConLimitesDTO
            {
                IdEmpresa = emp.IdEmpresa,
                Ruc = emp.Ruc,
                RazonSocial = emp.RazonSocial,
                NombreComercial = emp.NombreComercial,
                CodigoRegimenTributario = emp.CodigoRegimenTributario,
                RegimenDescripcion = regDesc,
                Anio = anio,
                ValorUit = lim?.ValorUit ?? 0,
                LimiteMensualVentas = lim?.LimiteMensualVentas,
                LimiteMensualCompras = lim?.LimiteMensualCompras,
                LimiteAnualVentas = lim?.LimiteAnualVentas,
                LimiteAnualCompras = lim?.LimiteAnualCompras,
                LimiteAnualVentasUit = lim?.LimiteAnualVentasUit,
                VentasSinLimite = emp.CodigoRegimenTributario.Equals("RG", StringComparison.OrdinalIgnoreCase) || (lim != null && lim.LimiteAnualVentas == null && lim.LimiteAnualVentasUit == null),
                ComprasSinLimite = emp.CodigoRegimenTributario.Equals("RG", StringComparison.OrdinalIgnoreCase) || emp.CodigoRegimenTributario.Equals("RMT", StringComparison.OrdinalIgnoreCase) || (lim != null && lim.LimiteAnualCompras == null)
            };

            resultado.Add(item);
        }

        return resultado;
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