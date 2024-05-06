using Microsoft.OpenApi.Models;
using NewAppVS.Middleware;
using Serilog;
using UPB.BusinessLogic.Managers;

var builder = WebApplication.CreateBuilder(args);

// Chaining Method
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile(
        "appsettings." + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") + ".json"
    )
    .Build();


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
Log.Information("Initialize ");


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<PatientManager>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var appName = builder.Configuration.GetSection("AppInfo:ApiName").Value;
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = appName,
        Version = "v1",
        Description = "API para manejar pacientes",
        TermsOfService = new Uri("https://ejemplo.com/terminos"),
        Contact = new OpenApiContact
        {
            Name = "Ayuda",
            Email = "ayuda@example.com",
            Url = new Uri("https://ayuda.example.com")
        }, // Optional
        License = new OpenApiLicense
        {
            Name = "Licencias",
            Url = new Uri("https://ejemplo.com/licencia")
        } // Optional
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "QA")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseErrorLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// RUN
app.Run();