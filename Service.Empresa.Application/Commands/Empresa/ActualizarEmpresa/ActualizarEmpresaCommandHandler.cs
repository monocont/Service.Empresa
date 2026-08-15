using MediatR;
using Service.Empresa.Application.Common.Exceptions;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Commands.Empresa.ActualizarEmpresa;

public class ActualizarEmpresaCommandHandler : IRequestHandler<ActualizarEmpresaCommand, CrearEmpresaDTO>
{
    private readonly IEmpresaRepository _empresaRepository;

    public ActualizarEmpresaCommandHandler(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
    }

    public async Task<CrearEmpresaDTO> Handle(ActualizarEmpresaCommand request, CancellationToken cancellationToken)
    {
        var empresa = await _empresaRepository.ObtenerPorIdAsync(request.Id);

        if (empresa is null)
            throw new NotFoundException(nameof(Domain.Entities.Empresa), request.Id);

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