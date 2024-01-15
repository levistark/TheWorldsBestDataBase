
using System.Security.Cryptography;

namespace TheWorldsBestDataBase_Console;

public class ViewServices
{

    private void WriteLine(string text) { Console.WriteLine(text); }
    private void Write(string text) { Console.Write(text); }
    private void Blank() { Console.WriteLine(); }
    private void Header(string text)
    {
        Console.Clear();
        WriteLine($"## {text.ToUpper()} ##");
        Blank();
    }
    private void Dash() { Console.WriteLine("----------------------------"); }

    public void StartMenuView()
    {
        Header("IT Service Management");

        WriteLine("1. Products Menu");
        WriteLine("2. Users Menu");
        WriteLine("3. Quit the program");

        var option = Console.ReadLine();
        
        while (true)
        {
            switch (option) 
            {
                case "1":
                    ProductsMenuView();
                    break;
                case "2":
                    UsersMenuView();
                    break;
                case "3":
                    return;
                default:
                    break;
            }
        }
    }

    public void ProductsMenuView()
    {
        Header("Product Management");

        // Lista ut alla produkter, hämtas från databasen

        WriteLine("1. Show products");
        WriteLine("2. Add new product");
        WriteLine("3. Edit existing product");
        WriteLine("4. Back to main menu");

        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "1":
                    AllProductsView();
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    StartMenuView();
                    return;
                default:
                    break;
            }
        }
    }

    public void UsersMenuView()
    {
        Header("User Management");


        WriteLine("1. Browse through all users");
        WriteLine("2. Add new user");
        WriteLine("3. Edit existing user");
        WriteLine("4. Back to main menu");

        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "1":
                    AllUsersView();
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    StartMenuView();
                    return;
                default:
                    break;
            }
        }
    }

    public void AllUsersView()
    {
        Header("All Users");

        // Lista ut alla användare, hämtas från databasen

        WriteLine("1. Search user");
        WriteLine("2. Filter list");
        WriteLine("3. Back to Users Menu");

        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "1":
                    SearchUserView();
                    break;
                case "2":
                    FilterUserListView();
                    break;
                case "3":
                    UsersMenuView();
                    return;
                default:
                    break;
            }
        }
    }

    public void SearchUserView()
    {
        Header("Search for a user in the list or press 'q' to go back");

        UserList();

        Write("Search term: ");

        var searchTerm = Console.ReadLine();
    }

    public void FilterUserListView()
    {
        Header("Filter user list");

        WriteLine("Enter a valid property to filter the list, or press 'q' to go back");

        WriteLine("1. Id");
        WriteLine("2. First Name");
        WriteLine("3. Last Name");
        WriteLine("4. Email");
        WriteLine("5. Phone");
        WriteLine("6. City");
        WriteLine("7. Street name");
        WriteLine("8. Street nr");
        WriteLine("9. Postal code");
        WriteLine("10. Role");
        WriteLine("11. Past orders count");
        WriteLine("12. LTV");

        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "q":
                    AllUsersView();
                    return;
                case "2":
                    return;
                case "3":
                    return;
                default:
                    break;
            }
        }
    }

    public void UserList()
    {
        Blank();
        Dash();

        // Lista ut alla användare, hämtas från databasen

        Dash();
        Blank();
    }

    public void AllProductsView()
    {
        Header("All Products");

        // Lista ut alla produkter, hämtas från databasen

        WriteLine("1. Search product");
        WriteLine("2. Filter list");
        WriteLine("3. Back to Products Menu");

        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "1":
                    SearchProductView();
                    break;
                case "2":
                    FilterProductListView();
                    break;
                case "3":
                    ProductsMenuView();
                    return;
                default:
                    break;
            }
        }
    }

    public void SearchProductView()
    {
        Header("Search for a product in the list or press 'q' to go back");

        ProductList();

        Write("Search term: ");

        var searchTerm = Console.ReadLine();
    }

    public void FilterProductListView()
    {
        Header("Filter product list");

        WriteLine("Enter a valid property to filter the list, or press 'q' to go back");

        WriteLine("1. Article number");
        WriteLine("2. Title");
        WriteLine("3. Manufacturer");
        WriteLine("4. Category");
        WriteLine("5. Price");
        WriteLine("6. Price unit");
        WriteLine("7. Review count");
        WriteLine("8. Rating");
        WriteLine("9. Past orders count");

        var option = Console.ReadLine();

        while (true)
        {
            switch (option)
            {
                case "q":
                    AllProductsView();
                    return;
                case "2":
                    return;
                case "3":
                    return;
                default:
                    break;
            }
        }
    }

    public void ProductList()
    {
        Blank();
        Dash();

        // Lista ut alla användare, hämtas från databasen

        Dash();
        Blank();
    }
}
