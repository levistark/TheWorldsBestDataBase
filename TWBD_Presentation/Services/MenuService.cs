using TWBD_Domain.DTOs.Models;
using TWBD_Domain.DTOs.Enums;
using TWBD_Domain.Services;
using TWBD_Domain.Services.ProductServices;
using TWBD_Domain.DTOs.Models.Product;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TWBD_Presentation.Services;
internal class MenuService
{
    private readonly UserService _userService;
    private readonly UserLoginService _loginService;
    private readonly ProductService _productService;
    private readonly DescriptionService _descriptionService;
    private readonly ReviewService _reviewService;
    private readonly ProductValidationService _productValidationService;

    public MenuService(UserService userService, UserLoginService loginService, ProductService productService, DescriptionService descriptionService, ReviewService reviewService, ProductValidationService productValidationService)
    {
        _userService = userService;
        _loginService = loginService;
        _productService = productService;
        _descriptionService = descriptionService;
        _reviewService = reviewService;
        _productValidationService = productValidationService;
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

    // ------------------- USER MENUS --------------------------- //

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

        WriteLine("Enter your login credentials below, or enter in 'q' to go back");
        Blank();

        Write("Enter your email address: ");
        var emailEntry = Console.ReadLine();
        while (emailEntry == "") { emailEntry = Console.ReadLine(); }

        if (emailEntry == "q")
            await StartMenu();

        Write("Enter your password: ");
        var passwordEntry = Console.ReadLine();
        while (passwordEntry == "") { passwordEntry = Console.ReadLine(); }

        if (passwordEntry == "q")
        {
            await StartMenu();
            return;
        } 
        else
        {
            if (await LoginHandler(emailEntry!, passwordEntry!))
            {
                await UserDashBoard(emailEntry!);
            }
            else
            {
                Blank();
                WriteLine("Login failed. Please try again or create a new user account.");
                PressAnyKey();
                await StartMenu();
            }
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
        if (firstNameEntry == "q")
            await StartMenu();
        
        Write("Last name: ");
        var lastNameEntry = Console.ReadLine();
        while (lastNameEntry == "")
        {
            WriteLine("No null values allowed.");
            lastNameEntry = Console.ReadLine(); 
        }
        if (lastNameEntry == "q")
            await StartMenu();

        Write("Email: ");
        var emailEntry = Console.ReadLine();
        while (emailEntry == "")
        {
            WriteLine("No null values allowed.");
            emailEntry = Console.ReadLine(); 
        }
        if (emailEntry == "q")
            await StartMenu();

        var emailValidationResult = await _userService.ValidateEmail(emailEntry!);

        while (!emailValidationResult.Success)
        {
            if (emailEntry == "q")
                await StartMenu();

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
        if (phoneEntry == "q")
            await StartMenu();

        Write("City: ");
        var cityEntry = Console.ReadLine();
        while (cityEntry == "")
        {
            WriteLine("No null values allowed.");
            cityEntry = Console.ReadLine(); 
        }
        if (cityEntry == "q")
            await StartMenu();

        Write("Street Name: ");
        var streetEntry = Console.ReadLine();
        while (streetEntry == "")
        {
            WriteLine("No null values allowed.");
            streetEntry = Console.ReadLine(); 
        }
        if (streetEntry == "q")
            await StartMenu();

        Write("Postal code: ");
        var postalCodeEntry = Console.ReadLine();
        while (postalCodeEntry == "")
        {
            WriteLine("No null values allowed.");
            postalCodeEntry = Console.ReadLine();
        }
        if (postalCodeEntry == "q")
            await StartMenu();


        Write("Password: ");
        var passwordEntry = Console.ReadLine();
        while (passwordEntry == "") 
        {
            WriteLine("No null values allowed.");
            passwordEntry = Console.ReadLine(); 
        }
        if (passwordEntry == "q")
            await StartMenu();

        var passwordValidationResult = _userService.ValidatePassword(passwordEntry!);

        while (!passwordValidationResult.Success)
        {
            WriteLine("The passwrd does not meet the requirements (Example of a valid password: Bytmig123!). Please try again.");
            passwordEntry = Console.ReadLine();
            passwordValidationResult = _userService.ValidatePassword(passwordEntry!);
        }

        Write("Confirm password: ");
        var passwordConfirmEntry = Console.ReadLine();

        if (passwordConfirmEntry == "q")
            await StartMenu();

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
        if (roleEntry == "q")
            await StartMenu();

        var registrationResult = await _userService.RegisterUser(new UserRegistrationModel()
        {
            FirstName = firstNameEntry!,
            LastName = lastNameEntry!,
            Email = emailEntry!,
            PhoneNumber = phoneEntry,
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
            Thread.Sleep(2000);
            await UserDashBoard(profile.Email);
        }
        else
        {
            Console.Clear();
            WriteLine("User could not be registered due to an internal error. Please try again");
            Thread.Sleep(2000);
            await SignupMenu();
        }
    }
    public async Task UserDashBoard(string email)
    {
        Header("ADMIN DASHBOARD");

        var userProfile = await _userService.GetUserProfileByEmail(email);

        if (userProfile.ReturnObject is UserProfileModel myProfile)
        {

            WriteLine("1. Users");
            WriteLine("2. Products");
            WriteLine("3. My profile");
            WriteLine("4. Log out");
            var option = Console.ReadLine();

            while (true)
            {
                switch (option)
                {
                    case "1":
                        await AllUsers(email);
                        return;
                    case "2":
                        await AllProducts(email);
                        return;
                    case "3":
                        await MyProfile(myProfile!.UserId);
                        return;
                    case "4":
                        await StartMenu();
                        return;
                    default:
                        option = Console.ReadLine();
                        break;
                }
            }
        }
        else
        {
            WriteLine("User does either not have a profile or registered authentication credentials. Please create a new user account.");
            PressAnyKey();
            await StartMenu();
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
                    
                    if (result.Code == ServiceCode.DELETED)
                    {
                        Console.Clear();
                        WriteLine("User was successfully removed.");
                        Thread.Sleep(2000);
                        await AllUsers(email);
                    }
                    else
                    {
                        Console.Clear();
                        WriteLine("User could not be removed due to an internal error.");
                        Thread.Sleep(2000);
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

            WriteLine($"1: User Id: {profile!.UserId}");
            WriteLine($"2: First Name: {profile!.FirstName}");
            WriteLine($"3: Last Name: {profile!.LastName}");
            WriteLine($"4: Email: {profile!.Email}");
            WriteLine($"5: Role: {profile!.Role}");
            WriteLine($"6: Phone Number: {profile!.PhoneNumber}");
            Blank();

            WriteLine("Address Info");
            WriteLine($"7: City: {profile!.City}");
            WriteLine($"8: Street Name: {profile!.StreetName}");
            WriteLine($"9: Postal Code: {profile!.PostalCode}");
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
                        WriteLine("You can't edit email address. To change email address, delete your user profile and create a new user account with your new email");
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
                break;
            case 3:
                existingProfile!.LastName = newValue!;
                break;
            case 5:
                existingProfile!.Role = newValue!;
                break;
            case 6:
                existingProfile!.PhoneNumber = newValue!;
                break;
            case 7:
                existingProfile!.City = newValue!;
                break;
            case 8:
                existingProfile!.StreetName = newValue!;
                break;
            case 9:
                existingProfile!.PostalCode = newValue!;
                break;
        }

        var updateResult = await _userService.UpdateUserProfile(existingProfile!);

        if (updateResult.Code == TWBD_Domain.DTOs.Enums.ServiceCode.UPDATED)
        {
            Console.Clear();
            WriteLine("User profile was successfully updated.");
            Thread.Sleep(2000);

            if (pageIndex == 1)
                await MyProfile(id);

            if (pageIndex == 2)
                await UserProfileDetails(id);
        }
        else
        {
            Console.Clear();
            WriteLine("Property was not updated due to an internal error. Please try again");
            Thread.Sleep(2000);

            if (pageIndex == 1)
                await MyProfile(id);

            if (pageIndex == 2)
                await UserProfileDetails(id);
        }
    }


    // ------------------- PRODUCT MENUS --------------------------- //

    public async Task AllProducts(string email)
    {
        Header("PRODUCTS");
        var productList = await _productService.GetAllProducts();

        if (productList is List<ProductModel> products)
        {
            // If product list is empty
            if (products.Count() == 0)
            {
                WriteLine("No existing products. To add a new product press 1, or press 2 to go back to the dashboard");
                var noProductsOption = Console.ReadLine();

                while (true)
                {
                    switch (noProductsOption)
                    {
                        case "2":
                            await UserDashBoard(email);
                            return;
                        case "1":
                            await AddProduct(email);
                            return;
                        default:
                            WriteLine("Invalid option, please enter either 1, or 2");
                            noProductsOption = Console.ReadLine();
                            break;
                    }
                }
            }
            // If product list is not empty
            else
            {
                WriteLine("Enter 1 to add new product");
                WriteLine("Enter 2 to view product details");
                WriteLine("Enter 3 to go back to user dashboard");

                foreach (ProductModel product in products)
                {
                    Blank();
                    DashedLine();
                    WriteLine($"Article number: {product.ArticleNumber}");
                    WriteLine($"Title: {product.Title}");
                    WriteLine($"Manufacturer: {product.Manufacturer}");
                    WriteLine($"Price : {product.Price}");
                    WriteLine($"Discount price: {product.DiscountPrice}");
                    Blank();
                }

                var option = Console.ReadLine();

                while (true)
                {
                    switch (option)
                    {
                        case "1":
                            await AddProduct(email);
                            return;
                        case "2":
                            await ChooseProductDetails(email);
                            return;
                        case "3":
                            await UserDashBoard(email);
                            return;
                        default:
                            WriteLine("Invalid option, please enter either 1, 2, or 3");
                            option = Console.ReadLine();
                            break;
                    }
                }
            }
        }
    }
    public async Task AddProduct(string email)
    {
        Header("ADD NEW PRODUCT");
        WriteLine("To register a new product, follow the steps below. To go back, enter in 'q'");
        Blank();

        Write("Articlenumber: ");
        var articleNumberEntry = Console.ReadLine();
        bool isValid = false;

        while (isValid == false)
        {
            while (articleNumberEntry == "")
            {
                WriteLine("This property can't be empty, please enter a valid value");
                articleNumberEntry = Console.ReadLine();
            }
            if (articleNumberEntry == "q")
            {
                await AllProducts(email);
                return;
            }
            while (!await _productValidationService.ValidateArticleNumber(articleNumberEntry!))
            {
                WriteLine("This article number already exist in the database. Please enter another article number");
                articleNumberEntry = Console.ReadLine();
            }
            if (await _productValidationService.ValidateArticleNumber(articleNumberEntry!))
            {
                isValid = true;
            }
        }

        Write("Product title: ");
        var titleEntry = Console.ReadLine();
        while (titleEntry == "")
        {
            WriteLine("This property can't be empty, please enter a valid value");
            titleEntry = Console.ReadLine();
        }

        Write("Manufacturer name: ");
        var manufacturerEntry = Console.ReadLine();
        while (manufacturerEntry == "")
        {
            WriteLine("This property can't be empty, please enter a valid value");
            manufacturerEntry = Console.ReadLine();
        }

        Write("Product description: ");
        var descriptionEntry = Console.ReadLine();
        while (descriptionEntry == "")
        {
            WriteLine("This property can't be empty, please enter a valid value");
            descriptionEntry = Console.ReadLine();
        }

        Write("Product description language: ");
        var languageEntry = Console.ReadLine();
        while (languageEntry == "")
        {
            WriteLine("This property can't be empty, please enter a valid value");
            languageEntry = Console.ReadLine();
        }

        Write("Product specification: ");
        var specificationEntry = Console.ReadLine();
        while (specificationEntry == "")
        {
            WriteLine("This property can't be empty, please enter a valid value");
            specificationEntry = Console.ReadLine();
        }


        Write("Price (only digits): ");
        var priceEntry = Console.ReadLine();
        while (priceEntry == "" || !decimal.TryParse(priceEntry, out decimal priceEntryDecimal))
        {
            WriteLine("Invalid value, please enter a valid value");
            priceEntry = Console.ReadLine();
        }

        Write("Product category: ");
        var categoryEntry = Console.ReadLine();
        while (categoryEntry == "" || decimal.TryParse(categoryEntry, out decimal categoryEntryDecimal))
        {
            WriteLine("Invalid value, please enter a valid value");
            categoryEntry = Console.ReadLine();
        }

        Write("Product parent category: ");
        var parentCategoryEntry = Console.ReadLine();
        while (decimal.TryParse(parentCategoryEntry, out decimal parentCategoryEntryDecimal))
        {
            WriteLine("Invalid value, please enter a valid value");
            parentCategoryEntry = Console.ReadLine();
        }

        if (decimal.TryParse(priceEntry, out decimal priceDecimal))
        {
            var result = await _productService.CreateProduct(new ProductRegistrationModel()
            {
                ArticleNumber = articleNumberEntry!,
                Title = titleEntry!,
                Manufacturer = manufacturerEntry!,
                Description = descriptionEntry!,
                DescriptionLanguage = languageEntry!,
                Specifications = specificationEntry!,
                Category = categoryEntry!,
                ParentCategory = parentCategoryEntry!,
                Price = priceDecimal!,
            });

            if (result is ProductModel)
            {
                Blank();
                WriteLine("Product successfully created");
                PressAnyKey();
                await AllProducts(email);
            }
            else
            {
                Blank();
                WriteLine("Product could not be created due to an internal error");
                PressAnyKey();
                await AllProducts(email);
            }
        }
    }
    public async Task ChooseProductDetails(string email)
    {
        Header("CHOOSE PRODUCT");
        var products = await _productService.GetAllProducts();

        WriteLine("Enter in the ArticleNumber of the product you wish to view/edit. To go back, enter in 'q'.");

        foreach (ProductModel product in products)
        {
            Blank();
            DashedLine();
            WriteLine($"Article number: {product.ArticleNumber}");
            WriteLine($"Title: {product.Title}");
            WriteLine($"Manufacturer: {product.Manufacturer}");
            WriteLine($"Price : {product.Price}");
            WriteLine($"Discount price: {product.DiscountPrice}");
            Blank();
        }

        var option = Console.ReadLine();

        while (true)
        {
            if (option == "q")
            {
                await AllProducts(email);
                return;
            }
            else if (option != "")
            {
                var existingProduct = await _productService.GetProductById(option!);

                if (existingProduct is ProductModel)
                {
                    await ProductDetails(existingProduct.ArticleNumber, email);
                    return;
                }
                else
                {
                    WriteLine("No product found due to incorrect article number, please enter in a correct article number, or enter 'q' to go back");
                    option = Console.ReadLine();
                }
            }
        }
    }
    public async Task ProductDetails(string articleNumber, string email)
    {
        Header("PRODUCT DETAILS");
        var product = await _productService.GetProductById(articleNumber);

        if (product != null)
        {
            WriteLine($"Article number: {product.ArticleNumber}");
            WriteLine($"Title: {product.Title}");
            WriteLine($"Manufacturer: {product.Manufacturer}");
            WriteLine($"Price: {product.Price}");
            WriteLine($"Discount price: {product.DiscountPrice}");
            Blank();
            WriteLine($"Description count: {product.Descriptions.Count}");
            WriteLine($"Review count: {product.Reviews.Count}");
            WriteLine($"Category: {product.Category.Category}");
            WriteLine($"Parent category: {product.Category.ParentCategory}");
            DashedLine();
            Blank();

            WriteLine("Enter 1 to edit product details");
            WriteLine("Enter 2 to view and edit descriptions");
            WriteLine("Enter 3 to view and edit reviews");
            WriteLine("Enter 4 to delete product.");
            WriteLine("Enter 5 to go back to all products.");

            var option = Console.ReadLine();

            while (true)
            {
                switch (option)
                {
                    case "1":
                        await EditProduct(articleNumber, email);
                        return;
                    case "2":
                        await AllDescriptions(articleNumber, email);
                        return;
                    case "3":
                        await AllReviews(articleNumber, email);
                        return;
                    case "4":
                        await DeleteProduct(articleNumber, email);
                        return;
                    case "5":
                        await AllProducts(email);
                        return;
                    default:
                        option = Console.ReadLine();
                        break;
                }
            }
        }
        else
        {
            Blank();
            WriteLine("No product found.");
            PressAnyKey();
            await AllProducts(email);
        }
    }
    public async Task EditProduct(string articleNumber, string email)
    {
        Header("EDIT PRODUCT DETAILS");
        var product = await _productService.GetProductById(articleNumber);

        if (product != null)
        {
            WriteLine($"[Article number]: {product.ArticleNumber}");
            WriteLine($"1. Title: {product.Title}");
            WriteLine($"2. Manufacturer: {product.Manufacturer}");
            WriteLine($"3. Price: {product.Price}");
            WriteLine($"4. Discount price: {product.DiscountPrice}");
            Blank();
            WriteLine($"5. Category: {product.Category.Category}");
            WriteLine($"6. Parent category: {product.Category.ParentCategory}");
            DashedLine();
            Blank();

            WriteLine("Enter the number of the field you wish to edit, or press 'q' to go  back");

            var option = Console.ReadLine();

            while (true)
            {
                switch (option)
                {
                    case "q":
                        await ProductDetails(articleNumber, email);
                        return;
                    case "1":
                        await EditProductField(1, articleNumber, email);
                        return;
                    case "2":
                        await EditProductField(2, articleNumber, email);
                        return;
                    case "3":
                        await EditProductField(3, articleNumber, email);
                        return;
                    case "4":
                        await EditProductField(4, articleNumber, email);
                        return;
                    case "5":
                        await EditProductField(5, articleNumber, email);
                        return;
                    case "6":
                        await EditProductField(6, articleNumber, email);
                        return;
                    default:
                        WriteLine("Invalid value, please enter in a valid field number");
                        option = Console.ReadLine();
                        break;
                }
            }
        }
        else
        {
            Blank();
            WriteLine("No product found.");
            PressAnyKey();
            await AllProducts(email);
        }
    }
    public async Task EditProductField(int fieldNumber, string articleNumber, string email)
    {
        Console.Clear();
        var product = await _productService.GetProductById(articleNumber);

        Write("Enter in new value for chosen field: ");
        var newValue = Console.ReadLine();

        while (newValue == "")
        {
            WriteLine("Value can't be empty, please enter in a value");
            newValue = Console.ReadLine();
        }

        switch (fieldNumber)
        {
            case 1:
                if (!decimal.TryParse(newValue!, out decimal incorrectTitleValue))
                {
                    product.Title = newValue!;
                }
                else
                {
                    WriteLine("Not a valid Title. Please try again");
                    newValue = Console.ReadLine();
                }
                break;
            case 2:
                if (!decimal.TryParse(newValue!, out decimal incorrectManufacturerValue))
                {
                    product.Manufacturer = newValue!;
                }
                else
                {
                    WriteLine("Not a valid Manufacturer name price. Please try again");
                    newValue = Console.ReadLine();
                }
                break;
            case 3:
                if (decimal.TryParse(newValue!, out decimal newPriceValue))
                {
                    product.Price = newPriceValue;
                }
                else
                {
                    WriteLine("Not a valid price. Please try again");
                    newValue = Console.ReadLine();
                }
                break;

            case 4:
                if (decimal.TryParse(newValue!, out decimal newDiscountValue))
                {
                    product.DiscountPrice = newDiscountValue;
                }
                else
                {
                    WriteLine("Not a valid discount price. Please try again");
                    newValue = Console.ReadLine();
                }
                break;
            case 5:
                if (!decimal.TryParse(newValue!, out decimal incorrectCategoryValue))
                {
                    product.Category.Category = newValue!;
                }
                else
                {
                    WriteLine("Not a valid category name. Please try again");
                    newValue = Console.ReadLine();
                }
                break;
            case 6:
                if (!decimal.TryParse(newValue!, out decimal incorrectParentValue))
                {
                    product.Category.ParentCategory = newValue!;
                }
                else
                {
                    WriteLine("Not a valid category name. Please try again");
                    newValue = Console.ReadLine();
                }
                break;
        }

        var updateResult = await _productService.UpdateProduct(product);

        if (updateResult is ProductModel updatedProduct)
        {
            Console.Clear();
            WriteLine("Product was successfully updated.");
            Thread.Sleep(2000);
            await ProductDetails(articleNumber, email);
        }
        else
        {
            Console.Clear();
            WriteLine("Property was not updated due to an internal error. Please try again");
            Thread.Sleep(2000);
            await ProductDetails(articleNumber, email);
        }
    }
    public async Task DeleteProduct(string articleNumber, string email)
    {
        Console.Clear();
        WriteLine("Are you sure you want to delete this product? (y/n)");
        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "n":
                    await ProductDetails(articleNumber, email);
                    return;
                case "y":
                    var result = await _productService.DeleteProductById(articleNumber);

                    if (result)
                    {
                        WriteLine("Product successfully deleted.");
                        Thread.Sleep(1500);
                        await AllProducts(email);
                    }
                    else
                    {
                        WriteLine("Product could not be deleted due to an internal error.");
                        Thread.Sleep(1500);
                        await AllProducts(email);
                    }
                    return;
                default:
                    WriteLine("Invalid option, please enter in either 'y' or 'n'");
                    option = Console.ReadLine();
                    break;
            }
        }
    }

    // ------------------- PRODUCT DESCRIPTION MENUS --------------------------- //

    public async Task AllDescriptions(string articleNumber, string email)
    {
        Header("PRODUCTS DESCRIPTIONS");
        var descriptions = await _descriptionService.GetDescriptionsByProperty(x => x.ArticleNumber == articleNumber);

        if (descriptions != null)
        {
            if (descriptions.Count > 0)
            {
                WriteLine("Enter 1 to add new description with another language");
                WriteLine("Enter 2 to edit/delete existing descriptions");
                WriteLine("Enter 3 to go back to product details");
                Blank();

                foreach (var desc in descriptions)
                {
                    DashedLine();
                    WriteLine($"Id: {desc.Id}");
                    WriteLine($"Description: {desc.Description}");
                    WriteLine($"Language: {desc.Language}");
                    WriteLine($"Specifications: {desc.Specifications}");
                    WriteLine($"Ingress: {desc.Ingress}");
                    WriteLine($"ArticleNumber: {desc.ArticleNumber}");
                    Blank();
                }

                var option = Console.ReadLine();

                while (true)
                {
                    switch (option)
                    {
                        case "1":
                            await AddDescription(articleNumber, email);
                            return;
                        case "2":
                            await EditDescription(articleNumber, email);
                            return;
                        case "3":
                            await ProductDetails(articleNumber, email);
                            return;
                        default:
                            option = Console.ReadLine();
                            break;
                    }
                }
            }
            else
            {
                Blank();
                WriteLine("No descriptions found for this product. Enter 1 to add new description, or 2 to go back");
                var secondOption = Console.ReadLine();

                while (true)
                {
                    switch (secondOption)
                    {
                        case "1":
                            await AddDescription(articleNumber, email);
                            return;
                        case "2":
                            await AllDescriptions(articleNumber, email);
                            return;
                        default:
                            secondOption = Console.ReadLine();
                            break;
                    }
                }
            }
        }
        else
        {
            WriteLine("Error: NullValues");
            PressAnyKey();
            await ProductDetails(articleNumber, email);
        }
        
    }
    public async Task AddDescription(string articleNumber, string email)
    {
        Header("ADD NEW DESCRIPTION");

        var product = await _productService.GetProductById(articleNumber);
        WriteLine($"Add new description for product: {product.Title}, with article number: {product.ArticleNumber}, by filling in the information below");

        Write("Description: ");
        var descriptionEntry = Console.ReadLine();
        while (descriptionEntry == "" || int.TryParse(descriptionEntry, out int number)) { WriteLine("Value can't be empty or a number"); descriptionEntry = Console.ReadLine(); }

        Write("Specifications: ");
        var specificationEntry = Console.ReadLine();
        while (specificationEntry == "" || int.TryParse(specificationEntry, out int number)) { WriteLine("Value can't be empty or a number"); specificationEntry = Console.ReadLine(); }

        Write("Ingress");
        var ingressEntry = Console.ReadLine();

        Write("Description language: ");
        var languageEntry = Console.ReadLine();
        while (languageEntry == "" || int.TryParse(languageEntry, out int number)) { WriteLine("Value can't be empty or a number"); languageEntry = Console.ReadLine(); }

        var existingDescriptions = await _descriptionService.GetDescriptionsByProperty(x => x.ArticleNumber == articleNumber);

        if (!existingDescriptions.Any(x => x.Language == languageEntry))
        {
            var result = await _descriptionService.CreateDescription(new DescriptionModel()
            {
                Description = descriptionEntry!,
                Specifications = specificationEntry!,
                Ingress = ingressEntry,
                Language = languageEntry!,
                ArticleNumber = articleNumber,
            });

            if (result is DescriptionModel)
            {
                Console.Clear();
                WriteLine("Description successfully created.");
                PressAnyKey();
                await AllDescriptions(articleNumber, email);
            }
            else
            {
                Console.Clear();
                WriteLine("Description could not be created due to an internal error");
                PressAnyKey();
                await AllDescriptions(articleNumber, email);
            }
        }
        else
        {
            WriteLine("A description with this language already exists for this product. Please edit/delete the existing one, or create a new one with a new language.");
            PressAnyKey();
            await AllDescriptions(articleNumber, email);
        }
    }
    public async Task EditDescription(string articleNumber, string email)
    {
        Header("EDIT PRODUCTS DESCRIPTIONS");
        var descriptions = await _descriptionService.GetDescriptionsByProperty(x => x.ArticleNumber == articleNumber);

        if (descriptions != null)
        {
            WriteLine("Enter the id of the description you want to edit/delete, or enter 'q' to go back");
            Blank();

            foreach (var desc in descriptions)
            {
                DashedLine();
                WriteLine($"Id: {desc.Id}");
                WriteLine($"Description: {desc.Description}");
                WriteLine($"Language: {desc.Language}");
                WriteLine($"Specifications: {desc.Specifications}");
                WriteLine($"Ingress: {desc.Ingress}");
                WriteLine($"ArticleNumber: {desc.ArticleNumber}");
                Blank();
            }

            var option = Console.ReadLine();

            while (true)
            {
                if (option == "q")
                {
                    await AllDescriptions(articleNumber, email);
                    return;
                }
                else if (int.TryParse(option, out int id))
                {
                    var existingDescription = await _descriptionService.GetDescriptionById(id);

                    if (existingDescription != null)
                    {
                        await ChooseDescriptionField(id, email, articleNumber);
                        return;
                    }
                    else
                    {
                        WriteLine("Invalid value, please enter in a valid field number");
                        option = Console.ReadLine();
                    }
                }
                else
                {
                    WriteLine("Invalid value, please enter in a valid description Id number");
                    option = Console.ReadLine();
                }
            }
        }
    }
    public async Task ChooseDescriptionField(int descriptionId, string email, string articleNumber)
    {
        Header("EDIT PRODUCTS DESCRIPTIONS");

        var descriptionToUpdate = await _descriptionService.GetDescriptionById(descriptionId);

        if (descriptionToUpdate != null)
        {
            DashedLine();
            WriteLine($"[Id]: {descriptionToUpdate.Id}");
            WriteLine($"1. Description: {descriptionToUpdate.Description}");
            WriteLine($"2. Specifications: {descriptionToUpdate.Specifications}");
            WriteLine($"3. Ingress: {descriptionToUpdate.Ingress}");
            WriteLine($"[Language]: {descriptionToUpdate.Language}");
            WriteLine($"[Article number]: {descriptionToUpdate.ArticleNumber}");
            DashedLine();
            Blank();

            WriteLine("Enter in the number of the field you wish to edit, or enter 9 to delete this description");
            WriteLine("To go back, enter 'q'");

            var option = Console.ReadLine();

            while (true)
            {
                switch (option)
                {
                    case "q":
                        await EditDescription(articleNumber, email);
                        return;
                    case "1":
                        await EditDescriptionField(1, descriptionId, articleNumber, email);
                        return;
                    case "2":
                        await EditDescriptionField(2, descriptionId, articleNumber, email);
                        return;
                    case "3":
                        await EditDescriptionField(3, descriptionId, articleNumber, email);
                        return;
                    case "9":
                        await DeleteDescription(descriptionId, articleNumber, email);
                        return;
                    default:
                        WriteLine("Invalid option, please try again");
                        option = Console.ReadLine();
                        break;
                }
            }
        }
        else
        {
            WriteLine("No product description found with given Id.");
            PressAnyKey();
            await EditDescription(articleNumber, email);
        }
    }
    public async Task EditDescriptionField(int fieldNumber, int descriptionId, string articleNumber, string email)
    {
        Console.Clear();
        var description = await _descriptionService.GetDescriptionById(descriptionId);

        Write("Enter in new value for chosen field: ");
        var newValue = Console.ReadLine();

        while (newValue == "")
        {
            WriteLine("Value can't be empty, please enter in a value");
            newValue = Console.ReadLine();
        }

        switch (fieldNumber)
        {
            case 1:
                description.Description = newValue!;
                break;
            case 2:
                description.Specifications = newValue!;
                break;
            case 3:
                description.Ingress = newValue;
                break;
        }

        var updateResult = await _descriptionService.UpdateDescription(description);

        if (updateResult is DescriptionModel)
        {
            Console.Clear();
            WriteLine("Description was successfully updated.");
            Thread.Sleep(2000);
            await ChooseDescriptionField(descriptionId, articleNumber, email);
        }
        else
        {
            Console.Clear();
            WriteLine("Property was not updated due to an internal error. Please try again");
            Thread.Sleep(2000);
            await ChooseDescriptionField(descriptionId, articleNumber, email);
        }
    }
    public async Task DeleteDescription(int descriptionId, string articleNumber, string email)
    {
        Console.Clear();
        var descriptionToDelete = await _descriptionService.GetDescriptionById(descriptionId);
        WriteLine("Are you sure you want to delete this description? (y/n)");
        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "n":
                    await ChooseDescriptionField(descriptionId, articleNumber, email);
                    return;
                case "y":
                    var result = await _descriptionService.DeleteDescription(descriptionToDelete);

                    if (result)
                    {
                        WriteLine("Description was successfully deleted.");
                        Thread.Sleep(2000);
                        await AllDescriptions(articleNumber, email);
                    }
                    else
                    {
                        WriteLine("Description could not be deleted due to an error");
                        Thread.Sleep(2000);
                        await AllDescriptions(articleNumber, email);
                    }
                    return;
                default:
                    WriteLine("Invalid option, please enter y/n");
                    option = Console.ReadLine();
                    break;
            }
        }
    }

