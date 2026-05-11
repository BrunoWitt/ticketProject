using Src.Modules.Ticket.Models;
using Src.Shared.DataBase;
using Npgsql;
using Microsoft.Extensions.Primitives;
using Src.Shared.Base;
using Src.Modules.Ticket.DTOs;

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

            var query = @"UPDATE ticket SET status = @status WHERE id = @id";

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
                WHERE id = @idTicket;
            ";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("idTicket", idTicket);
            cmd.Parameters.AddWithValue("idAtendente", idAtendente);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<PageResult<TicketResponseDTO>> GetPaged(PaginacaoDTO request)
        {
            var offset = (request.Page - 1) * request.PageSize;

            using var conn = Connection;
            await conn.OpenAsync();

            var where = "WHERE data_hora_delecao IS NULL";
            var parameters = new List<NpgsqlParameter>();

            var search = request.Search?.Trim();

            if (!string.IsNullOrEmpty(search) && search != "string")
            {
                where += @" AND (
                    titulo ILIKE @search OR
                    descricao ILIKE @search
                )";

                parameters.Add(new NpgsqlParameter("search", $"%{search}%"));
            }

            if (request.Status != null)
            {
                where += " AND status = CAST(@status AS status_ticket)";

                parameters.Add(
                    new NpgsqlParameter(
                        "status",
                        request.Status.ToString()
                    )
                );
            }

            if (request.IdCategoria != null || request.IdCategoria == 0)
            {
                where += " AND id_categoria = @idCategoria";

                parameters.Add(
                    new NpgsqlParameter("idCategoria", request.IdCategoria)
                );
            }

            var orderBy = request.OrderBy?.ToLower() switch
            {
                "titulo" => "titulo",
                "status" => "status",
                "prioridade" => "prioridade",
                "datahoracriado" => "data_hora_criado",
                _ => "id"
            };

            var orderDir = request.OrderDir?.ToLower() == "desc"
                ? "DESC"
                : "ASC";

            var sql = $@"
                SELECT *
                FROM ticket
                {where}
                ORDER BY {orderBy} {orderDir}
                LIMIT @limit OFFSET @offset;
            ";

            parameters.Add(new NpgsqlParameter("limit", request.PageSize));
            parameters.Add(new NpgsqlParameter("offset", offset));

            var data = new List<TicketResponseDTO>();

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(parameters.ToArray());

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var ticket = Map(reader);

                    data.Add(new TicketResponseDTO
                    {
                        Id = ticket.Id,
                        Titulo = ticket.Titulo,
                        Descricao = ticket.Descricao,
                        Status = ticket.Status,
                        Prioridade = ticket.Prioridade,
                        IdCategoria = ticket.IdCategoria,
                        IdAtendente = ticket.IdAtendente,
                        DataHoraCriado = ticket.DataHoraCriado,
                        DataHoraFinalizado = ticket.DataHoraFinalizado,
                        Atrasado = false
                    });
                }
            }

            var totalSql = $@"
                SELECT COUNT(*)
                FROM ticket
                {where};
            ";

            var totalParams = parameters
                .Where(p =>
                    p.ParameterName != "limit" &&
                    p.ParameterName != "offset"
                )
                .Select(p => new NpgsqlParameter(p.ParameterName, p.Value))
                .ToArray();

            int total;

            using (var cmd = new NpgsqlCommand(totalSql, conn))
            {
                cmd.Parameters.AddRange(totalParams);

                total = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            return new PageResult<TicketResponseDTO>
            {
                Data = data,
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
