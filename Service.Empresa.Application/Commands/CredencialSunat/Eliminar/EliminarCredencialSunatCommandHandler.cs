using MediatR;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Commands.CredencialSunat.Eliminar;

public class EliminarCredencialSunatCommandHandler : IRequestHandler<EliminarCredencialSunatCommand>
{
    private readonly ICredencialSunatRepository _credencialRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IUsuarioEmpresaRepository _usuarioEmpresaRepository;

    public EliminarCredencialSunatCommandHandler(ICredencialSunatRepository credencialRepository, IEmpresaRepository empresaRepository, IUsuarioEmpresaRepository usuarioEmpresaRepository)
    {
        _credencialRepository = credencialRepository;
        _empresaRepository = empresaRepository;
        _usuarioEmpresaRepository = usuarioEmpresaRepository;
    }

    public async Task Handle(EliminarCredencialSunatCommand request, CancellationToken cancellationToken)
    {
        var credencial = await _credencialRepository.ObtenerPorIdAsync(Guid.Parse(request.IdCredencial));
        if (credencial is null || credencial.IdEmpresa.ToString() != request.IdEmpresa.ToString())
            throw new ArgumentException("La credencial SUNAT no existe o no pertenece a la empresa.");

        if (!request.EsAdmin)
        {
            var empresa = await _empresaRepository.ObtenerPorIdAsync(request.IdEmpresa);
            var acceso = empresa is null ? null : await _usuarioEmpresaRepository.ObtenerPorUsuarioYEmpresaAsync(request.IdUsuario, empresa.IdEmpresa);
            if (acceso is null)
                throw new UnauthorizedAccessException("No tiene acceso a esta empresa.");
        }


        credencial.MarcarComoEliminado();
        await _credencialRepository.CommitAsync();
    }
}