using DNMH.Security.ApiKeyAuthentication;
using DNMH.Security.ApiKeyAuthentication.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Adds the default authentication scheme named "ApiKey". This is activated on a controller by using the [Authorize] attribute.
builder.Services.AddAuthentication("ApiKey")
    .AddApiKeyAuthentication(sp => builder.Configuration.GetRequiredSection("MyApiKey").Value!)
    .AddSwaggerAuthorization("Please add a valid api key (the valid api key is found in appsettings.json 🤫)");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();