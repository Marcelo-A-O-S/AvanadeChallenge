using SaleService.API.Extensions;
using SaleService.API.Middleware;
using SaleService.Application.Extensions;
using SaleService.Infrastructure.Extensions;
using System.Text.Json.Serialization;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddSwaggerConfig();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfig();
builder.Services.AddInfrastructureExtensions(builder.Configuration);
builder.Services.AddApplicationLayerExtensions();

var app = builder.Build();
app.Services.ApplyMigrations();
// Configure the HTTP request pipeline.
app.UseCors("AllowAll");
app.UseSwagger(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
});
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run("http://+:5002");

