using System;
using System.Security.Cryptography.X509Certificates;

namespace ticketProject.src.Models
{
    public enum StatusTicket
    {
        aberto,
        em_andamento,
        resolvido,
        fechado
    }

    public enum PrioridadeTicket
    {
        baixa,
        media,
        alta   
    }
    public class Ticket
    {
        public int id_ticket {get;set;}
        public required string titulo { get; set; }
        public required string descricao { get; set; }
        public StatusTicket status { get; set; }
        public required PrioridadeTicket prioridade {get; set;}
        public DateTime? data_fechamento {get; set; }
        public required int id_usuario {get; set;}
        public int id_usuario_responsavel {get; set;}
        public required int id_categoria {get; set;}
    }
}