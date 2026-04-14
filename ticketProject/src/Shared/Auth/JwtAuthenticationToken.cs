using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ticketProject.src.Modules.Usuario.Models;
using DotNetEnv;
using System.Security.Cryptography;

namespace ticketProject.src.Shared.Auth
{
    public class Auth
    {
        public Task<string> GenerateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.id_usuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.email),
                new Claim(ClaimTypes.Role, usuario.perfil_usuario.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("SECRET_KEY")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }


        public Task<string> CreateRefreshToken()
        {
            var uniqueString = $"{Guid.NewGuid()}-{DateTimeOffset.Now.Ticks}";
            
            var randomNumber = new byte[32];

            RandomNumberGenerator.Create().GetBytes(randomNumber);

            return Task.FromResult(Convert.ToBase64String(randomNumber) + uniqueString);
        }


        public T? ReadProperty<T>(string token, string property)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                if (handler.ReadToken(token.Replace("Bearer ", "")) is not JwtSecurityToken jwt)
                    throw new ArgumentNullException(nameof(token));

                var properties = property.Split(".").ToList();

                if (properties.Count == 0)
                    throw new ArgumentNullException(nameof(property));

                var payload = jwt.Payload;

                foreach (var prop in properties)
                {
                    if (properties.IndexOf(prop) == properties.Count - 1)
                    {
                        var result = payload.GetValueOrDefault(prop);
                        return result == null ? default : (T)Convert.ChangeType(result, typeof(T));
                    }

                    payload = JwtPayload.Deserialize(payload.GetValueOrDefault(prop)!.ToString());
                }

                return default;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("Erro ao validar token", ex);
            }
        }
    }
}