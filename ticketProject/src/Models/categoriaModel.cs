using System;

namespace ticketProject.src.Models
{
    internal class Categoria
    {
        public required int id_categoria { get; set; }
        public required string nome { get; set; }
        public DateTime data_hora_criacao { get; set; }
        public DateTime? data_hora_delecao { get; set; } 
    }
}