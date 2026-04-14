using ticketProject.src.Database;
using Npgsql;
using ticketProject.src.Modules.Usuario.Models;
using ticketProject.src.Modules.Usuario.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ticketProject.src.Modules.Usuario.Repository 
{
    internal class UsuarioRepository : IUsuarioRepository
    {
        //loggin
        //listar todos os usuários
        //get informações
        public async Task<Models.Usuario?> GetByEmail(string emailInput)
        {
            try
            {
                using var conn = DataBaseController.GetConnection();
                conn.Open();

                var query = @"SELECT id_usuario, nome, email, senha, perfil_usuario FROM usuario WHERE email = @email LIMIT 1";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("email", emailInput);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Models.Usuario()
                    {
                        id_usuario = reader.GetInt32(0),
                        nome = reader.GetString(1),
                        email = reader.GetString(2),
                        senha = reader.GetString(3),
                        perfil_usuario = Enum.Parse<Models.PerfilUsuario>(reader.GetString(4))
                    };
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            
            return null;
        }


        public async Task CreateUser(string nome, string email, string senha, Models.PerfilUsuario perfil_usuario)
        {
            try
            {
                using var conn = DataBaseController.GetConnection();
                await conn.OpenAsync();

                var hasher = new PasswordHasher<Models.Usuario>();
                var senhaHash = hasher.HashPassword(new Models.Usuario(), senha);

                string query = "INSERT INTO usuario (nome, email, senha, perfil_usuario) " +
                    "VALUES (@nome, @email, @senha, @perfil_usuario::perfil_usuario_enum)";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("nome", nome);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("senha", senhaHash);
                cmd.Parameters.AddWithValue("perfil_usuario", perfil_usuario.ToString());

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}