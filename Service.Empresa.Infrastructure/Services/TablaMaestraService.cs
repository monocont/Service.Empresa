using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;
using Service.Empresa.Infrastructure.Database;

namespace Service.Empresa.Infrastructure.Services;

public class TablaMaestraService : ITablaMaestraService
{
    private readonly EmpresaDbContext _context;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(12);

    private static readonly string CacheKeyRegimen = "RegimenTributario";
    private static readonly string CacheKeyEstado = "EstadoContribuyente";
    private static readonly string CacheKeyCondicion = "CondicionContribuyente";

    public TablaMaestraService(EmpresaDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<List<RegimenTributarioDTO>> ListarRegimenTributarioAsync(CancellationToken cancellationToken = default)
    {
        return (await _cache.GetOrCreateAsync(CacheKeyRegimen, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await _context.RegimenTributario
                .AsNoTracking()
                .Select(e => new RegimenTributarioDTO
                {
                    Codigo = e.Codigo,
                    Descripcion = e.Descripcion
                })
                .OrderBy(x => x.Codigo)
                .ToListAsync(cancellationToken);
        }))!;
    }

    public async Task<List<EstadoContribuyenteDTO>> ListarEstadoContribuyenteAsync(CancellationToken cancellationToken = default)
    {
        return (await _cache.GetOrCreateAsync(CacheKeyEstado, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await _context.EstadoContribuyente
                .AsNoTracking()
                .Select(e => new EstadoContribuyenteDTO
                {
                    Codigo = e.Codigo,
                    Descripcion = e.Descripcion
                })
                .OrderBy(x => x.Codigo)
                .ToListAsync(cancellationToken);
        }))!;
    }

    public async Task<List<CondicionContribuyenteDTO>> ListarCondicionContribuyenteAsync(CancellationToken cancellationToken = default)
    {
        return (await _cache.GetOrCreateAsync(CacheKeyCondicion, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await _context.CondicionContribuyente
                .AsNoTracking()
                .Select(e => new CondicionContribuyenteDTO
                {
                    Codigo = e.Codigo,
                    Descripcion = e.Descripcion
                })
                .OrderBy(x => x.Codigo)
                .ToListAsync(cancellationToken);
        }))!;
    }
}