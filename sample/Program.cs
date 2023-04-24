using Dnmh.Security.ApiKeyAuthentication.ApiKeyAuthenticationHandler;
using Dnmh.Security.ApiKeyAuthentication.Sample.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Setup the "Authorize" button in the Swagger UI
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Query,
        Description = "Please add a valid api key (the valid api key is found in appsettings.json 🤫)",
        Name = "ApiKey",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKey"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Adds the default authentication scheme named "ApiKey". This is activated on a controller by using the [Authorize(AuthenticationSchemes = "ApiKey")] attribute.
builder.Services.AddAuthentication().AddApiKeyAuthentication<ApiKeyAuthenticationService>();

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
