using System.Diagnostics;
using TWBD_Domain.DTOs;
using TWBD_Domain.DTOs.Enums;
using TWBD_Domain.DTOs.Responses;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services;
public class UserRegisterService
{
    private readonly UserAddressService _addressService;
    private readonly UserRepository _userRepository;
    private readonly AuthenticationRepository _authenticationRepository;
    private readonly ProfileRepository _profileRepository;
    private readonly UserRoleService _userRoleService;
    private readonly UserSecurityService _userSecurityService;

    public UserRegisterService
        (UserRepository userRepository, 
        UserAddressService addressService, 
        AuthenticationRepository authenticationRepository, 
        ProfileRepository profileRepository,
        UserRoleService userRoleService,
        UserSecurityService userSecurityService)
    {
        _addressService = addressService;
        _userRepository = userRepository;
        _authenticationRepository = authenticationRepository;
        _profileRepository = profileRepository;
        _userRoleService = userRoleService;
        _userSecurityService = userSecurityService;
    }

    public async Task<ServiceResponse> RegisterUser(UserRegistrationModel user)
    {
        try
        {
            var newUser = await _userRepository.CreateAsync(new UserEntity()
            {
                RoleId = await _userRoleService.GetRoleId(user.Role)
            });

            if (newUser != null) 
            {
                var newUserAuth = await _authenticationRepository.CreateAsync(new UserAuthenticationEntity()
                {
                    UserId = newUser.UserId,
                    Email = user.Email,
                    PasswordHash = _userSecurityService.GenerateSecurePassword(user.Password)
                });

                if (newUserAuth != null)
                {
                    var newUserProfile = await _profileRepository.CreateAsync(new UserProfileEntity()
                    {
                        UserId = newUser.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        AddressId = await _addressService.GetAddressId(new AddressModel()
                        {
                            City = user.City,
                            StreetName = user.StreetName,
                            PostalCode = user.PostalCode,
                        })
                    });

                    if (newUserProfile != null)
                    {
                        return new ServiceResponse()
                        {
                            Success = true,
                            Code = ServiceCode.CREATED,
                            ReturnObject = new UserModel()
                            {
                                UserId = newUser.UserId,
                                FirstName = newUserProfile.FirstName,
                                LastName = newUserProfile.LastName,
                                Email = newUserAuth.Email,
                                PhoneNumber = newUserProfile.PhoneNumber, 
                                Role = await _userRoleService.GetRoleType(newUser.RoleId)
                            }
                        };
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }
}
