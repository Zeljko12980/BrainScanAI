namespace Notification.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
   (this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<NotificationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddSignalR();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddAutoMapper(typeof(MappingProfile.MappingProfile).Assembly);
            return services;
        }
    }
}
