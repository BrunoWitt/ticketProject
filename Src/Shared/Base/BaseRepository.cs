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


    public async Task<TEntity?> GetByIdAsync(int Id)
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
        var props = typeof(TEntity)
            .GetProperties()
            .Where(p => p.Name.ToLower() != "id");

        var columns = string.Join(", ", props.Select(p => ToSnakeCase(p.Name)));

        var values = string.Join(", ", props.Select(p =>
        {
            var column = ToSnakeCase(p.Name);

            if (p.PropertyType.IsEnum)
            {
                var enumTypeName = ToSnakeCase(p.PropertyType.Name);
                return "@" + column + "::" + enumTypeName;
            }

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
                cmd.Parameters.AddWithValue(column, value.ToString());
            else
                cmd.Parameters.AddWithValue(column, value ?? DBNull.Value);
        }

        await cmd.ExecuteNonQueryAsync();
    }


    public async Task UpdateAsync(TEntity entity)
    {
        var props = typeof(TEntity).GetProperties();

        var idProp = props.FirstOrDefault(p => p.Name.ToLower() == "id");
        if (idProp == null)
            throw new Exception("Id property not found");

        var setProps = props.Where(p => p.Name.ToLower() != "id");

        var setClause = string.Join(", ", setProps.Select(p =>
        {
            var column = ToSnakeCase(p.Name);

            if (p.PropertyType.IsEnum)
                return $"{column} = @{column}::" + GetEnumTypeName(p.PropertyType);

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

            if (prop.PropertyType.IsEnum)
                cmd.Parameters.AddWithValue(column, value?.ToString() ?? (object)DBNull.Value);
            else
                cmd.Parameters.AddWithValue(column, value ?? DBNull.Value);
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
            var columnName = ToSnakeCase(prop.Name);

            if (!reader.HasColumn(columnName)) continue;

            var value = reader[columnName];

            if (value == DBNull.Value)
            {
                prop.SetValue(entity, null);
                continue;
            }

            var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            try
            {
                if (targetType.IsEnum)
                {
                    var enumValue = Enum.Parse(targetType, value.ToString()!, true);
                    prop.SetValue(entity, enumValue);
                }
                else
                {
                    if (targetType == typeof(DateTimeOffset) && value is DateTime dt)
                    {
                        prop.SetValue(entity,
                            new DateTimeOffset(
                                DateTime.SpecifyKind(dt, DateTimeKind.Utc)
                            )
                        );
                    }
                    else
                    {
                        var safeValue = Convert.ChangeType(value, targetType);
                        prop.SetValue(entity, safeValue);
                    }
                }
            }
            catch
            {
                throw new Exception($"Erro ao converter coluna '{columnName}' ({value.GetType()}) para {targetType}");
            }
        }

        return entity;
    }


    protected string ToSnakeCase(string name)
    {
        return string.Concat(name.Select((x, i) =>
            i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()
        )).ToLower();
    }


    private string GetEnumTypeName(Type enumType)
    {
        return ToSnakeCase(enumType.Name);
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


