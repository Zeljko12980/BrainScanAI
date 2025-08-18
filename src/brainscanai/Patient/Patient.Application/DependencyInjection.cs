using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Patient.Application.Handlers;

namespace Patient.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(ctg =>
            {
                ctg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddMessageBroker(configuration, typeof(TumorAnalysisCompletedEventHandler).Assembly);
            return services;
        }
    }
}
