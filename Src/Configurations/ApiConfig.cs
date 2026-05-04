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
using Src.Shared.Authentication;
using Microsoft.OpenApi.Models;
using Src.Modules.Category.Service;
using Src.Modules.Category.Repository;
using Src.Modules.RefreshToken.Service;
using Src.Shared.Middlewares;
using Src.Modules.Historico.Service;
using Src.Modules.Historico.Repository;

namespace Src.Configurations;

public static class ApiConfig
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
{
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Digite: Bearer {seu token}"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

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

        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<CategoryService>();

        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<RefreshTokenService>();

        builder.Services.AddScoped<HistoricoService>();
        builder.Services.AddScoped<IHistoricoRepository, HistoricoRepository>();

        builder.Services.AddScoped<AuthService>();

        builder.Services.AddSignalR();
    }


    public static void ConfigureApp(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<Src.Modules.Message.Hub.MessageHub>("/hub/messages");
    }
}