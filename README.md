# DNMH.Security.ApiKeyAuthentication

## Overview

This project contains two libraries that provide API key authentication for your web applications:

 - `ApiKeyAuthentication` implements an AuthenticationHandler that validates API keys in the Authorization HTTP requests.
 - `ApiKeyAuthentication.SwwaggerUI` provides an extension method for SwaggerGenOptions that adds an "Authorize" button to the Swagger UI that lets you enter your API key.
  
## Installation

You can install this library using NuGet. Simply run the following command:

```bash
dotnet add package DNMH.Security.ApiKeyAuthentication
```

which will add API key authentication to your .NET Core application.

Or:

```bash
dotnet add package DNMH.Security.ApiKeyAuthentication.SwaggerUI
```

which will add API key authentication to your .NET Core application and a Swagger UI.

## Usage

Each library has its own usage instructions:

 - See the [README.md](Source/ApiKeyAuthentication/README.md) for `ApiKeyAuthentication`.
 - See the [README.md](Source/ApiKeyAuthentication.SwaggerUI/README.md) for `ApiKeyAuthentication.SwaggerUI`.

## License

This library is licensed under the [MIT License](LICENSE).