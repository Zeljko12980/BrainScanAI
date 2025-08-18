using Auth.Application.Common.Interface;
using Auth.Application.Services;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;

namespace Auth.Aplication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddMediatR(ctg =>
            {
                ctg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
            services.AddSingleton<IPasswordGenerator, PasswordGenerator>();

            return services;
        }
    }
}
