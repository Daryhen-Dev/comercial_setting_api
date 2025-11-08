using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using comercial_setting_api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddUsers();

builder.Services.AddControllers()
    .AddJsonOptions(opciones =>
    {
        opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Comercial Setting API", Version = "v1" });
});

string dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddSingleton<IDbConnection>(_ => new SqlConnection(dbConnectionString));

// Remove AddOpenApi() if present; Swashbuckle covers OpenAPI
var app = builder.Build();

// Habilitar Swagger siempre (si prefieres solo en dev, reubica dentro del if)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Comercial Setting API v1");
    c.RoutePrefix = "swagger"; // usa "string.Empty" para servir en la raíz "/"
});

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();