using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Empresa.Application.Commands.Empresa.ActualizarEmpresa;
using Service.Empresa.Application.Commands.Empresa.CrearEmpresa;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;
using Service.Empresa.Application.Queries.Empresa.ListarEmpresas;
using Service.Empresa.Application.Queries.Empresa.ObtenerEmpresaPorId;
using Service.Empresa.Application.Queries.Empresa.ObtenerEmpresaPorRuc;
using Service.Empresa.Application.Queries.Empresa.ObtenerEmpresasConLimites;
using Service.Empresa.Application.Queries.Empresa.ValidarAccesoEmpresa;

namespace Service.Empresa.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/empresa")]
public class EmpresaController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITablaMaestraService _tablaMaestraService;

    public EmpresaController(IMediator mediator, ITablaMaestraService tablaMaestraService)
    {
        _mediator = mediator;
        _tablaMaestraService = tablaMaestraService;
    }


    private Guid ObtenerIdUsuario() =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;

    private bool EsAdmin() => User.IsInRole("ADMIN");

    [HttpPost]
    public async Task<IActionResult> CrearEmpresa([FromBody] CrearEmpresaRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Sistema";
        var command = new CrearEmpresaCommand
        {
            Ruc = request.Ruc,
            RazonSocial = request.RazonSocial,
            NombreComercial = request.NombreComercial,
            CodigoRegimenTributario = request.CodigoRegimenTributario,
            CodigoEstadoContribuyente = request.CodigoEstadoContribuyente,
            CodigoCondicionContribuyente = request.CodigoCondicionContribuyente,
            DireccionFiscal = request.DireccionFiscal,
            Ubigeo = request.Ubigeo,
            MonedaBase = request.MonedaBase,
            LogoUrl = request.LogoUrl,
            CreadoPor = userId,
            IdUsuario = ObtenerIdUsuario()
        };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> ListarEmpresas(
        [FromQuery] string? ruc,
        [FromQuery] string? razonSocial,
        [FromQuery] string? codigoRegimenTributario,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Sistema";
        var query = new ListarEmpresasQuery
        {
            IdUsuario = ObtenerIdUsuario(),
            Ruc = ruc,
            RazonSocial = razonSocial,
            CodigoRegimenTributario = codigoRegimenTributario,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> ActualizarEmpresa(Guid id, [FromBody] ActualizarEmpresaRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Sistema";
        var command = new ActualizarEmpresaCommand
        {
            Id = id,
            Ruc = request.Ruc,
            RazonSocial = request.RazonSocial,
            NombreComercial = request.NombreComercial,
            CodigoRegimenTributario = request.CodigoRegimenTributario,
            CodigoEstadoContribuyente = request.CodigoEstadoContribuyente,
            CodigoCondicionContribuyente = request.CodigoCondicionContribuyente,
            DireccionFiscal = request.DireccionFiscal,
            Ubigeo = request.Ubigeo,
            MonedaBase = request.MonedaBase,
            LogoUrl = request.LogoUrl,
            ModificadoPor = userId,
            IdUsuario = ObtenerIdUsuario(),
            EsAdmin = EsAdmin()
        };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerEmpresaPorId(Guid id)
    {
        var result = await _mediator.Send(new ObtenerEmpresaPorIdQuery { Id = id, IdUsuario = ObtenerIdUsuario(), EsAdmin = EsAdmin() });
        return Ok(result);
    }

    [HttpGet("ruc/{ruc}")]
    public async Task<IActionResult> ObtenerEmpresaPorRuc(string ruc)
    {
        var result = await _mediator.Send(new ObtenerEmpresaPorRucQuery { Ruc = ruc, IdUsuario = ObtenerIdUsuario(), EsAdmin = EsAdmin() });
        return Ok(result);
    }


    /// <summary>
    /// Determina si el usuario del JWT tiene acceso a la empresa del RUC indicado.
    /// Consumido por otros microservicios (delegación de autorización multi-tenant).
    /// </summary>
    [HttpGet("{ruc}/acceso")]
    public async Task<IActionResult> ValidarAcceso(string ruc)
    {
        var query = new ValidarAccesoEmpresaQuery
        {
            Ruc = ruc,
            IdUsuario = ObtenerIdUsuario(),
            EsAdmin = EsAdmin()
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Lista las empresas a las que tiene acceso el usuario autenticado junto con sus límites de régimen para el año consultado.
    /// Consumido por Service.Operaciones para el control de límites tributarios.
    /// </summary>
    [HttpGet("usuario-empresas-limites")]
    public async Task<IActionResult> ListarEmpresasConLimites([FromQuery] int anio)
    {
        var query = new ObtenerEmpresasConLimitesQuery
        {
            Anio = anio > 0 ? anio : DateTime.UtcNow.Year,
            IdUsuario = ObtenerIdUsuario(),
            EsAdmin = EsAdmin()
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("regimen-tributario")]
    public async Task<IActionResult> ListarRegimenTributario()
    {
        var result = await _tablaMaestraService.ListarRegimenTributarioAsync();
        return Ok(result);
    }

    [HttpGet("estado-contribuyente")]
    public async Task<IActionResult> ListarEstadoContribuyente()
    {
        var result = await _tablaMaestraService.ListarEstadoContribuyenteAsync();
        return Ok(result);
    }

    [HttpGet("condicion-contribuyente")]
    public async Task<IActionResult> ListarCondicionContribuyente()
    {
        var result = await _tablaMaestraService.ListarCondicionContribuyenteAsync();
        return Ok(result);
    }
}