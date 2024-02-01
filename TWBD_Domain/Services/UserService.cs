using System.Diagnostics;
using TWBD_Domain.DTOs;
using TWBD_Domain.DTOs.Enums;
using TWBD_Domain.DTOs.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace TWBD_Domain.Services;
public class UserService
{
    private readonly UserValidationService _validation;
    private readonly UserRegisterService _userRegisterService;

    public UserService(UserValidationService validation, UserRegisterService userRegisterService)
    {
        _validation = validation;
        _userRegisterService = userRegisterService;
    }

    // Add new user
    public async Task<ServiceResponse> CreateUser(UserRegistrationModel newUser)
    {
        try
        {
            // Validate user email and password
            var emailValid = await _validation.ValidateEmail(newUser.Email);
            var passwordValid = _validation.ValidatePassword(newUser.Password);
            var passwordMatching = _validation.ValidateMatchingPassword(newUser.Password, newUser.PasswordConfirm);

            if (emailValid.Code == ValidationCode.ALREADY_EXISTS) return new ServiceResponse() { Code = ServiceCode.ALREADY_EXISTS };
            if (emailValid.Code == ValidationCode.INVALID_EMAIL) return new ServiceResponse() { Code = ServiceCode.INVALID_EMAIL };
            if (passwordValid.Code == ValidationCode.INVALID_PASSWORD) return new ServiceResponse() { Code = ServiceCode.INVALID_PASSWORD };
            if (passwordMatching.Code == ValidationCode.PASSWORD_NOT_MATCH) return new ServiceResponse() { Code = ServiceCode.PASSWORD_NOT_MATCH};

            if (string.IsNullOrEmpty(newUser.FirstName) || string.IsNullOrEmpty(newUser.FirstName) || string.IsNullOrEmpty(newUser.City) || string.IsNullOrEmpty(newUser.StreetName) || string.IsNullOrEmpty(newUser.PostalCode) || string.IsNullOrEmpty(newUser.Role))
                return new ServiceResponse() { Code = ServiceCode.NULL_VALUES};

            var result = await _userRegisterService.RegisterUser(newUser);

            if (result.ReturnObject is UserModel model)
            {
                return new ServiceResponse()
                {
                    Success = true,
                    ReturnObject = model,
                };
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

}
