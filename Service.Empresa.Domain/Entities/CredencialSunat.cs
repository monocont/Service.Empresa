using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Domain.Entities;

public class CredencialSunat : EntidadAuditoria
{
    public Guid IdCredencial { get; private set; }
    public Guid IdEmpresa { get; set; }
    public string UsuarioSol { get; set; } = "";
    public string ClaveSol { get; set; } = "";

    protected CredencialSunat() { }

    public static CredencialSunat Crear(
        Guid idEmpresa,
        string usuarioSol,
        string claveSol,
        string usuarioCreacion)
    {
        return new CredencialSunat
        {
            IdCredencial = Guid.NewGuid(),
            IdEmpresa = idEmpresa,
            UsuarioSol = usuarioSol,
            ClaveSol = claveSol,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            CreadoPor = usuarioCreacion
        };
    }

    public void Actualizar(
        string usuarioSol,
        string claveSol,
        string usuarioModificacion)
    {
        UsuarioSol = usuarioSol;
        ClaveSol = claveSol;
        ModificadoPor = usuarioModificacion;
        FechaModificacion = DateTime.UtcNow;
    }
}