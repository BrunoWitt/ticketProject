using ticketProject.src.Database;
using Npgsql;

namespace ticketProject.src.Modules.Usuario.Repository 
{
    internal class UsuarioRepository : IUsuarioRepository
    {
        //loggin
        //listar todos os usuários
        //get informações
        public async Task<Models.Usuario?> ValidadeUserLoginDB(string emailInput, string passwordHash)
        {
            try
            {
                using var conn = DataBaseController.GetConnection();
                conn.Open();

                var query = @"SELECT id_usuario, nome, email, perfil_usuario FROM usuario WHERE email = @email AND senha = @passwordHash LIMIT 1";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("email", emailInput);
                cmd.Parameters.AddWithValue("passwordHash", passwordHash);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var id_user = reader.GetInt32(0);
                    var name = reader.GetString(1);
                    var perfil_user_str = reader.GetString(3);

                    var perfil_user = Enum.Parse<Models.PerfilUsuario>(perfil_user_str);

                    var usuario = new Models.Usuario()
                    {
                        id_usuario = id_user,
                        nome = name,
                        email = emailInput,
                        senha = passwordHash,
                        perfil_usuario = perfil_user
                    };

                    return usuario;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return null;
        }
    }
}