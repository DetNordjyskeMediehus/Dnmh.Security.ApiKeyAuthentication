using Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler;

/// <summary>
/// Handles Api Key authentication, by checking the request header and request query parameters for occurrences of the api key (depending on <see cref="ApiKeyAuthenticationOptions"/>)
/// </summary>
public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly IApiKeyAuthenticationService _authenticationService;

    /// <inheritdoc/>
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IApiKeyAuthenticationService authenticationService)
        : base(options, logger, encoder, clock)
    {
        _authenticationService = authenticationService;
    }

    /// <inheritdoc/>
    protected override Task InitializeHandlerAsync()
    {
        Options.Validate();
        return base.InitializeHandlerAsync();
    }

    /// <inheritdoc/>
    protected new ApiKeyAuthenticationEvents? Events { get => (ApiKeyAuthenticationEvents?)base.Events; set => base.Events = value; }

    /// <inheritdoc/>
    protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new ApiKeyAuthenticationEvents());

    /// <inheritdoc/>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            string? apiKey = null;
            if (Options.AllowApiKeyInRequestHeader && TryExtractFromHeader(Request, out apiKey) && apiKey is null)
            {
                return await FailAuthentication(new FailedAuthenticationException("Missing api key"));
            }

            if (apiKey is null && Options.AllowApiKeyInQuery && TryExtractFromQuery(Request, out apiKey) && apiKey is null)
            {
                return await FailAuthentication(new FailedAuthenticationException("Missing api key"));
            }

            if (apiKey is null)
            {
                return AuthenticateResult.NoResult();
            }

            var result = await _authenticationService.ValidateAsync(new ValidationContext(Context, Scheme, Options, apiKey!));

            if (result is null)
            {
                return await FailAuthentication(new FailedAuthenticationException("Invalid api key"));
            }

            if (Events is not null)
            {
                await Events.OnAuthenticationSuccess(new AuthenticationSuccessContext(Context, Scheme, Options, result));
            }
            var ticket = new AuthenticationTicket(result, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            return await FailAuthentication(ex);
        }
    }

    /// <inheritdoc/>
    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers[HeaderNames.WWWAuthenticate] = Scheme.Name;
        await base.HandleChallengeAsync(properties);
    }

    private bool TryExtractFromHeader(HttpRequest request, [MaybeNullWhen(false)] out string headerValue)
    {
        if (Options.UseAuthorizationHeaderKey)
        {
            if (!request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                headerValue = default;
                // Authorization header not in request
                return false;
            }

            if (!AuthenticationHeaderValue.TryParse(request.Headers[HeaderNames.Authorization], out var authenticationHeaderValue))
            {
                headerValue = default;
                //Invalid Authorization header
                return false;
            }

            if (authenticationHeaderValue.Parameter is null)
            {
                headerValue = default;
                // Invalid Authorization header
                return false;
            }

            var schemeName = Options.UseSchemeNameInAuthorizationHeader ? Scheme.Name : Options.AuthorizationSchemeInHeader;
            if (!schemeName.Equals(authenticationHeaderValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                // Not correct scheme authentication header
                headerValue = default;
                return false;
            }
            
            headerValue = authenticationHeaderValue.Parameter;
        }
        else
        {
            if (!request.Headers.ContainsKey(Options.HeaderKey))
            {
                headerValue = default;
                // Authorization header not in request
                return false;
            }

            headerValue = request.Headers[Options.HeaderKey]!;
        }

        return true;
    }

    private bool TryExtractFromQuery(HttpRequest request, [MaybeNullWhen(false)] out string queryValue)
    {
        if (request.Query.ContainsKey(Options.QueryKey))
        {
            queryValue = request.Query[Options.QueryKey]!;
            return true;
        }

        queryValue = null;
        return false;
    }

    private async Task<AuthenticateResult> FailAuthentication(Exception ex)
    {
        if (Events is not null)
        {
            await Events.OnAuthenticationFailed(new AuthenticationFailedContext(Context, Scheme, Options, ex));
        }
        return AuthenticateResult.Fail(ex.Message);
    }
}
