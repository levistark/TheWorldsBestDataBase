using TWBD_Domain.DTOs.Models;
using TWBD_Domain.Services;

namespace TWBD_Presentation.Services;
internal class MenuService
{
    private readonly UserService _userService;
    private readonly UserLoginService _loginService;

    public MenuService(UserService userService, UserLoginService loginService)
    {
        _userService = userService;
        _loginService = loginService;
    }

    /// <summary>
    /// Nedan följer metoder som är till för att "förkorta" vissa Console.-metoder för att göra koden lite mer clean. 
    /// </summary>
    public void WriteLine(string text) { Console.WriteLine(text); }
    public void Write(string text) { Console.Write(text); }
    public void Blank() { Console.WriteLine(); }
    public void Header(string text)
    {
        Console.Clear();
        WriteLine($"## {text.ToUpper()} ##");
        Blank();
    }
    public void PressAnyKey()
    {
        Blank();
        WriteLine("Press any key to continue.");
        Console.ReadKey();
    }
    
    /// <summary>
    /// Alla menyer och vyer nedan
    /// </summary>
    public async Task StartMenu()
    {
        Header("LOGIN / CREATE ACCOUNT");

        WriteLine("1. Login ");
        WriteLine("2. Create account");
        WriteLine("3. Quit program");

        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "1":
                    await LoginMenu();
                    return;
                case "2":
                    SignupMenu();
                    return;
                case "3":
                    return;
                default:
                    option = Console.ReadLine();
                    break;
            }
        }
    }

    private async Task<bool> LoginHandler(string email, string password)
    {
        var result = await _loginService.LoginValidation(new LoginModel(email, password));
        return result.Success;
    }

    public async Task LoginMenu()
    {
        Header("LOGIN");

        Write("Enter your email address: ");
        var emailEntry = Console.ReadLine();
        while (emailEntry == "") { emailEntry = Console.ReadLine(); }

        Write("Enter your passwrod: ");
        var passwordEntry = Console.ReadLine();
        while (passwordEntry == "") { passwordEntry = Console.ReadLine(); }

        var validLogin = await LoginHandler(emailEntry!, passwordEntry!);

        if (validLogin)
            await UserDashBoard(emailEntry!);
        else
        {
            Blank();
            WriteLine("Login failed. Please try again or create a new user account.");
            PressAnyKey();
            await StartMenu();
        }
    }
    public void SignupMenu()
    {
        Header("CREATE ACCOUNT");

        WriteLine("Enter your information below:");
        Blank();

        Write("First name: ");
        var firstNameEntry = Console.ReadLine();
        while (firstNameEntry == "") { firstNameEntry = Console.ReadLine(); }
        
        Write("Last name: ");
        var lastNameEntry = Console.ReadLine();
        while (lastNameEntry == "") { lastNameEntry = Console.ReadLine(); }

        Write("Email: ");
        var emailEntry = Console.ReadLine();
        while (emailEntry == "") { emailEntry = Console.ReadLine(); }

        Write("Country: ");
        var countryEntry = Console.ReadLine();
        while (countryEntry == "") { countryEntry = Console.ReadLine(); }

        Write("City: ");
        var cityEntry = Console.ReadLine();
        while (cityEntry == "") { cityEntry = Console.ReadLine(); }

        Write("Street Name: ");
        var streetEntry = Console.ReadLine();
        while (streetEntry == "") { streetEntry = Console.ReadLine(); }

        Write("Phone number: ");
        var phoneEntry = Console.ReadLine();
        while (phoneEntry == "") { phoneEntry = Console.ReadLine(); }

        Write("Password: ");
        var passwordEntry = Console.ReadLine();
        while (passwordEntry == "") { passwordEntry = Console.ReadLine(); }

        Write("Confirm password: ");
        var passwordConfirmEntry = Console.ReadLine();
        while (passwordConfirmEntry != passwordEntry)
        {
            WriteLine("Passwords do not match, try again");
            passwordConfirmEntry = Console.ReadLine();
        }
    }

    public async Task UserDashBoard(string email)
    {
        Header("USER DASHBOARD");

        WriteLine("1. All Users ");
        WriteLine("2. My profile");
        WriteLine("3. Log out");

        // 1. Hämta upp korrekt profilinformation baserat på email
        var userProfile = await _userService.GetUserProfileByEmail(email);
        UserProfileModel? myProfile = userProfile.ReturnObject as UserProfileModel;

        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "1":
                    await LoginMenu();
                    return;
                case "2":
                    await MyProfile(myProfile!.UserId);
                    return;
                case "3":
                    await StartMenu();
                    return;
                default:
                    option = Console.ReadLine();
                    break;
            }
        }
    }

    public async Task MyProfile(int id)
    {

    }
}
