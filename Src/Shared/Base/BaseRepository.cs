using System.Data.SqlTypes;
using System.Numerics;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Npgsql;
using Src.Shared.Interfaces;

namespace Src.Shared.Base;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseModel, new()
{
    private readonly IConfiguration _configuration;

    protected NpgsqlConnection Connection =>
        new(_configuration.GetConnectionString("Default"));

    protected BaseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected virtual string TableName => typeof(TEntity).Name.ToLower();
    protected virtual string IdColumn => typeof(TEntity).Name.ToLower();


    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var query = $"SELECT * FROM {TableName} WHERE Data_Hora_Delecao IS NULL";

        var list = new List<TEntity>();

        using var conn = Connection;
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(query, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var entity = Map(reader);
            list.Add(entity);
        }

        return list;
    }


    public async Task<TEntity?> GetByIdAsync(Guid Id)
    {
        var query = $"SELECT * FROM {TableName} WHERE {IdColumn} = @id";

        using var conn = Connection;
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("id", Id);

        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
            return Map(reader);
        
        return null;
    }


    public async Task CreateAsync(TEntity entity)
    {
        var props = typeof(TEntity).GetProperties();

        var columns = string.Join(", ", props.Select(p => ToSnakeCase(p.Name)));
        var values = string.Join(", ", props.Select(p =>
        {
            var column = ToSnakeCase(p.Name);

            if (p.PropertyType.IsEnum)
                return "@" + column + "::perfil_usuario"; 

            return "@" + column;
        }));

        var sql = $"INSERT INTO {TableName} ({columns}) VALUES ({values})";

        using var conn = Connection;
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);

        foreach (var prop in props)
        {
            var column = ToSnakeCase(prop.Name);
            var value = prop.GetValue(entity);

            if (value is Enum)
            {
                cmd.Parameters.AddWithValue(column, value.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue(column, value ?? DBNull.Value);
            }
        }

        await cmd.ExecuteNonQueryAsync();
    }


    public async Task UpdateAsync(TEntity entity)
    {
        var props = typeof(TEntity).GetProperties();

        var idProp = props.FirstOrDefault(p => 
            ToSnakeCase(p.Name) == IdColumn);

        if (idProp == null)
            throw new Exception($"Id property '{IdColumn}' not found in entity");

        var setClause = string.Join(", ", props
            .Where(p => ToSnakeCase(p.Name) != IdColumn)
            .Select(p =>
            {
                var column = ToSnakeCase(p.Name);

                if (p.PropertyType.IsEnum)
                    return $"{column} = @{column}::perfil_usuario";

                return $"{column} = @{column}";
            }));

        var sql = $"UPDATE {TableName} SET {setClause} WHERE {IdColumn} = @id";

        using var conn = Connection;
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);

        foreach (var prop in props)
        {
            var column = ToSnakeCase(prop.Name);
            var value = prop.GetValue(entity);

            if (column == IdColumn)
            {
                cmd.Parameters.AddWithValue("id", value);
                continue;
            }

            if (value is Enum)
            {
                cmd.Parameters.AddWithValue(column, value.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue(column, value ?? DBNull.Value);
            }
        }

        await cmd.ExecuteNonQueryAsync();
    }


    public async Task DeleteAsync(BigInteger Id)
    {
        var sql = $"UPDATE {TableName} SET data_hora_delecao = NOW() WHERE {IdColumn} = @id";

        using var conn = Connection;
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", Id);

        await cmd.ExecuteNonQueryAsync();
    }


    protected TEntity Map(NpgsqlDataReader reader)
    {
        var entity = new TEntity();

        foreach (var prop in typeof(TEntity).GetProperties())
        {
            var name = prop.Name.ToLower();

            if (!reader.HasColumn(name)) continue;

            var value = reader[name];
            prop.SetValue(entity, value == DBNull.Value ? null : value);
        }

        return entity;
    }


    protected string ToSnakeCase(string name)
    {
        return string.Concat(name.Select((x, i) =>
            i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()
        )).ToLower();
    }
}

public static class DataReaderExtensions
    {
        public static bool HasColumn(this NpgsqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).ToLower() == columnName)
                    return true;

            return false;
        }
    }


