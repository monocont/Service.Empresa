namespace Service.Empresa.Application.DTOs.CredencialSunat;

public class ActualizarCredencialSunatRequest
{
    public string IdCredencial { get; set; } = "";
    public string UsuarioSol { get; set; } = "";
    public string ClaveSol { get; set; } = "";
}
