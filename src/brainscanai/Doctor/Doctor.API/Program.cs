using Doctor.Application;
using Doctor.Infrastructure;
using Doctor.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCarter();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DoctorDbContext>();
        dbContext.Database.Migrate();
    }
}


app.MapCarter();


app.Run();

