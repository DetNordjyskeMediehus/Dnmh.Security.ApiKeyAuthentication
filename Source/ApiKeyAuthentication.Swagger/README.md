# DNMH.Security.ApiKeyAuthentication

## Overview

`DNMH.Security.ApiKeyAuthentication.Swagger` is a .NET library that provides API key authentication for your web applications, including a Swagger and Swagger UI.

## Installation

You can install this library using NuGet. Simply run the following command:

```bash
dotnet add package DNMH.Security.ApiKeyAuthentication.Swagger
```

## Usage

To use this library, follow these steps:

1. In your `Startup.cs` file, add the following code to the `ConfigureServices` method:

```csharp
services.AddAuthentication("ApiKey")
        .AddApiKeyAuthentication("MySecretApiKey")
        .AddSwaggerAuthorization("Description");
```

2. In your controller or endpoint, add the `[Authorize(AuthenticationSchemes = "ApiKey")]` (or simply `[Authorize]`) attribute to require an API key for that endpoint:

```csharp
[HttpGet]
[Authorize(AuthenticationSchemes = "ApiKey")]
public IActionResult Get()
{
    // Your code here
}
```

That's it! Now your API endpoints will require an API key to access them, and Swagger UI will show an "Authorize" button that lets you enter your API key.

## Example

Here's an example of how to use this library in a .NET Core application:

```csharp
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DNMH.Security.ApiKeyAuthentication;

namespace MyApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Your JWT configuration here
            });

            services.AddApiKeyAuthentication<ApiKeyAuthenticationService>()
                    .AddSwaggerAuthorization("API Key Authorization");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = "ApiKey")]
        public IActionResult Get()
        {
            return Ok("Hello, world!");
        }
    }
}

public class ApiKeyAuthenticationService : SimpleApiKeyAuthenticationServiceBase
{
    public bool Validate(ValidationContext context)
    {
        return context.ApiKey == "YOUR_API_KEY";
    }
}
```

In this example, the `AddApiKeyAuthentication` method is called with a generic type parameter `ApiKeyAuthenticationService` that implements the `IApiKeyAuthenticationService` interface with a `ValidateApiKey` method that checks if the provided API key matches the expected value. The `AddSwaggerAuthorization` method is called with a string parameter that specifies the description of the authorization method that will be shown in Swagger UI. The `[Authorize(AuthenticationSchemes = "ApiKey")]` attribute is added to the `Get` method of the `MyController` class to require an API key for that endpoint.

## License

This library is licensed under the [MIT License](LICENSE).