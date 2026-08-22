using MediatR;
using Service.Empresa.Application.DTOs.CredencialSunat;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Commands.CredencialSunat.Actualizar;

public class ActualizarCredencialSunatCommandHandler : IRequestHandler<ActualizarCredencialSunatCommand, CredencialSunatDTO>
{
    private readonly ICredencialSunatRepository _credencialRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IUsuarioEmpresaRepository _usuarioEmpresaRepository;

    public ActualizarCredencialSunatCommandHandler(ICredencialSunatRepository credencialRepository, IEmpresaRepository empresaRepository, IUsuarioEmpresaRepository usuarioEmpresaRepository)
    {
        _credencialRepository = credencialRepository;
        _empresaRepository = empresaRepository;
        _usuarioEmpresaRepository = usuarioEmpresaRepository;
    }

    public async Task<CredencialSunatDTO> Handle(ActualizarCredencialSunatCommand request, CancellationToken cancellationToken)
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


        credencial.Actualizar(
            request.UsuarioSol.ToUpper(),
            request.ClaveSol,
            request.CreadoPor);

        await _credencialRepository.CommitAsync();

        return new CredencialSunatDTO
        {
            IdCredencial = credencial.IdCredencial.ToString(),
            IdEmpresa = credencial.IdEmpresa.ToString(),
            UsuarioSol = credencial.UsuarioSol,
            ClaveSol = credencial.ClaveSol
        };
    }
}