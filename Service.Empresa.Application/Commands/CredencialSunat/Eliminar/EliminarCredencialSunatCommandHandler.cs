using MediatR;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Commands.CredencialSunat.Eliminar;

public class EliminarCredencialSunatCommandHandler : IRequestHandler<EliminarCredencialSunatCommand>
{
    private readonly ICredencialSunatRepository _credencialRepository;

    public EliminarCredencialSunatCommandHandler(ICredencialSunatRepository credencialRepository)
    {
        _credencialRepository = credencialRepository;
    }

    public async Task Handle(EliminarCredencialSunatCommand request, CancellationToken cancellationToken)
    {
        var credencial = await _credencialRepository.ObtenerPorIdAsync(Guid.Parse(request.IdCredencial));
        if (credencial is null || credencial.IdEmpresa.ToString() != request.IdEmpresa.ToString())
            throw new ArgumentException("La credencial SUNAT no existe o no pertenece a la empresa.");

        credencial.MarcarComoEliminado();
        await _credencialRepository.CommitAsync();
    }
}