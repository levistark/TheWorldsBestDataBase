using System.Diagnostics;
using System.Text.RegularExpressions;
using TWBD_Domain.DTOs.Responses;
using TWBD_Infrastructure.Migrations;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services;
public class UserValidationService(AuthenticationRepository authenticationRepository)
{
    private readonly AuthenticationRepository _authenticationRepository = authenticationRepository;

    public AuthenticationResponse ValidatePassword(string password)
    {
        try
        {
            var regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");

            // If password criteria not met:
            if (regex.IsMatch(password))
            {
                return new AuthenticationResponse() { Success = true };
            }
            else
                return new AuthenticationResponse() { Code = AuthenticationCode.INVALID_PASSWORD, Success = false };
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new AuthenticationResponse();
    }

    public async Task<AuthenticationResponse> ValidateEmail(string email)
    {
        try
        {
            var regex = new Regex("^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))\\z");

            // Does the email match the criteria?
            if (!regex.IsMatch(email))
                return new AuthenticationResponse() { Code = AuthenticationCode.INVALID_EMAIL };

            // Does the user exist?
            if (await _authenticationRepository.Existing(x => x.Email == email))
                return new AuthenticationResponse() { Code = AuthenticationCode.ALREADY_EXISTS };

            return new AuthenticationResponse() { Success = true };
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new AuthenticationResponse() { Code = AuthenticationCode.FAILED };
    }
    public AuthenticationResponse ValidateUpdatePassword(string newPassword, string oldPassword)
    {
        try
        {
            // Is the password the same as the old password?
            if (newPassword == oldPassword)
                return new AuthenticationResponse() { Code = AuthenticationCode.ALREADY_EXISTS };

            if (!ValidatePassword(newPassword).Success)
                return new AuthenticationResponse() { Code = AuthenticationCode.INVALID_PASSWORD };

            return new AuthenticationResponse() { Success = true };
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new AuthenticationResponse();
    }

}
