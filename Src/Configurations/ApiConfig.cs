using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Src.Modules.User.Repository;
using Src.Modules.User.Service;
using Src.Modules.Ticket.Repository;
using Src.Modules.Ticket.Service;
using Src.Modules.Message.Repository;
using Src.Modules.Message.Service;
using DotNetEnv;

namespace Src.Configurations;

public static class ApiConfig
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

        var key = Encoding.UTF8.GetBytes(Env.GetString("SECRET_KEY"));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<UserService>();

        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
        builder.Services.AddScoped<TicketService>();

        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddScoped<MessageService>();

        builder.Services.AddSignalR();
    }


    public static void ConfigureApp(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<Src.Modules.Message.Hub.MessageHub>("/hub/messages");
    }
}