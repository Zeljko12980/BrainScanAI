using Appointment.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCarter();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppointmentDbContext>();
        dbContext.Database.Migrate();
        AppointmentSeed.Initialize(dbContext);
    }
}

app.MapCarter();
app.MapHub<AppointmentHub>("/hubs/appointments");

app.Run();


