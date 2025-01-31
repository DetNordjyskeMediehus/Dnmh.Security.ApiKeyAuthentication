using DNMH.Security.ApiKeyAuthentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adds the default authentication scheme named "ApiKey". This is activated on a controller by using the [Authorize] attribute.
builder.Services.AddAuthentication("ApiKey")
    .AddApiKeyAuthentication(sp => builder.Configuration.GetRequiredSection("MyApiKey").Value!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
