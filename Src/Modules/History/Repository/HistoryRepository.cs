using Src.Shared.Base;
using Src.Modules.Historico.Models;
using Npgsql;

namespace Src.Modules.Historico.Repository;

public class HistoricoRepository 
    : BaseRepository<HistoricoModel>, IHistoricoRepository
{
    public HistoricoRepository(IConfiguration config) : base(config)
    {
    }

    protected override string TableName => "historico";

    public async Task<List<HistoricoModel>> GetByTicket(long ticketId)
    {
        var query = $"SELECT * FROM {TableName} WHERE id_ticket = @id";

        using var conn = Connection;
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("id", ticketId);

        using var reader = await cmd.ExecuteReaderAsync();

        var list = new List<HistoricoModel>();

        while (await reader.ReadAsync())
            list.Add(Map(reader));

        return list;
    }
}