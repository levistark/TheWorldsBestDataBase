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
    public void DashedLine()
    {
        Console.WriteLine("---------------------");
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
                    await SignupMenu();
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

        Write("Enter your password: ");
        var passwordEntry = Console.ReadLine();
        while (passwordEntry == "") { passwordEntry = Console.ReadLine(); }

        var validLogin = await LoginHandler(emailEntry!, passwordEntry!);

        if (validLogin)
        {
            Console.Clear();
            await UserDashBoard(emailEntry!);
            Console.WriteLine("loading...");
            Thread.Sleep(300);
        }
            
        else
        {
            Blank();
            WriteLine("Login failed. Please try again or create a new user account.");
            PressAnyKey();
            await StartMenu();
        }
    }
    public async Task SignupMenu()
    {
        Header("CREATE ACCOUNT");

        WriteLine("Enter your information below:");
        Blank();

        Write("First name: ");
        var firstNameEntry = Console.ReadLine();
        while (firstNameEntry == "")
        {
            WriteLine("No null values allowed.");
            firstNameEntry = Console.ReadLine(); 
        }
        
        Write("Last name: ");
        var lastNameEntry = Console.ReadLine();
        while (lastNameEntry == "")
        {
            WriteLine("No null values allowed.");
            lastNameEntry = Console.ReadLine(); 
        }

        Write("Email: ");
        var emailEntry = Console.ReadLine();
        while (emailEntry == "")
        {
            WriteLine("No null values allowed.");
            emailEntry = Console.ReadLine(); 
        }

        var emailValidationResult = await _userService.ValidateEmail(emailEntry!);

        while (!emailValidationResult.Success)
        {
            if (emailValidationResult.Code == TWBD_Domain.DTOs.Enums.ServiceCode.ALREADY_EXISTS)
            {
                WriteLine("This email address is already registered with us. Please enter another email, or login with your existing email.");
            }
            else
            {
                WriteLine("The email does not meet the requirements of a valid email address. Please enter a valid email.");
            }
            emailEntry = Console.ReadLine();
            emailValidationResult = await _userService.ValidateEmail(emailEntry!);
        }

        Write("Phone number: ");
        var phoneEntry = Console.ReadLine();
        while (phoneEntry == "")
        {
            WriteLine("No null values allowed.");
            phoneEntry = Console.ReadLine(); 
        }

        Write("City: ");
        var cityEntry = Console.ReadLine();
        while (cityEntry == "")
        {
            WriteLine("No null values allowed.");
            cityEntry = Console.ReadLine(); 
        }

        Write("Street Name: ");
        var streetEntry = Console.ReadLine();
        while (streetEntry == "")
        {
            WriteLine("No null values allowed.");
            streetEntry = Console.ReadLine(); 
        }

        Write("Postal code: ");
        var postalCodeEntry = Console.ReadLine();
        while (postalCodeEntry == "")
        {
            WriteLine("No null values allowed.");
            postalCodeEntry = Console.ReadLine();
        }


        Write("Password: ");
        var passwordEntry = Console.ReadLine();
        while (passwordEntry == "") 
        {
            WriteLine("No null values allowed.");
            passwordEntry = Console.ReadLine(); 
        }
        var passwordValidationResult = _userService.ValidatePassword(passwordEntry!);

        while (!passwordValidationResult.Success)
        {
            WriteLine("The email does not meet the requirements of a secure password. Please try again.");
            passwordEntry = Console.ReadLine();
            passwordValidationResult = _userService.ValidatePassword(passwordEntry!);
        }

        Write("Confirm password: ");
        var passwordConfirmEntry = Console.ReadLine();
        while (passwordConfirmEntry != passwordEntry)
        {
            WriteLine("Passwords do not match, try again");
            passwordConfirmEntry = Console.ReadLine();
        }

        Write("User role: ");
        var roleEntry = Console.ReadLine();
        while (roleEntry == "")
        {
            WriteLine("No null values allowed.");
            roleEntry = Console.ReadLine();
        }

        var registrationResult = await _userService.RegisterUser(new UserRegistrationModel()
        {
            FirstName = firstNameEntry!,
            LastName = lastNameEntry!,
            Email = emailEntry!,
            PhoneNumber = phoneEntry!,
            Password = passwordEntry!,
            PasswordConfirm = passwordConfirmEntry!,
            City = cityEntry!,
            StreetName = streetEntry!,
            PostalCode = postalCodeEntry!,
            Role = roleEntry!,
        });

        if (registrationResult.ReturnObject is UserProfileModel profile)
        {
            Console.Clear();
            WriteLine("User was successfully registered. Redirecting to user dashboard");
            Thread.Sleep(1000);
            await UserDashBoard(profile.Email);
        }
        else
        {
            Console.Clear();
            WriteLine("User could not be registered due to an internal error. Please try again");
            Thread.Sleep(1000);
            await SignupMenu();
        }
    }
    public async Task UserDashBoard(string email)
    {
        Header("USER ADMIN DASHBOARD");

        var userProfile = await _userService.GetUserProfileByEmail(email);

        WriteLine("1. All Users ");
        WriteLine("2. My profile");
        WriteLine("3. Log out");

        UserProfileModel? myProfile = userProfile.ReturnObject as UserProfileModel;

        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "1":
                    await AllUsers(email);
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
    public async Task AllUsers(string email)
    {
        Header("ALL USERS");
        
        var listResult = await _userService.GetAllUserProfiles();

        WriteLine("To view and edit user details, enter in a specific user id. To go back, enter in 'q'.");

        if (listResult.ReturnObject is List<UserProfileModel> users)
        {
            foreach (UserProfileModel user in users)
            {
                DashedLine();
                WriteLine($"User Id: {user.UserId}");
                WriteLine($"User First Name: {user.FirstName}");
                WriteLine($"User Last Name: {user.LastName}");
                WriteLine($"User Email: {user.Email}");
                WriteLine($"User Role: {user.Role}");
                WriteLine($"User City: {user.City}");
                Blank();
            }
        }

        var option = Console.ReadLine();

        while (true)
        {
            if (option == "q")
            {
                await UserDashBoard(email);
            }
            else if (int.TryParse(option, out int id))
            {
                var searchResult = await _userService.GetUserProfileById(id);

                if (searchResult.ReturnObject is UserProfileModel userProfile)
                {
                    await UserProfileDetails(id);
                    return;
                }
                else
                {
                    WriteLine("No user found with given Id. Please try again");
                    option = Console.ReadLine();
                }
            }
            else
            {
                WriteLine("Invalid option. Please try again");
                option = Console.ReadLine();
            }
        }
    }
    public async Task UserProfileDetails(int id)
    {
        Header("User Profile");

        var result = await _userService.GetUserProfileById(id);

        if (result.ReturnObject is UserProfileModel)
        {
            UserProfileModel? profile = result.ReturnObject as UserProfileModel;

            Write($"User Id: {profile!.UserId}");
            Write($"First Name: {profile!.FirstName}");
            Write($"Last Name: {profile!.LastName}");
            Write($"Email: {profile!.Email}");
            Write($"Role: {profile!.Role}");
            Write($"Phone Number: {profile!.PhoneNumber}");
            Blank();

            WriteLine("Address Info");
            Write($"City: {profile!.City}");
            Write($"Street Name: {profile!.StreetName}");
            Write($"Postal Code: {profile!.PostalCode}");
            Blank();

            WriteLine("Press 1 to edit info, press 2 to go back to all users, or press 3 to delete this user.");
            var option = Console.ReadLine();

            while (true)
            {
                switch (option)
                {
                    case "1":
                        await EditProfile(id, 2);
                        return;
                    case "2":
                        await AllUsers(profile!.Email);
                        return;
                    case "3":
                        await DeleteUser(id, profile!.Email);
                        return;
                    default:
                        option = Console.ReadLine();
                        break;
                }
            }
        }
    }
    public async Task DeleteUser(int id, string email)
    {
        WriteLine("Are you sure you want to delete this user? (y/n)");
        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "y":
                    var result = await _userService.DeleteUserProfileById(id);
                    
                    if (result.Code == TWBD_Domain.DTOs.Enums.ServiceCode.DELETED)
                    {
                        Console.Clear();
                        WriteLine("User was successfully removed.");
                        Thread.Sleep(1000);
                        await AllUsers(email);
                    }
                    else
                    {
                        Console.Clear();
                        WriteLine("User could not be removed due to an internal error.");
                        Thread.Sleep(1000);
                        await AllUsers(email);
                    }
                    return;

                case "n":
                    await UserProfileDetails(id);
                    return;
                default:
                    option = Console.ReadLine();
                    break;
            }
        }

    }
    public async Task MyProfile(int id)
    {
        Header("My Profile");

        var result = await _userService.GetUserProfileById(id);

        if (result.ReturnObject is UserProfileModel)
        {
            UserProfileModel? profile = result.ReturnObject as UserProfileModel;

            WriteLine($"User Id: {profile!.UserId}");
            WriteLine($"First Name: {profile!.FirstName}");
            WriteLine($"Last Name: {profile!.LastName}");
            WriteLine($"Email: {profile!.Email}");
            WriteLine($"Role: {profile!.Role}");
            WriteLine($"Phone Number: {profile!.PhoneNumber}");
            Blank();
            
            WriteLine("Address Info");
            WriteLine($"City: {profile!.City}");
            WriteLine($"Street Name: {profile!.StreetName}");
            WriteLine($"Postal Code: {profile!.PostalCode}");
            Blank();

            WriteLine("Press 1 to edit info, or press 2 to go back to dashboard.");
            var option = Console.ReadLine();

            while (true)
            {
                switch (option)
                {
                    case "1":
                        await EditProfile(id, 1);
                        return;
                    case "2":
                        await UserDashBoard(profile!.Email);
                        return;
                    default:
                        option = Console.ReadLine();
                        break;
                }
            }
        }
    }
    public async Task EditProfile(int id, int pageIndex)
    {
        Header("Edit Profile");

        var result = await _userService.GetUserProfileById(id);

        if (result.ReturnObject is UserProfileModel)
        {
            UserProfileModel? profile = result.ReturnObject as UserProfileModel;

            Write($"1: User Id: {profile!.UserId}");
            Write($"2: First Name: {profile!.FirstName}");
            Write($"3: Last Name: {profile!.LastName}");
            Write($"4: Email: {profile!.Email}");
            Write($"5: Role: {profile!.Role}");
            Write($"6: Phone Number: {profile!.PhoneNumber}");
            Blank();

            WriteLine("Address Info");
            Write($"7: City: {profile!.City}");
            Write($"8: Street Name: {profile!.StreetName}");
            Write($"9: Postal Code: {profile!.PostalCode}");
            Blank();

            WriteLine("Enter the desired field number you wish to edit, or press 'q' to go back to your profile.");
            var option = Console.ReadLine();

            while (true)
            {
                switch (option)
                {
                    case "q":
                        await MyProfile(id);
                        return;
                    case "1":
                        WriteLine("You can't edit your user id. Please enter in another field number");
                        option = Console.ReadLine();
                        break;
                    case "2":
                        await EditProfileField(2, id, pageIndex);
                        return;
                    case "3":
                        await EditProfileField(3, id, pageIndex);
                        return;
                    case "4":
                        WriteLine("You can't edit your emaila address. To change email address, delete your user profile and create a new user account with your new email");
                        option = Console.ReadLine();
                        break;
                    case "5":
                        await EditProfileField(5, id, pageIndex);
                        return;
                    case "6":
                        await EditProfileField(6, id, pageIndex);
                        return;
                    case "7":
                        await EditProfileField(7, id, pageIndex);
                        return;
                    case "8":
                        await EditProfileField(8, id, pageIndex);
                        return;
                    case "9":
                        await EditProfileField(9, id, pageIndex);
                        return;
                    default:
                        WriteLine("Invalid value, please enter in a valid field number");
                        option = Console.ReadLine();
                        break;
                }
            }
        }
    }
    public async Task EditProfileField(int fieldNumber, int id, int pageIndex)
    {
        Console.Clear();
        var result = await _userService.GetUserProfileById(id);
        UserProfileModel? existingProfile = result.ReturnObject as UserProfileModel;

        Write("Enter in new value for chosen field: ");
        var newValue = Console.ReadLine();

        while (newValue == "")
        {
            WriteLine("Value can't be empty, please enter in a value");
            newValue = Console.ReadLine();
        }

        switch (fieldNumber)
        {
            case 2:
                existingProfile!.FirstName = newValue!;
                return;
            case 3:
                existingProfile!.LastName = newValue!;
                return;
            case 5:
                existingProfile!.Role = newValue!;
                return;
            case 6:
                existingProfile!.PhoneNumber = newValue!;
                return;
            case 7:
                existingProfile!.City = newValue!;
                return;
            case 8:
                existingProfile!.StreetName = newValue!;
                return;
            case 9:
                existingProfile!.PostalCode = newValue!;
                return;
        }

        var updateResult = await _userService.UpdateUserProfile(existingProfile!);

        if (updateResult.Code == TWBD_Domain.DTOs.Enums.ServiceCode.UPDATED)
        {
            Console.Clear();
            WriteLine("User profile was successfully updated.");
            Thread.Sleep(1000);
        }
        else
        {
            Console.Clear();
            WriteLine("Property was not updated due to an internal error. Please try again");
            Thread.Sleep(1000);
        }

        if (pageIndex == 1)
            await MyProfile(id);

        if (pageIndex == 2)
            await UserProfileDetails(id);
    }

}
