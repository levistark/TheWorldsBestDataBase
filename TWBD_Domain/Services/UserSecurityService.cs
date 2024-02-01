using System.Diagnostics;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services;
public class UserSecurityService
{
    private readonly AuthenticationRepository _authenticationRepository;

    public UserSecurityService(AuthenticationRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository;
    }
    public string GenerateSecurePassword(string password)
    {
        try
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public async Task<bool> VerifyPassword(string password, string email)
    {
        try
        {
            var passwordHash = await GetPasswordHash(email);
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

    private async Task<string> GetPasswordHash(string email)
    {
        try
        {
            var ua = await _authenticationRepository.ReadOneAsync(x => x.Email == email);
            if (ua != null)
                return ua.PasswordHash;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message);  }
        return null!;
    }
}
