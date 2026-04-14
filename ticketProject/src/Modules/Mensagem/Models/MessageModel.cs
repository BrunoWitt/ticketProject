using System;

namespace ticketProject.src.Models
{
    public class Message
    {
        public int id_mensagem {get; set;}
        public required string texto {get; set;}
        public required int id_usuario {get; set;}
        public required int id_ticket {get; set;}

        public DateTime data_hora_criacao {get; set;}
        public DateTime? data_hora_delecao {get; set;}
    }
}