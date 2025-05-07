using Microsoft.AspNetCore.Components.Authorization;

namespace Capybara.Services
{
    public class BaseService : IBaseService
    {
        protected readonly IConfiguration _configuration;
        protected readonly HttpClient _httpClient;
        protected readonly IJSRuntime _jSRuntime;
        protected readonly IAccessTokenProvider _AuthorizationService;
        protected readonly ILocalStorageService _localStorage;
        protected readonly AuthenticationStateProvider _authenticationStateProvider;
        protected readonly string? apiUrl;
        public BaseService(
            IConfiguration configuration,
            HttpClient httpClient,
            IJSRuntime jSRuntime,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IAccessTokenProvider AuthorizationService

            )
        {
            _httpClient = httpClient;
            _jSRuntime = jSRuntime;
            _AuthorizationService = AuthorizationService;
            _configuration = configuration;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;

            apiUrl = _configuration.GetValue<string>("mcce.srv") ?? throw new ArgumentNullException(nameof(apiUrl));
        }
    }
}
