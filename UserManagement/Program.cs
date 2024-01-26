using System.Net;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context,serverOptions)=>{
    serverOptions.Listen(IPAddress.Any, 7232, listenOptions=>{

    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ApplicationDbContext>(options=>{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
},ServiceLifetime.Scoped);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await dbContext.Database.EnsureCreatedAsync();
    await dbContext.Database.MigrateAsync();
}

//app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async () =>
{
    if(!dbContext.Users.Any()){
        var usersList = new List<User>{
            new User{Name = "Ram"},
            new User{Name = "Shyam"},
            new User{Name = "Hari"}
        };

        await dbContext.Users.AddRangeAsync(usersList);
        await dbContext.SaveChangesAsync();
    }

    return await dbContext.Users.ToListAsync();
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
