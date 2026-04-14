using ticketProject.src.Modules.Usuario.Repository;
using ticketProject.src.Modules.Usuario.Services;
using ticketProject.src.Shared.Auth;
using ticketProject.src.Hubs;
using ticketProject.src.Modules.TicketS.Services;
using ticketProject.src.Modules.Repository;
using ticketProject.src.Repositories;
using ticketProject.src.Modules.Mensagem.Repository;
using ticketProject.src.Modules.Mensagem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<Auth>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<MessageService>();

builder.Services.AddCors(options =>{
    options.AddPolicy("AllowAll", policy =>{
        policy
            .WithOrigins(
                "http://127.0.0.1:5500",
                "http://localhost:5500",
                "http://localhost:5248"
                )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.MapControllers();

app.MapHub<MessageHub>("/messageHub");

app.Run();