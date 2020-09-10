using ApiTwo.Configuration;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiTwo.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<IdentityConfiguration> _identityOptions;
        private readonly IOptions<ApiOneConfiguration> _apiOneOptions;

        public HomeController(IHttpClientFactory httpClientFactory,
            IOptions<IdentityConfiguration> identityOptions,
            IOptions<ApiOneConfiguration> apiOneOptions)
        {
            _httpClientFactory = httpClientFactory;
            _identityOptions = identityOptions;
            _apiOneOptions = apiOneOptions;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // get access token
            var serverClient = _httpClientFactory.CreateClient();

            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(_identityOptions.Value.AuthorityEndpoint);

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = _apiOneOptions.Value.ClientId,
                ClientSecret = _apiOneOptions.Value.ClientSecret,
                Scope = _apiOneOptions.Value.Scope
            });

            // get secret data
            var apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient.GetAsync($"{_apiOneOptions.Value.Endpoint}api/secret");
            var secretData = await response.Content.ReadAsStringAsync();

            return Ok(new
            {
                access_token = tokenResponse.AccessToken,
                secret_data = secretData
            });
        }
    }
}
