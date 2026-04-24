using System.Numerics;
using Src.Modules.User.Models;

namespace Src.Modules.User.DTOs;

public class LoginDTO
{
    public required string Email { get; set; }
    public required string Senha { get; set; }
}

public class CreateUserDTO
{
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Senha { get; set; }
    public PerfilUsuario PerfilUsuario { get; set; }
}

public class UpdateUserDTO
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Senha { get; set; }
    public PerfilUsuario PerfilUsuario { get; set; }
}

public class CreationUserDTO
{
    public required string Nome {get;set;}
    public required string Email {get;set;}
    public required string Senha {get;set;}
    public required PerfilUsuario PerfilUsuario {get;set;}
}
