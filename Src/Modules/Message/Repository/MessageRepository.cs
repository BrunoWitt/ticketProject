using Src.Modules.Message.Models;
using Src.Shared.DataBase;
using Npgsql;

namespace Src.Modules.Message.Repository
{
    public class MessageRepository : IMessageRepository
    {
        public async Task Create(MessageModel message)
        {
            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = @"
                INSERT INTO mensagem (texto, id_usuario, id_ticket, data_hora_criado)
                VALUES (@texto, @usuario, @ticket, NOW())
                RETURNING id_mensagem;
            ";

            using var cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("texto", message.Texto);
            cmd.Parameters.AddWithValue("usuario", message.IdUsuario);
            cmd.Parameters.AddWithValue("ticket", message.IdTicket);

            var result = await cmd.ExecuteScalarAsync();

            if (result == null)
                throw new Exception("Erro ao inserir mensagem");

            message.Id = Convert.ToInt32(result);
        }


        public async Task<List<MessageModel>> GetByTicket(int ticketId)
        {
            var list = new List<MessageModel>();

            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = @"
                SELECT * FROM mensagem
                WHERE id_ticket = @ticket
                AND data_hora_delecao IS NULL
                ORDER BY data_hora_criado ASC;
            ";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("ticket", ticketId);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(Map(reader));
            }

            return list;
        }


        public async Task Delete(long id)
        {
            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = @"
                UPDATE mensagem
                SET data_hora_delecao = NOW()
                WHERE id_mensagem = @id;
            ";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);

            await cmd.ExecuteNonQueryAsync();
        }


        private MessageModel Map(NpgsqlDataReader reader)
        {
            return new MessageModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("id_mensagem")),
                Texto = reader.GetString(reader.GetOrdinal("texto")),
                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                IdTicket = reader.GetInt32(reader.GetOrdinal("id_ticket")),
                DataHoraCriado = reader.GetDateTime(reader.GetOrdinal("data_hora_criado"))
            };
        }
    }
}