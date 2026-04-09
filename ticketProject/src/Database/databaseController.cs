using Npgsql;
using DotNetEnv;

namespace ticketProject.src.Database
{
    internal class DataBaseController
    {
        public static NpgsqlConnection GetConnection()
        {
            ///Cria conexão com o banco de dados
            Env.Load();
            var host = Environment.GetEnvironmentVariable("HOST");
            var port = Environment.GetEnvironmentVariable("PORT");
            var user = Environment.GetEnvironmentVariable("USERNAME");
            var password = Environment.GetEnvironmentVariable("PASSWORD");
            var database = Environment.GetEnvironmentVariable("DATABASE");

            var connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={database}";

            return new NpgsqlConnection(connectionString);
        }
    }
}