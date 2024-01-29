namespace TWBD_Presentation.Services;
internal class MenuService
{
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
        StartMenu();
    }
    
    /// <summary>
    /// Alla menyer och vyer nedan
    /// </summary>
    public void StartMenu()
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
                    LoginMenu();
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
    public void LoginMenu()
    {
        Header("LOGIN");

        Write("Enter your email address: ");
        var emailEntry = Console.ReadLine();
        while (emailEntry == "") { emailEntry = Console.ReadLine(); }

        Write("Enter your passwrod: ");
        var passwordEntry = Console.ReadLine();
        while (passwordEntry == "") { passwordEntry = Console.ReadLine(); }
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
}
