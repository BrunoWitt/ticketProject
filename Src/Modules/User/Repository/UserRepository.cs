using Src.Modules.User.DTOs;
using Src.Modules.User.Models;
using Src.Shared.DataBase;
using Npgsql;
using System.ComponentModel;
using Src.Shared.Base;


namespace Src.Modules.User.Repository
{
    public class UserRepository : BaseRepository<UsuarioModel>, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {
    }

    protected override string TableName => "usuario";
    protected override string IdColumn => "id";

    public async Task<UsuarioModel?> GetByEmail(string email)
    {
        using var conn = Connection;
        await conn.OpenAsync();

        var query = "SELECT * FROM usuario WHERE email = @email LIMIT 1";

        using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("email", email);

        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
            return Map(reader);

        return null;
    }
}

}