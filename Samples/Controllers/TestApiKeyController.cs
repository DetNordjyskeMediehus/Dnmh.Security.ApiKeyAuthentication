using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dnmh.Security.ApiKeyAuthentication.Sample.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = "ApiKey")]
public class TestApiKeyController : ControllerBase
{
    /// <summary>
    /// Tests the ApiKey Authentication. 
    /// </summary>
    [HttpGet]
    public string Get() => "Congratulations! You supplied a valid api key!";
}
