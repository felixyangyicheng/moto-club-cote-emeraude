using Capybara.Contracts;
using Capybara.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net;

namespace Capybara.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthenticationService(HttpClient client,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _client = client;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> ForgotPassword(ForgotPasswordModel user)
        {
            //var response = await _client.PostAsJsonAsync(Endpoint.ForgotPasswordEndpoint, user);
            //if (!response.IsSuccessStatusCode)
            //{
            //    return false;
            //}
            //return true;

            throw new NotImplementedException();
        }

        public async Task<bool> Login(LoginModel user)
        {
            //var response = await _client.PostAsJsonAsync(Endpoint.LoginEndpoint, user);
            //if (!response.IsSuccessStatusCode)
            //{
            //    return false;
            //}
            //var content = await response.Content.ReadAsStringAsync();
            //var token = JsonSerializer.Deserialize<TokenResponse>(content);
            ////Store Token
            //await _localStorage.SetItemAsync("authToken", token.Token);
            ////Change auth state of app
            //await ((ApiAuthenticationStateProvider)_authenticationStateProvider)
            //    .LoggedIn();
            //_client.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("bearer", token.Token);
            //return true;
            throw new NotImplementedException();

        }



        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider)
            .LoggedOut();
        }

        public async Task<bool> Register(RegisterModel user)
        {
            //var response = await _client.PostAsJsonAsync(Endpoint.RegisterEndpoint, user);
            //if (!response.IsSuccessStatusCode)
            //{
            //    return false;
            //}
            //return true;
            throw new NotImplementedException();

        }

        public async Task<bool> ResetForgottenPassword(ResetForgottenPasswordModel user)
        {
            //var response = await _client.PostAsJsonAsync(Endpoint.ResetForgottenPasswordEndpoint, user);
            //if (!response.IsSuccessStatusCode)
            //{
            //    return false;
            //}
            //return true;
            throw new NotImplementedException();

        }

        public async Task<bool> ResetPassword(ResetPasswordModel user)
        {
            //var response = await _client.PostAsJsonAsync(Endpoint.ResetPasswordEndpoint, user);
            //if (!response.IsSuccessStatusCode)
            //{
            //    return false;
            //}
            //return true;
            throw new NotImplementedException();

        }
    }

}
