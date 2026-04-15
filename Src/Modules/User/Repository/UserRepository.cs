using Src.Modules.User.DTOs;
using Src.Modules.User.Models;
using Src.Shared.DataBase;
using Npgsql;
using System.ComponentModel;


namespace Src.Modules.User.Repository
{
    //Create User
    //Read all users
    //Update User informations
    //Delete(soft) user
    public class UserRepository : IUserRepository
    {
        public async Task Create(string nome, string email, string senhaHash, PerfilUsuario perfilUsuario)
        {
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                await conn.OpenAsync();

                string query = "INSERT INTO usuario (nome, email, senha, perfil_usuario) VALUES (@nome, @email, @senha, @perfil_usuario::perfil_usuario)";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("nome", nome);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("senha", senhaHash);
                cmd.Parameters.AddWithValue("perfil_usuario", perfilUsuario.ToString());

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        public async Task<List<UsuarioModel>> Read()
        {
            var userList = new List<UsuarioModel>();

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                var query = @" SELECT id_ticket, titulo, descricao, status, prioridade, data_fechamento, id_usuario, id_usuario_responsavel, id_categoria FROM usuario;";

                using var cmd = new NpgsqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var ticket = new UsuarioModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Senha = reader.GetString(reader.GetOrdinal("senha")),
                        PerfilUsuario = Enum.Parse<PerfilUsuario>(reader.GetString(reader.GetOrdinal("status"))),
                    };

                    userList.Add(ticket);
                }

                return userList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        public async Task Update(int id, string nome, string email, string senhaHash, PerfilUsuario perfilUsuario)
        {
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                await conn.OpenAsync();

                string query = @"
                    UPDATE usuario 
                    SET 
                        nome = @nome,
                        email = @email,
                        senha = @senha,
                        perfil_usuario = @perfil_usuario::perfil_usuario
                    WHERE id_usuario = @id;
                ";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("nome", nome);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("senha", senhaHash);
                cmd.Parameters.AddWithValue("perfil_usuario", perfilUsuario.ToString());

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        public async Task Delete(int id)
        {
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                await conn.OpenAsync();

                string query = @"
                    UPDATE usuario
                    SET data_hora_delecao = NOW()
                    WHERE id_usuario = @id;
                ";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("id", id);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        public async Task<UsuarioModel?> GetByEmail(string emailInput)
        {
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"SELECT id_usuario, nome, email, senha, perfil_usuario FROM usuario WHERE email = @email LIMIT 1";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("email", emailInput);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new UsuarioModel()
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Email = reader.GetString(2),
                        Senha = reader.GetString(3),
                        PerfilUsuario = Enum.Parse<PerfilUsuario>(reader.GetString(4))
                    };
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return null;
        }
    }

}