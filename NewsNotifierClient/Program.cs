//using NewsNotifierClient.Services;
using static NewsNotifierClient.Models.AuthModels;

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var baseUrl = "https://localhost:7050/api";
//        var authService = new AuthService(baseUrl);

//        while (true)
//        {
//            Console.Clear();
//            Console.WriteLine("\nAUTH MENU");
//            Console.WriteLine("1. Signup");
//            Console.WriteLine("2. Login");
//            Console.WriteLine("3. Exit");
//            Console.Write("Choose: ");
//            var input = Console.ReadLine();

//            if (input == "1")
//            {
//                Console.Write("Username: ");
//                var username = Console.ReadLine()!;
//                Console.Write("Email: ");
//                var email = Console.ReadLine()!;
//                Console.Write("Password: ");
//                var password = Console.ReadLine()!;

//                await authService.SignupAsync(new SignupRequest
//                {
//                    Username = username,
//                    Email = email,
//                    Password = password
//                });
//            }
//            else if (input == "2")
//            {
//                Console.Write("Email: ");
//                var email = Console.ReadLine()!;
//                Console.Write("Password: ");
//                var password = Console.ReadLine()!;

//                var loginSuccess = await authService.LoginAsync(new LoginRequest
//                {
//                    Email = email,
//                    Password = password
//                });

//                if (loginSuccess)
//                {
//                    //var adminService = new AdminService(authService.Client, baseUrl);
//                    //await AdminMenu(adminService);
//                    if (authService.Role == "Admin")
//                    {
//                        var adminService = new AdminService(authService.Client, baseUrl);
//                        await AdminMenu(adminService);
//                    }
//                    else if (authService.Role == "User")
//                    {
//                        Console.WriteLine("\nUser login detected. User menu is not yet implemented.");
//                        Console.WriteLine("Press Enter to continue...");
//                        Console.ReadLine();
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("\nLogin failed. Press Enter to continue...");
//                    Console.ReadLine();
//                }
//            }
//        }

//        static async Task AdminMenu(AdminService adminService)
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.WriteLine("\nADMIN MENU");
//                Console.WriteLine("1. Get Server Summaries");
//                Console.WriteLine("2. Get Servers");
//                Console.WriteLine("3. Get All Categories");
//                Console.WriteLine("4. Add Category");
//                Console.WriteLine("5. Logout");
//                Console.Write("Choose: ");
//                var input = Console.ReadLine();

//                if (input == "1")
//                {
//                    await adminService.GetServerSummariesAsync();
//                }
//                else if (input == "2")
//                {
//                    await adminService.GetServersAsync();
//                }
//                else if (input == "3")
//                {
//                    await adminService.GetAllCategoriesAsync();
//                }
//                else if (input == "4")
//                {
//                    Console.Write("Enter category name: ");
//                    var categoryName = Console.ReadLine()!;
//                    await adminService.AddCategoryAsync(categoryName);
//                }
//                else if (input == "5")
//                {
//                    Console.WriteLine("Logged out.");
//                    break;
//                }
//                else
//                {
//                    Console.WriteLine("Invalid input");
//                }
//                Console.WriteLine("\nPress Enter to return to Admin Menu...");
//                Console.ReadLine();
//            }
//        }
//    }
//}





using NewsNotifierClient.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var baseUrl = "https://localhost:7050/api";
        var authService = new AuthService(baseUrl);

        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("\nAUTH MENU");
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

                var loginSuccess = await authService.LoginAsync(new LoginRequest
                {
                    Email = email,
                    Password = password
                });

                if (loginSuccess)
                {
                    if (authService.Role == "Admin")
                    {
                        var adminService = new AdminService(authService.Client, baseUrl);
                        await AdminMenu(adminService);
                    }
                    else if (authService.Role == "User")
                    {
                        var userService = new UserService(authService.Client, baseUrl);
                        await userService.ShowUserMenuAsync(); 
                    }
                }
                else
                {
                    Console.WriteLine("\nLogin failed. Press Enter to continue...");
                    Console.ReadLine();
                }
            }
            else if (input == "3")
            {
                Console.WriteLine("\nExiting application...");
                isRunning = false; // This will exit the Auth Menu loop
            }
            else
            {
                Console.WriteLine("Invalid input. Press Enter to continue...");
                Console.ReadLine();
            }
        }

        // ✅ Hold screen after exit
        Console.WriteLine("\nApplication exited. Press Enter to close...");
        Console.ReadLine();
    }

    static async Task AdminMenu(AdminService adminService)
    {
        bool isAdminMenuActive = true;

        while (isAdminMenuActive)
        {
            Console.Clear();
            Console.WriteLine("\nADMIN MENU");
            Console.WriteLine("1. Get Server Summaries");
            Console.WriteLine("2. Get Servers");
            Console.WriteLine("3. Get All Categories");
            Console.WriteLine("4. Add Category");
            Console.WriteLine("5. Logout");
            Console.Write("Choose: ");
            var input = Console.ReadLine();

            if (input == "1")
            {
                await adminService.GetServerSummariesAsync();
            }
            else if (input == "2")
            {
                await adminService.GetServersAsync();
            }
            else if (input == "3")
            {
                await adminService.GetAllCategoriesAsync();
            }
            else if (input == "4")
            {
                Console.Write("Enter category name: ");
                var categoryName = Console.ReadLine()!;
                await adminService.AddCategoryAsync(categoryName);
            }
            else if (input == "5")
            {
                Console.WriteLine("\nLogged out. Press Enter to return to Auth Menu...");
                Console.ReadLine();
                isAdminMenuActive = false; // ✅ Exits Admin Menu and returns to Auth Menu
            }
            else
            {
                Console.WriteLine("Invalid input");
            }

            if (isAdminMenuActive)
            {
                Console.WriteLine("\nPress Enter to return to Admin Menu...");
                Console.ReadLine();
            }
        }
    }


}