    // ------------------- PRODUCT REVIEW MENUS --------------------------- //

    public async Task AllReviews(string articleNumber, string email)
    {
        Header("PRODUCT REVIEWS");
        var reviews = await _reviewService.GetReviewsByProperty(x => x.ArticleNumber == articleNumber);

        if (reviews != null)
        {
            if (reviews.Count() == 0)
            {
                WriteLine("No existing reviews for this product");
                Blank();
                WriteLine("Enter 1 to add new review");
                WriteLine("Enter 2 to go back to product details");
                Blank();

                var option = Console.ReadLine();

                while (true)
                {
                    switch (option)
                    {
                        case "2":
                            await ProductDetails(articleNumber, email);
                            return;
                        case "1":
                            await AddReview(articleNumber, email);
                            return;
                        default:
                            WriteLine("Invalid option, please enter either 1 or 2");
                            break;
                    }
                }
            }
            else
            {
                WriteLine("Enter 1 to add new review");
                WriteLine("Enter 2 to delete existing reviews");
                WriteLine("Enter 3 to go back to product details");
                Blank();

                foreach (var review in reviews)
                {
                    DashedLine();
                    WriteLine($"Id: {review.Id}");
                    WriteLine($"Review: {review.Review}");
                    WriteLine($"Rating: {review.Rating}");
                    WriteLine($"Author: {review.Author}");
                    WriteLine($"Article number: {review.ArticleNumber}");
                    Blank();
                }

                var option = Console.ReadLine();

                while (true)
                {
                    switch (option)
                    {
                        case "1":
                            await AddReview(articleNumber, email);
                            return;
                        case "2":
                            await DeleteReview(articleNumber, email);
                            return;
                        case "3":
                            await ProductDetails(articleNumber, email);
                            return;
                        default:
                            option = Console.ReadLine();
                            break;
                    }
                }
            }
        }
        else
        {
            WriteLine("Could not display null items due to internal error");
            PressAnyKey();
            await ProductDetails(articleNumber, email);
        }
    }
    public async Task AddReview(string articleNumber, string email)
    {
        Header("ADD NEW REVIEW");

        var product = await _productService.GetProductById(articleNumber);
        WriteLine($"Add new review for product: {product.Title}, with article number: {product.ArticleNumber}, by filling in the information below");
        Blank();

        Write("Review: ");
        var reviewEntry = Console.ReadLine();
        while (reviewEntry == "" || int.TryParse(reviewEntry, out int number)) { WriteLine("Value can't be empty or a number"); reviewEntry = Console.ReadLine(); }

        Write("Rating (enter in a number from 1-10): ");
        var ratingEntry = Console.ReadLine();
        while (ratingEntry == "" || !byte.TryParse(ratingEntry, out byte rating)) { WriteLine("Value must be a number"); ratingEntry = Console.ReadLine(); }

        Write("Author: ");
        var authorEntry = Console.ReadLine();

        if (byte.TryParse(ratingEntry, out byte ratingNumber))
        {
            var result = await _reviewService.CreateReview(new ReviewModel()
            {
                Review = reviewEntry!,
                Rating = ratingNumber!,
                Author = authorEntry,
                ArticleNumber = articleNumber,
            });

            if (result is ReviewModel)
            {
                Console.Clear();
                WriteLine("Review successfully created.");
                PressAnyKey();
                await AllReviews(articleNumber, email);
            }
            else
            {
                Console.Clear();
                WriteLine("Review could not be created due to an internal error");
                PressAnyKey();
                await AllReviews(articleNumber, email);
            }
        }
    }
    public async Task DeleteReview(string articleNumber, string email)
    {
        Header("DELETE PRODUCT REVIEWS");
        var reviews = await _reviewService.GetReviewsByProperty(x => x.ArticleNumber == articleNumber);

        if (reviews != null)
        {
            WriteLine("Enter the id of the review you want to delete, or enter 'q' to go back");
            Blank();

            foreach (var review in reviews)
            {
                DashedLine();
                WriteLine($"Id: {review.Id}");
                WriteLine($"Review: {review.Review}");
                WriteLine($"Rating: {review.Rating}");
                WriteLine($"Author: {review.Author}");
                WriteLine($"Article number: {review.ArticleNumber}");
                Blank();
            }

            var option = Console.ReadLine();

            while (true)
            {
                if (option == "q")
                {
                    await AllReviews(articleNumber, email);
                    return;
                }
                else if (int.TryParse(option, out int id))
                {
                    var result = await _reviewService.DeleteReviewById(id);
            
                    if (result)
                    {
                        Console.Clear();
                        WriteLine("Review successfully removed.");
                        PressAnyKey();
                        await AllReviews(articleNumber, email);
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        WriteLine("Review could not be deleted due to an internal error");
                        PressAnyKey();
                        await AllReviews(articleNumber, email);
                        return;
                    }
                }
                else
                {
                    WriteLine("Invalid value, please enter in a valid review Id number");
                    option = Console.ReadLine();
                }
            }
        }
    }
}
