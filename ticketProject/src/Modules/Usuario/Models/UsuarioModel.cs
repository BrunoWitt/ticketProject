using System;

namespace ticketProject.src.Modules.Usuario.Models
{

    public enum PerfilUsuario
    {
        admin,
        atendente,
        usuario
    }
    

    public class Usuario
    {
        public int id_usuario { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public PerfilUsuario perfil_usuario { get; set; }

        public DateTime data_hora_criacao { get; set; }
        public DateTime? data_hora_delecao { get; set; } 
    }
}