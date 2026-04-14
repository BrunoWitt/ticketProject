namespace ticketProject.src.Modules.Usuario.DTOs
{
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
        public required Models.PerfilUsuario PerfilUsuario { get; set; }
    }
}