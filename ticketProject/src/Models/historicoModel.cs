using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ticketProject.src.Models
{
    internal class Historico
    {
        public required int id_historico {get; set;}
        public required int id_ticket {get; set;}
        public DateTime data_alteracao {get; set;}
        public string status_anterior {get; set;} = string.Empty;
        public string status_novo {get; set;} = string.Empty;
    }
}