using Src.Shared.Base;
using Npgsql;

public class RefreshTokenRepository 
    : BaseRepository<RefreshTokenModel>, IRefreshTokenRepository
{
    public RefreshTokenRepository(IConfiguration config) : base(config)
    {
    }

    protected override string TableName => "refresh_token";

    public async Task<RefreshTokenModel?> GetByToken(string token)
    {
        var query = $"SELECT * FROM {TableName} WHERE token = @token";

        using var conn = Connection;
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("token", token);

        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
            return Map(reader);

        return null;
    }
}