using Dnmh.Security.ApiKeyAuthentication.Context;
using Dnmh.Security.ApiKeyAuthentication.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Dnmh.Security.ApiKeyAuthentication;

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
        IApiKeyAuthenticationService authenticationService)
        : base(options, logger, encoder)
    {
        _authenticationService = authenticationService;
    }

    /// <inheritdoc/>
    protected override Task InitializeHandlerAsync()
    {
        Options.Validate();
        Options.InitializeDefaultValues();
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
            IEnumerable<string>? apiKeys = null;
            if (Options.AllowApiKeyInRequestHeader && TryExtractFromHeader(Request, out apiKeys) && apiKeys is null)
            {
                return await FailAuthentication(new FailedAuthenticationException("Missing api key"));
            }

            if (apiKeys is null && Options.AllowApiKeyInQuery && TryExtractFromQuery(Request, out apiKeys) && apiKeys is null)
            {
                return await FailAuthentication(new FailedAuthenticationException("Missing api key"));
            }

            if (apiKeys is null)
            {
                return AuthenticateResult.NoResult();
            }

            ClaimsPrincipal? validPrinciple = null;
            foreach (var apiKey in apiKeys)
            {
                validPrinciple = await _authenticationService.ValidateAsync(new ValidationContext(Context, Scheme, Options, apiKey!));
                if (validPrinciple is not null)
                {
                    break;
                }
            }

            if (validPrinciple is null)
            {
                return await FailAuthentication(new FailedAuthenticationException("Invalid api key"));
            }

            if (Events is not null)
            {
                await Events.OnAuthenticationSuccess(new AuthenticationSuccessContext(Context, Scheme, Options, validPrinciple));
            }
            var ticket = new AuthenticationTicket(validPrinciple, Scheme.Name);
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

    private bool TryExtractFromHeader(HttpRequest request, [MaybeNullWhen(false)] out IEnumerable<string> headerValues)
    {
        if (Options.UseAuthorizationHeaderKey)
        {
            if (!request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                headerValues = default;
                // Authorization header not in request
                return false;
            }

            if (!AuthenticationHeaderValue.TryParse(request.Headers[HeaderNames.Authorization], out var authenticationHeaderValue))
            {
                headerValues = default;
                //Invalid Authorization header
                return false;
            }

            if (authenticationHeaderValue.Parameter is null)
            {
                headerValues = default;
                // Invalid Authorization header
                return false;
            }

            var schemeName = Options.UseSchemeNameInAuthorizationHeader ? Scheme.Name : Options.AuthorizationSchemeInHeader;
            if (!schemeName.Equals(authenticationHeaderValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                // Not correct scheme authentication header
                headerValues = default;
                return false;
            }

            headerValues = new List<string> { authenticationHeaderValue.Parameter };
        }
        else
        {
            var validKeys = Options.HeaderKeys.Intersect(request.Headers.Keys);
            if (validKeys == null || !validKeys.Any())
            {
                headerValues = default;
                // Authorization header not in request
                return false;
            }

            headerValues = request.Headers.Where(x => validKeys.Contains(x.Key)).SelectMany(x => x.Value).Where(x => x != null).Select(x => x!);
        }

        return true;
    }

    private bool TryExtractFromQuery(HttpRequest request, [MaybeNullWhen(false)] out IEnumerable<string> queryValues)
    {
        var validKeys = Options.QueryKeys.Intersect(request.Query.Keys);
        if (validKeys != null && validKeys.Any())
        {
            queryValues = request.Query.Where(x => validKeys.Contains(x.Key)).SelectMany(x => x.Value).Where(x => x != null).Select(x => x!);
            return true;
        }

        queryValues = null;
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
