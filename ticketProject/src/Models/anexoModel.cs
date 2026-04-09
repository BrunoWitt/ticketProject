using System;
using System.Data.SqlTypes;

namespace ticketProject.src.Models
{
    internal class Anexo
    {
        public required int id_anexo { get; set; }
        public required SqlBytes arquivo { get; set; }
        public required string tipo { get; set; }
        public required string id_mensagem { get; set; }
        public DateTime data_hora_criacao { get; set; }
        public DateTime? data_hora_delecao { get; set; } 
    }
}