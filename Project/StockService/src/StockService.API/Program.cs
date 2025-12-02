using System.Text.Json.Serialization;
using StockService.API.Extensions;
using StockService.Application.Extensions;
using StockService.Infrastructure.Extensions;
using StockService.API.Middleware;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddSwaggerConfig();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfig();
builder.Services.AddInfrastructureExtension(builder.Configuration);
builder.Services.AddApplicationLayerExtension();
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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run("http://+:5003");

