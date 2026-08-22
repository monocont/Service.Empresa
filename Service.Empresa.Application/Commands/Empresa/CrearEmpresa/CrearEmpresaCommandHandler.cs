using MediatR;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Commands.Empresa.CrearEmpresa;

public class CrearEmpresaCommandHandler : IRequestHandler<CrearEmpresaCommand, CrearEmpresaDTO>
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IUsuarioEmpresaRepository _usuarioEmpresaRepository;

    public CrearEmpresaCommandHandler(IEmpresaRepository empresaRepository, IUsuarioEmpresaRepository usuarioEmpresaRepository)
    {
        _empresaRepository = empresaRepository;
        _usuarioEmpresaRepository = usuarioEmpresaRepository;
    }

    public async Task<CrearEmpresaDTO> Handle(CrearEmpresaCommand request, CancellationToken cancellationToken)
    {
        var existe = await _empresaRepository.ObtenerPorRucYUsuarioAsync(request.Ruc, request.CreadoPor);
        if (existe is not null)
        {
            throw new ArgumentException("El RUC ya se encuentra registrado en el sistema.");
        }

        var empresa = Domain.Entities.Empresa.Crear(
            request.Ruc,
            request.RazonSocial.ToUpper(),
            request.NombreComercial?.ToUpper() ?? "",
            request.CodigoRegimenTributario.ToUpper(),
            request.CodigoEstadoContribuyente.ToUpper(),
            request.CodigoCondicionContribuyente.ToUpper(),
            request.DireccionFiscal.ToUpper(),
            request.Ubigeo,
            request.MonedaBase.ToUpper(),
            request.LogoUrl ?? "",
            request.CreadoPor);

        await _empresaRepository.AgregarAsync(empresa);

        // El creador queda registrado como dueño de la empresa (en memoria; el commit es único)
        var propiedad = Domain.Entities.UsuarioEmpresa.Crear(request.IdUsuario, empresa.IdEmpresa, "DUENO", request.CreadoPor);
        await _usuarioEmpresaRepository.AgregarAsync(propiedad);

        await _empresaRepository.CommitAsync();

        return new CrearEmpresaDTO
        {
            IdEmpresa = empresa.IdEmpresa.ToString()
        };
    }
}