using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Empresa.Application.Commands.CredencialSunat.Actualizar;
using Service.Empresa.Application.Commands.CredencialSunat.Crear;
using Service.Empresa.Application.Commands.CredencialSunat.Eliminar;
using Service.Empresa.Application.Common;
using Service.Empresa.Application.DTOs.CredencialSunat;
using Service.Empresa.Application.Queries.CredencialSunat;

namespace Service.Empresa.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/empresa/{idEmpresa}/credencial-sunat")]
public class CredencialSunatController : ControllerBase
{
    private readonly IMediator _mediator;

    public CredencialSunatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Guid idEmpresa, [FromBody] CrearCredencialSunatRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Sistema";
        var command = new CrearCredencialSunatCommand
        {
            IdEmpresa = idEmpresa,
            UsuarioSol = request.UsuarioSol,
            ClaveSol = request.ClaveSol,
            CreadoPor = userId
        };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Obtener(Guid idEmpresa)
    {
        var result = await _mediator.Send(new ObtenerCredencialSunatQuery { IdEmpresa = idEmpresa });
        if (result is null)
        {
            HttpContext.Items["ResponseMessages"] = new List<string> { "La empresa no tiene una credencial SUNAT registrada." };
            return Ok(result);
        }
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(Guid idEmpresa, [FromBody] ActualizarCredencialSunatRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Sistema";
        var command = new ActualizarCredencialSunatCommand
        {
            IdEmpresa = idEmpresa,
            IdCredencial = request.IdCredencial,
            UsuarioSol = request.UsuarioSol,
            ClaveSol = request.ClaveSol,
            CreadoPor = userId
        };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{idCredencial}")]
    public async Task<IActionResult> Eliminar(Guid idEmpresa, string idCredencial)
    {
        await _mediator.Send(new EliminarCredencialSunatCommand
        {
            IdEmpresa = idEmpresa,
            IdCredencial = idCredencial
        });
        return Ok(new { mensaje = "Credencial SUNAT eliminada correctamente." });
    }
}