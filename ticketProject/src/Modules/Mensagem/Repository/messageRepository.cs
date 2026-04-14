using Npgsql;
using ticketProject.src.Database;
using ticketProject.src.Models;
using ticketProject.src.Modules.Mensagem.Repository;

namespace ticketProject.src.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        //Crudzão
        public async Task SendMessage(Message mensagem)
        {
            ///
            /// Função responsável por colocar a mensagem no banco de dados de um chamado especifico
            /// 
            try
            {
                using var conn = DataBaseController.GetConnection();
                conn.Open();

                var query = @"INSERT INTO mensagem(texto, id_usuario, id_ticket) VALUES (@texto, @id_usuario, @id_ticket)";

                using var cmd = new NpgsqlCommand(query, conn);

                cmd.Parameters.AddWithValue("texto", mensagem.texto);
                cmd.Parameters.AddWithValue("id_usuario", mensagem.id_usuario);
                cmd.Parameters.AddWithValue("id_ticket", mensagem.id_ticket);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public async Task<List<Message>> ReadAllMessagesFromTicket(int ticketId)
        {
            var messageList = new List<Message>();

            using var conn = DataBaseController.GetConnection();
            conn.Open();

            var query = @"SELECT id_mensagem, texto, id_usuario, id_ticket 
                        FROM mensagem 
                        WHERE id_ticket = @id_ticket;";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id_ticket", ticketId);

            using var reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                var mensagem = new Message
                {
                    id_mensagem = reader.GetInt32(0),
                    texto = reader.GetString(1),
                    id_usuario = reader.GetInt32(2),
                    id_ticket = reader.GetInt32(3)
                };

                messageList.Add(mensagem);
            }

            return messageList;
        }
    }
}