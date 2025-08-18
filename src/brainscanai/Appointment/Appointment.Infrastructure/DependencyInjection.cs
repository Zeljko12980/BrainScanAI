namespace Appointment.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
       (this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppointmentDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddAutoMapper(typeof(MappingProfile.MappingProfile).Assembly);
            return services;
        }
    }
}
