using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static NewsNotifierClient.Models.AuthModels;

namespace NewsNotifierClient.Services
{
    public class AuthService
    {
            private readonly HttpClient _client;
            private readonly string _baseUrl;

            public string JwtToken { get; private set; } = string.Empty;

            public AuthService(string baseUrl)
            {
                _baseUrl = baseUrl.TrimEnd('/');
                _client = new HttpClient();
            }

            public async Task SignupAsync(SignupRequest request)
            {
                var response = await _client.PostAsJsonAsync($"{_baseUrl}/auth/signup", request);

                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Signup successful!");
                else
                    Console.WriteLine($"Signup failed: {await response.Content.ReadAsStringAsync()}");
            }

            public async Task LoginAsync(LoginRequest request)
            {
                var response = await _client.PostAsJsonAsync($"{_baseUrl}/auth/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    JwtToken = loginResponse?.Token ?? "";
                    Console.WriteLine("Login successful!");
                    Console.WriteLine("JWT Token:\n" + JwtToken);
                }
                else
                {
                    Console.WriteLine($"Login failed: {await response.Content.ReadAsStringAsync()}");
                }
            }
    }
}
