using System.Diagnostics;

namespace TWBD_Domain.Services;
public class SecurityService
{
    public string GenerateSecurePassword(string password)
    {
        try
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
