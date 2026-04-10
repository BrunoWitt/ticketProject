using Npgsql;
using ticketProject.src.Database;
using ticketProject.src.Models;

namespace ticketProject.src.Repositories
{
    internal class MessageRepository
    {
        //Crudzão
        public void SendMessage(Message mensagem)
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

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public List<Message> ReadAllMessagesFromTicket(Ticket ticket)
        {
            var messageList = new List<Message>();

            try
            {
                using var conn = DataBaseController.GetConnection();
                conn.Open();

                var query = @" SELECT id_mensagem, texto, id_usuario WHERE id_ticket = @id_ticket;";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("id_ticket", ticket.id_ticket);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var mensagem = new Message
                    {
                        id_mensagem = reader.GetInt32(reader.GetOrdinal("id_mensagem")),
                        texto = reader.GetString(reader.GetOrdinal("texto")),
                        id_usuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                        id_ticket = ticket.id_ticket
                    };

                    messageList.Add(mensagem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return messageList;
        }
    }
}