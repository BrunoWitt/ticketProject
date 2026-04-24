using Src.Shared.Base;

namespace Src.Modules.User.Models;

public enum PerfilUsuario
{
    admin,
    atendente,
    usuario,
}

public class UsuarioModel : BaseModel
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public PerfilUsuario Perfil { get; set; }
}