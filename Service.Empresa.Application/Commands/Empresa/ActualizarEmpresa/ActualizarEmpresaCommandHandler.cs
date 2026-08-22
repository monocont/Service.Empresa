using MediatR;
using Service.Empresa.Application.Common.Exceptions;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Commands.Empresa.ActualizarEmpresa;

public class ActualizarEmpresaCommandHandler : IRequestHandler<ActualizarEmpresaCommand, CrearEmpresaDTO>
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IUsuarioEmpresaRepository _usuarioEmpresaRepository;

    public ActualizarEmpresaCommandHandler(IEmpresaRepository empresaRepository, IUsuarioEmpresaRepository usuarioEmpresaRepository)
    {
        _empresaRepository = empresaRepository;
        _usuarioEmpresaRepository = usuarioEmpresaRepository;
    }

    public async Task<CrearEmpresaDTO> Handle(ActualizarEmpresaCommand request, CancellationToken cancellationToken)
    {
        var empresa = await _empresaRepository.ObtenerPorIdAsync(request.Id);

        if (empresa is null)
            throw new NotFoundException(nameof(Domain.Entities.Empresa), request.Id);

        if (!request.EsAdmin)
        {
            var acceso = await _usuarioEmpresaRepository.ObtenerPorUsuarioYEmpresaAsync(request.IdUsuario, empresa.IdEmpresa);
            if (acceso is null)
                throw new UnauthorizedAccessException("No tiene acceso a esta empresa.");
        }

        var existeOtroRuc = await _empresaRepository.RucDisponibleAsync(request.Ruc, request.ModificadoPor, request.Id);
        if (!existeOtroRuc)
        {
            throw new ArgumentException("El RUC ya se encuentra registrado en el sistema.");
        }

        empresa.Actualizar(
            request.RazonSocial.ToUpper(),
            request.NombreComercial?.ToUpper() ?? "",
            request.CodigoRegimenTributario.ToUpper(),
            request.CodigoEstadoContribuyente.ToUpper(),
            request.CodigoCondicionContribuyente.ToUpper(),
            request.DireccionFiscal.ToUpper(),
            request.Ubigeo,
            request.MonedaBase.ToUpper(),
            request.LogoUrl ?? "",
            request.ModificadoPor);

        await _empresaRepository.CommitAsync();

        return new CrearEmpresaDTO
        {
            IdEmpresa = empresa.IdEmpresa.ToString()
        };
    }
}