namespace Doctor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DoctorDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

         //   services.AddScoped<IDoctorService, DoctorService>();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddScoped<IDoctorService, DoctorService>(sp =>
            {
                var context = sp.GetRequiredService<DoctorDbContext>();
                var mapper = sp.GetRequiredService<IMapper>();
                var grpcAddress = configuration["GrpcSettings:BrainTumorAnalyzerAddress"];

                return new DoctorService(context, mapper, grpcAddress);
            });

            services.AddScoped<IBrainTumorAnalyzer>(sp =>
            {
                var context = sp.GetRequiredService<DoctorDbContext>();
                var mapper = sp.GetRequiredService<IMapper>();
                var grpcAddress = configuration["GrpcSettings:BrainTumorAnalyzerAddress"];

                return new DoctorService(context, mapper, grpcAddress);
            });
            return services;
        }
    }
}
