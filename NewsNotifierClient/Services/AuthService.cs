using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static NewsNotifierClient.Models.AuthModels;
using NewsNotifierClient.Services;

namespace NewsNotifierClient.Services
{
    public class AuthService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        private readonly UserService _userService;
        public string JwtToken { get; private set; } = string.Empty;
        public string Role { get; private set; } = string.Empty;
        public int UserId { get; private set; }
        public HttpClient Client => _client;

        public AuthService(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _client = new HttpClient();
            //_userService = new UserService(_client, _baseUrl);
        }

        public async Task SignupAsync(SignupRequest request)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/auth/signup", request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Signup successful!");
            else
                Console.WriteLine($"Signup failed: {await response.Content.ReadAsStringAsync()}");
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                JwtToken = loginResponse?.Token ?? "";
                Role = loginResponse?.Role ?? "";
                UserId = loginResponse?.UserId ?? 0;

                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);

                Console.WriteLine("Login successful!");
                Console.WriteLine("JWT Token:\n" + JwtToken);
                Console.WriteLine($"Logged in as: {Role}");
                await ShowAccessibleApisAsync();
                return true; 
            }
            else
            {
                Console.WriteLine($"Login failed: {await response.Content.ReadAsStringAsync()}");
                return false;
            }
        }

        public async Task ShowAccessibleApisAsync()
        {
            if (string.IsNullOrEmpty(JwtToken))
            {
                Console.WriteLine("Login token missing. Cannot fetch accessible APIs.");
                return;
            }

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);

            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/user/accessible-apis");

                if (response.IsSuccessStatusCode)
                {
                    var apis = await response.Content.ReadFromJsonAsync<List<string>>();
                    Console.WriteLine("\nYou have access to the following endpoints:");
                    foreach (var api in apis!)
                        Console.WriteLine($"- {api}");
                }
                else
                {
                    Console.WriteLine("Could not fetch accessible APIs.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching API list: " + ex.Message);
            }
        }

    }
}
