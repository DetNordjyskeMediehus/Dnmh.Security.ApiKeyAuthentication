﻿namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler.Internal
{
    /// <summary>
    /// Options for <see cref="PostConfigureSwaggerAuthorization"/> when generating the swagger doc.
    /// </summary>
    internal class ApiKeySchemeSwaggerOptions
    {
        internal string AuthenticationScheme { get; set; } = string.Empty;
        internal string SwaggerDescription { get; set; } = string.Empty;
    }
}
