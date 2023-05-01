using FluentValidation;

namespace Dnmh.Security.ApiKeyAuthentication.AuthenticationHandler.Internal;

/// <summary>
/// Validator for <see cref="ApiKeyAuthenticationOptions"/>
/// </summary>
internal class ApiKeyAuthenticationOptionsValidator : AbstractValidator<ApiKeyAuthenticationOptions>
{
    public ApiKeyAuthenticationOptionsValidator()
    {
        RuleFor(x => x.AllowApiKeyInQuery).Must(x => x).When(x => !x.AllowApiKeyInRequestHeader)
            .WithMessage($"Both {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInQuery)} and {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInRequestHeader)} cannot be false");
        RuleFor(x => x.AllowApiKeyInRequestHeader).Must(x => x).When(x => !x.AllowApiKeyInQuery)
            .WithMessage($"Both {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInQuery)} and {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInRequestHeader)} cannot be false");
        RuleFor(x => x.AllowApiKeyInRequestHeader).Must(x => x).When(x => x.UseAuthorizationHeaderKey)
            .WithMessage($"Setting {nameof(ApiKeyAuthenticationOptions.UseAuthorizationHeaderKey)} to true requires {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInRequestHeader)} to also be true");
        RuleFor(x => x.AllowApiKeyInRequestHeader).Must(x => x).DependentRules(() => RuleFor(x => x.UseAuthorizationHeaderKey).Must(x => x)).When(x => x.UseSchemeNameInAuthorizationHeader)
            .WithMessage($"Setting {nameof(ApiKeyAuthenticationOptions.UseSchemeNameInAuthorizationHeader)} to true requires {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInRequestHeader)} and {nameof(ApiKeyAuthenticationOptions.UseAuthorizationHeaderKey)} to also be true");
        RuleFor(x => x.HeaderKey).NotNull().NotEmpty().When(x => x.AllowApiKeyInRequestHeader)
            .WithMessage($"{nameof(ApiKeyAuthenticationOptions.HeaderKey)} must be non-empty when {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInRequestHeader)} is true");
        RuleFor(x => x.QueryKey).NotNull().NotEmpty().When(x => x.AllowApiKeyInQuery)
            .WithMessage($"{nameof(ApiKeyAuthenticationOptions.QueryKey)} must be non-empty when {nameof(ApiKeyAuthenticationOptions.AllowApiKeyInQuery)} is true");
        RuleFor(x => x.AuthorizationSchemeInHeader).NotNull().NotEmpty().When(x => x.UseAuthorizationHeaderKey && !x.UseSchemeNameInAuthorizationHeader)
            .WithMessage($"{nameof(ApiKeyAuthenticationOptions.AuthorizationSchemeInHeader)} must be non-empty when {nameof(ApiKeyAuthenticationOptions.UseAuthorizationHeaderKey)} is true and {nameof(ApiKeyAuthenticationOptions.UseSchemeNameInAuthorizationHeader)} is false");
    }
}
