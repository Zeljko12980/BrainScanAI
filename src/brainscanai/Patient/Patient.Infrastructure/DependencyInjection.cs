using Patient.Infrastructure.MappingProfile;
using Patient.Infrastructure.Services;

namespace Patient.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<PatientDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<IPatientService, PatientService>();
            services.AddAutoMapper(typeof(MappingProfile.MappingProfile).Assembly);
            return services;
        }
    }
}
