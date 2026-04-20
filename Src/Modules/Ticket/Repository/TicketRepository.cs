using Src.Modules.Ticket.Models;
using Src.Shared.DataBase;
using Npgsql;
using Microsoft.Extensions.Primitives;

namespace Src.Modules.Ticket.Repository
{
    
    public class TicketRepository : ITicketRepository
    {
        public async Task Create(string Titulo, string Descricao, PrioridadeTicket Prioridade, int IdUsuario, int? IdCategoria)
        {
            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = @"
                INSERT INTO ticket 
                (titulo, descricao, status, prioridade, data_hora_criado, id_usuario, id_categoria)
                VALUES (@titulo, @descricao, 'aberto', @prioridade::prioridade_ticket, NOW(), @idUsuario, @idCategoria);
            ";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("titulo", Titulo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("descricao", Descricao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("prioridade", Prioridade.ToString());
            cmd.Parameters.AddWithValue("idUsuario", IdUsuario);
            cmd.Parameters.AddWithValue("idCategoria", (object?)IdCategoria ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<List<TicketModel>> GetAll()
        {
            var list = new List<TicketModel>();

            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = "SELECT * FROM ticket WHERE data_hora_delecao IS NULL";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(Map(reader));
            }

            return list;
        }


        public async Task<TicketModel?> GetById(int id)
        {
            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = "SELECT * FROM ticket WHERE id_ticket = @id LIMIT 1";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                return Map(reader);

            return null;
        }


        public async Task Update(int Id, string? Titulo, string? Descricao, PrioridadeTicket? Prioridade)
        {   
            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = @"
                UPDATE ticket SET
                    titulo = COALESCE(@titulo, titulo),
                    descricao = COALESCE(@descricao, descricao),
                    prioridade = COALESCE(@prioridade::prioridade_ticket, prioridade)
                WHERE id_ticket = @id;
            ";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", Id);
            cmd.Parameters.AddWithValue("titulo", (object?)Titulo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("descricao", (object?)Descricao ?? DBNull.Value);
            cmd.Parameters.AddWithValue("prioridade", (object?)Prioridade.ToString() ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task Delete(int id)
        {
            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = @"
                UPDATE ticket
                SET data_hora_delecao = NOW()
                WHERE id_ticket = @id;
            ";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);

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


        private TicketModel Map(NpgsqlDataReader reader)
        {
            return new TicketModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("id_ticket")),
                Titulo = reader.GetString(reader.GetOrdinal("titulo")),
                Descricao = reader.GetString(reader.GetOrdinal("descricao")),
                Status = Enum.Parse<StatusTicket>(reader.GetString(reader.GetOrdinal("status"))),
                Prioridade = Enum.Parse<PrioridadeTicket>(reader.GetString(reader.GetOrdinal("prioridade"))),
                DataHoraCriado = reader.GetDateTime(reader.GetOrdinal("data_hora_criado")),
                DataHoraFinalizado = reader.IsDBNull(reader.GetOrdinal("data_hora_finalizado"))
                    ? default
                    : reader.GetDateTime(reader.GetOrdinal("data_hora_finalizado")),
                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                IdAtendente = reader.IsDBNull(reader.GetOrdinal("id_usuario_responsavel"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("id_usuario_responsavel")),
                IdCategoria = reader.IsDBNull(reader.GetOrdinal("id_categoria"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("id_categoria"))
            };
        }
    }
}
