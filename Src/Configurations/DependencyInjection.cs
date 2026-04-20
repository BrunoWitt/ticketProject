using Microsoft.Extensions.DependencyInjection;
using Src.Modules.User.Repository;
using Src.Modules.User.Service;
using Src.Modules.Ticket.Repository;
using Src.Modules.Ticket.Service;
using Src.Modules.Message.Repository;
using Src.Modules.Message.Service;

namespace Src.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        return services
            .AddUserModule()
            .AddTicketModule()
            .AddMessageModule();
    }


    private static IServiceCollection AddUserModule(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<UserService>();

        return services;
    }


    private static IServiceCollection AddTicketModule(this IServiceCollection services)
    {
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<TicketService>();

        return services;
    }


    private static IServiceCollection AddMessageModule(this IServiceCollection services)
    {
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<MessageService>();

        return services;
    }
}