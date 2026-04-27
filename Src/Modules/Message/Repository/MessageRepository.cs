using Src.Modules.Message.Models;
using Src.Shared.DataBase;
using Npgsql;
using Src.Shared.Base;
using Src.Modules.User.Repository;

namespace Src.Modules.Message.Repository
{
    public class MessageRepository : BaseRepository<MessageModel>, IMessageRepository
    {

        public MessageRepository(IConfiguration config) : base(config)
        {
        }

        protected override string TableName => "mensagem";
        protected override string IdColumn => "id_mensagem";

        public async Task<List<MessageModel>> GetByTicket(long ticketId)
        {
            var list = new List<MessageModel>();

            using var conn = Connection;
            await conn.OpenAsync();

            var query = @"
                SELECT * FROM mensagem
                WHERE id_ticket = @ticket
                AND (data_hora_delecao IS NULL OR data_hora_delecao = '-infinity')
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


        public async Task CreateAnexo(long idMensagem, byte[] arquivo, string tipo)
        {
            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = @"
                INSERT INTO anexo (arquivo, tipo, id_mensagem, data_hora_criado)
                VALUES (@arquivo, @tipo, @mensagem, NOW());
            ";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("arquivo", arquivo);
            cmd.Parameters.AddWithValue("tipo", tipo);
            cmd.Parameters.AddWithValue("mensagem", idMensagem);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<AnexoModel?> GetAnexo(long id)
        {
            using var conn = DatabaseConnection.GetConnection();
            await conn.OpenAsync();

            var query = @"
                SELECT id_anexo, arquivo, tipo, id_mensagem, data_hora_criado FROM anexo WHERE id_anexo = @id AND data_hora_delecao IS NULL;";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("id", id);

                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync()){
                    return new AnexoModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_anexo")),
                        Arquivo = (byte[])reader["arquivo"],
                        Tipo = reader.GetString(reader.GetOrdinal("tipo")),
                        IdMensagem = reader.GetInt32(reader.GetOrdinal("id_mensagem")),
                        DataHoraCriado = reader.GetDateTime(reader.GetOrdinal("data_hora_criado"))
                    };
                }

                return null;
        }
    }
}