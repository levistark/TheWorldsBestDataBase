using System.Diagnostics;
using TWBD_Domain.DTOs.Enums;
using TWBD_Domain.DTOs.Models;
using TWBD_Domain.DTOs.Responses;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services;
public class UserLoginService
{
    private readonly UserRepository _userRepository;
    private readonly AuthenticationRepository _authenticationRepository;
    private readonly UserSecurityService _userSecurityService;

    public UserLoginService
        (UserRepository userRepository,
        AuthenticationRepository authenticationRepository,
        UserSecurityService userSecurityService)
    {
        _userRepository = userRepository;
        _authenticationRepository = authenticationRepository;
        _userSecurityService = userSecurityService;
    }

    public async Task<ServiceResponse> LoginValidation(LoginModel login)
    {
        try
        {
            if (string.IsNullOrEmpty(login.PasswordEntry) || string.IsNullOrEmpty(login.EmailEntry))
                return new ServiceResponse() { Code = ServiceCode.NULL_VALUES };

            var verificationResult = await _userSecurityService.VerifyPassword(login.PasswordEntry, login.EmailEntry);

            if (verificationResult)
                return new ServiceResponse() { Success = true };
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }
}
