using Gateway.API.Extensions;
using Gateway.API.Middleware;
using Gateway.Application.Extension;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddYarpConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfig();
builder.Services.AddHttpClient();
builder.Services.AddApplicationExtensions();
var app = builder.Build();
app.UseCors("AllowAll");
app.UseSwagger(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json","Gateway API");
    options.SwaggerEndpoint("http://localhost:5001/swagger/v1/swagger.json","Auth API");
    options.SwaggerEndpoint("http://localhost:5002/swagger/v1/swagger.json","Sale API");
    options.SwaggerEndpoint("http://localhost:5003/swagger/v1/swagger.json","Stock API");
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.MapReverseProxy();
app.Run("http://+:5000");