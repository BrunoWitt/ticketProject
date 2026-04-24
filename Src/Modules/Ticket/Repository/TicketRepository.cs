using Src.Modules.Ticket.Models;
using Src.Shared.DataBase;
using Npgsql;
using Microsoft.Extensions.Primitives;
using Src.Shared.Base;

namespace Src.Modules.Ticket.Repository
{
    
    public class TicketRepository : BaseRepository<TicketModel>, ITicketRepository
    {
        public TicketRepository(IConfiguration config) : base(config)
        {
        }

        protected override string TableName => "ticket";
        protected override string IdColumn => "id";

        public async Task UpdateStatus(int id, StatusTicket newStatus)
        {
            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = @"UPDATE ticket SET status = @status WHERE id_ticket = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("status", newStatus.ToString());

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task Assign(int idTicket, int idAtendente)
        {
            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = @"
                UPDATE ticket
                SET id_usuario_responsavel = @idAtendente,
                    status = 'em_andamento'
                WHERE id_ticket = @idTicket;
            ";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("idTicket", idTicket);
            cmd.Parameters.AddWithValue("idAtendente", idAtendente);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
