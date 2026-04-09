using System;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;

using ticketProject.src.Database;
using ticketProject.src.Models;
using ticketProject.src.Services.Filter;

namespace ticketProject.src.Repositories
{
    internal class TicketRepository
    {
        public void CreateTicket(Ticket ticket)
        {
            /// <summary>
            /// Usuário cria o ticket no banco de dados
            /// </summary>
            try
            {
                using var conn = DataBaseController.GetConnection();
                conn.Open();

                var query = @"
                    INSERT INTO ticket 
                    (titulo, descricao, status, prioridade, id_usuario, id_usuario_responsavel, id_categoria)
                    VALUES 
                    (@titulo, @descricao, @status, @prioridade, @id_usuario, @id_usuario_responsavel, @id_categoria);
                ";

                using var cmd = new NpgsqlCommand(query, conn);

                cmd.Parameters.AddWithValue("titulo", ticket.titulo);
                cmd.Parameters.AddWithValue("descricao", ticket.descricao);
                cmd.Parameters.AddWithValue("status", ticket.status.ToString());
                cmd.Parameters.AddWithValue("prioridade", ticket.prioridade.ToString());
                cmd.Parameters.AddWithValue("id_usuario", ticket.id_usuario);
                cmd.Parameters.AddWithValue("id_usuario_responsavel", ticket.id_usuario_responsavel);
                cmd.Parameters.AddWithValue("id_categoria", ticket.id_categoria);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public List<Ticket> ReadAllTickets()
        {
            var ticketList = new List<Ticket>();

            try
            {
                using var conn = DataBaseController.GetConnection();
                conn.Open();

                var query = @" SELECT id_ticket, titulo, descricao, status, prioridade, data_fechamento, id_usuario, id_usuario_responsavel, id_categoria FROM ticket;";

                using var cmd = new NpgsqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var ticket = new Ticket
                    {
                        id_ticket = reader.GetInt32(reader.GetOrdinal("id_ticket")),
                        titulo = reader.GetString(reader.GetOrdinal("titulo")),
                        descricao = reader.GetString(reader.GetOrdinal("descricao")),
                        status = Enum.Parse<StatusTicket>(reader.GetString(reader.GetOrdinal("status"))),
                        prioridade = Enum.Parse<PrioridadeTicket>(reader.GetString(reader.GetOrdinal("prioridade"))),
                        data_fechamento = reader.IsDBNull(reader.GetOrdinal("data_fechamento"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("data_fechamento")),
                        id_usuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                        id_usuario_responsavel = reader.GetInt32(reader.GetOrdinal("id_usuario_responsavel")),
                        id_categoria = reader.GetInt32(reader.GetOrdinal("id_categoria"))
                    };

                    ticketList.Add(ticket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ticketList;
        }


        public List<Ticket> GetTickets(TicketFilter filter)
        {
            var tickets = new List<Ticket>();

            try
            {
                using var conn = DataBaseController.GetConnection();
                conn.Open();

                var query = @"
                    SELECT id_ticket, titulo, descricao, status, prioridade, 
                        data_fechamento, id_usuario, id_usuario_responsavel, id_categoria
                    FROM ticket
                    WHERE 1=1
                ";

                using var cmd = new NpgsqlCommand();
                cmd.Connection = conn;

            if (filter.Status != null)
            {
                query += " AND status = @status";
                cmd.Parameters.AddWithValue("status", filter.Status.ToString());
            }

            if (filter.Prioridade != null)
            {
                query += " AND prioridade = @prioridade";
                cmd.Parameters.AddWithValue("prioridade", filter.Prioridade.ToString());
            }

            if (filter.IdUsuario != null)
            {
                query += " AND id_usuario = @id_usuario";
                cmd.Parameters.AddWithValue("id_usuario", filter.IdUsuario);
            }

            if (filter.IdResponsavel != null)
            {
                query += " AND id_usuario_responsavel = @id_responsavel";
                cmd.Parameters.AddWithValue("id_responsavel", filter.IdResponsavel);
            }

            if (filter.IdCategoria != null)
            {
                query += " AND id_categoria = @id_categoria";
                cmd.Parameters.AddWithValue("id_categoria", filter.IdCategoria);
            }

            if (filter.atribuido != string.Empty)
            {
                query += " AND id_usuario_responsavel IS NULL";
            }

            cmd.CommandText = query;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var ticket = new Ticket
                {
                    id_ticket = reader.GetInt32(reader.GetOrdinal("id_ticket")),
                    titulo = reader.GetString(reader.GetOrdinal("titulo")),
                    descricao = reader.GetString(reader.GetOrdinal("descricao")),
                    status = Enum.Parse<StatusTicket>(reader.GetString(reader.GetOrdinal("status"))),
                    prioridade = Enum.Parse<PrioridadeTicket>(reader.GetString(reader.GetOrdinal("prioridade"))),
                    data_fechamento = reader.IsDBNull(reader.GetOrdinal("data_fechamento"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("data_fechamento")),
                    id_usuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                    id_usuario_responsavel = reader.GetInt32(reader.GetOrdinal("id_usuario_responsavel")),
                    id_categoria = reader.GetInt32(reader.GetOrdinal("id_categoria"))
                };

                tickets.Add(ticket);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar tickets: {ex.Message}");
            throw;
        }

        return tickets;
    }


    public void UpdateStatus(int id_ticket, StatusTicket newStatus)
        {
            try
            {
                var conn = DataBaseController.GetConnection();
                conn.Open();

                var query = "UPDATE ticket SET status = @newStatus WHERE id_ticket = @id_ticket ";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("newStatus", newStatus);
                cmd.Parameters.AddWithValue("id_ticket", id_ticket);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}