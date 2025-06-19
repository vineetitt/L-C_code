using NewsNotifierClient.Services;
using static NewsNotifierClient.Models.AuthModels;

class Program
{
    static async Task Main(string[] args)
    {
        var baseUrl = "https://localhost:7050/api"; 
        var authService = new AuthService(baseUrl);

        while (true)
        {
            Console.WriteLine("AUTH MENU");
            Console.WriteLine("1. Signup");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Choose: ");
            var input = Console.ReadLine();

            if (input == "1")
            {
                Console.Write("Username: ");
                var username = Console.ReadLine()!;
                Console.Write("Email: ");
                var email = Console.ReadLine()!;
                Console.Write("Password: ");
                var password = Console.ReadLine()!;

                await authService.SignupAsync(new SignupRequest
                {
                    Username = username,
                    Email = email,
                    Password = password
                });
            }
            else if (input == "2")
            {
                Console.Write("Email: ");
                var email = Console.ReadLine()!;
                Console.Write("Password: ");
                var password = Console.ReadLine()!;

                await authService.LoginAsync(new LoginRequest
                {
                    Email = email,
                    Password = password
                });
            }
            else if (input == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("❗ Invalid input");
            }
        }
    }
}
