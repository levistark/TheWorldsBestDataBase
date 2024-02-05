using System.Diagnostics;
using TWBD_Domain.DTOs.Enums;
using TWBD_Domain.DTOs.Models;
using TWBD_Domain.DTOs.Responses;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace TWBD_Domain.Services;
public class UserService
{
    private readonly UserValidationService _validation;
    private readonly UserRepository _userRepository;
    private readonly AuthenticationRepository _authenticationRepository;
    private readonly ProfileRepository _profileRepository;
    private readonly UserRoleService _userRoleService;
    private readonly UserSecurityService _userSecurityService;
    private readonly UserAddressService _addressService;

    public UserService(UserValidationService validation,
        UserRepository userRepository,
        UserAddressService addressService,
        AuthenticationRepository authenticationRepository,
        ProfileRepository profileRepository,
        UserRoleService userRoleService,
        UserSecurityService userSecurityService)
    {
        _validation = validation;
        _userRepository = userRepository;
        _authenticationRepository = authenticationRepository;
        _profileRepository = profileRepository;
        _userRoleService = userRoleService;
        _userSecurityService = userSecurityService;
        _addressService = addressService;
    }

    // Register new user
    public async Task<ServiceResponse> RegisterUser(UserRegistrationModel user)
    {
        try
        {
            // Validate user email and password
            var emailValid = await _validation.ValidateEmail(user.Email);
            var passwordValid = _validation.ValidatePassword(user.Password);
            var passwordMatching = _validation.ValidateMatchingPassword(user.Password, user.PasswordConfirm);

            // Return error codes if invalid
            if (emailValid.Code == ValidationCode.ALREADY_EXISTS) return new ServiceResponse() { Code = ServiceCode.ALREADY_EXISTS };
            if (emailValid.Code == ValidationCode.INVALID_EMAIL) return new ServiceResponse() { Code = ServiceCode.INVALID_EMAIL };
            if (passwordValid.Code == ValidationCode.INVALID_PASSWORD) return new ServiceResponse() { Code = ServiceCode.INVALID_PASSWORD };
            if (passwordMatching.Code == ValidationCode.PASSWORD_NOT_MATCH) return new ServiceResponse() { Code = ServiceCode.PASSWORD_NOT_MATCH };

            // Validate other string fields
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.City) || string.IsNullOrEmpty(user.StreetName) || string.IsNullOrEmpty(user.PostalCode) || string.IsNullOrEmpty(user.Role))
                return new ServiceResponse() { Code = ServiceCode.NULL_VALUES };

            // Register a new user entity
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
                        var createdProfile = await GetUserProfileByEmail(user.Email);

                        if (createdProfile.ReturnObject is UserProfileModel profile)
                        {
                            return new ServiceResponse()
                            {
                                Success = true,
                                Code = ServiceCode.CREATED,
                                ReturnObject = profile
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> GetUserProfileById(int id)
    {
        try
        {
            if (id == 0)
                return new ServiceResponse() { Code = ServiceCode.NULL_VALUES };

            var entity = await _userRepository.ReadOneAsync(u => u.UserId == id);

            if (entity.UserProfile != null && entity.UserAuthentication != null)
            {
                return new ServiceResponse()
                {
                    Success = true,
                    ReturnObject = new UserProfileModel()
                    {
                        UserId = entity.UserId,
                        FirstName = entity.UserProfile.FirstName,
                        LastName = entity.UserProfile.LastName,
                        Email = entity.UserAuthentication.Email,
                        PhoneNumber = entity.UserProfile.PhoneNumber,
                        Role = entity.Role.RoleType,
                        City = entity.UserProfile.Address.City,
                        StreetName = entity.UserProfile.Address.StreetName,
                        PostalCode = entity.UserProfile.Address.PostalCode,
                    }
                };
            }
            else
                return new ServiceResponse() { Message = "User does either not have a profile or registered authentication credentials" };
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> GetUserProfileByEmail(string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
                return new ServiceResponse() { Code = ServiceCode.NULL_VALUES };

            var entity = await _userRepository.ReadOneAsync(u => u.UserAuthentication!.Email == email);

            if (entity.UserProfile != null && entity.UserAuthentication != null)
            {
                return new ServiceResponse()
                {
                    Success = true,
                    ReturnObject = new UserProfileModel()
                    {
                        UserId = entity.UserId,
                        FirstName = entity.UserProfile.FirstName,
                        LastName = entity.UserProfile.LastName,
                        Email = entity.UserAuthentication.Email,
                        PhoneNumber = entity.UserProfile.PhoneNumber,
                        Role = entity.Role.RoleType,
                        City = entity.UserProfile.Address.City,
                        StreetName = entity.UserProfile.Address.StreetName,
                        PostalCode = entity.UserProfile.Address.PostalCode,
                    }
                };
            }
            else
                return new ServiceResponse() { Message = "User does either not have a profile or registered authentication credentials" };
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> GetAllUserProfiles()
    {
        try
        {
            var userList = new List<UserProfileModel>();
            var entityList = await _userRepository.ReadAllAsync();

            if (entityList.Count() != 0 || entityList != null)
            {
                foreach (var entity in entityList)
                {
                    if (entity.UserProfile != null && entity.UserAuthentication != null)
                    {
                        userList.Add(new UserProfileModel()
                        {
                            UserId = entity.UserId,
                            FirstName = entity.UserProfile.FirstName,
                            LastName = entity.UserProfile.LastName,
                            Email = entity.UserAuthentication.Email,
                            PhoneNumber = entity.UserProfile.PhoneNumber,
                            Role = entity.Role.RoleType,
                            City = entity.UserProfile.Address.City,
                            StreetName = entity.UserProfile.Address.StreetName,
                            PostalCode = entity.UserProfile.Address.PostalCode,
                            ProfileImage = entity.UserProfile.ProfileImage
                        });
                    }
                }
            }
            else
                return new ServiceResponse() { Message = "No users in database" };

            return new ServiceResponse()
            {
                Success = true,
                ReturnObject = userList
            };
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> UpdateUserProfile(UserProfileModel profileModel)
    {
        try
        {
            // Validate user email 
            var emailValid = await _validation.ValidateEmail(profileModel.Email);

            // Return error codes if invalid
            if (emailValid.Code == ValidationCode.ALREADY_EXISTS) return new ServiceResponse() { Code = ServiceCode.ALREADY_EXISTS };
            if (emailValid.Code == ValidationCode.INVALID_EMAIL) return new ServiceResponse() { Code = ServiceCode.INVALID_EMAIL };

            if (emailValid.Success)
            {
                // Validate string fields
                if (string.IsNullOrEmpty(profileModel.FirstName) || string.IsNullOrEmpty(profileModel.FirstName) || string.IsNullOrEmpty(profileModel.City) || string.IsNullOrEmpty(profileModel.StreetName) || string.IsNullOrEmpty(profileModel.PostalCode) || string.IsNullOrEmpty(profileModel.Role))
                    return new ServiceResponse() { Code = ServiceCode.NULL_VALUES };

                var userToBeUpdated = await _userRepository.ReadOneAsync(u => u.UserId == profileModel.UserId);

                // Update user entity
                var updatedUser = await _userRepository.UpdateAsync(u => u.UserId == profileModel.UserId, new UserEntity()
                {
                    UserId = profileModel.UserId,
                    RoleId = await _userRoleService.GetRoleId(profileModel.Role)
                });

                if (updatedUser == null || userToBeUpdated.UserAuthentication == null)
                {
                    return new ServiceResponse() { Message = "updatedUser = null" };
                }
                else
                {
                    // Update user authentication entity
                    var updatedUserAuth = await _authenticationRepository.UpdateAsync(u => u.UserId == profileModel.UserId, new UserAuthenticationEntity()
                    {
                        UserId = profileModel.UserId,
                        Email = profileModel.Email,

                        // Not updating password this way (keeping it the same as before)
                        PasswordHash = userToBeUpdated.UserAuthentication.PasswordHash
                    });

                    if (updatedUserAuth == null || userToBeUpdated.UserProfile == null)
                    {
                        return new ServiceResponse() { Message = "updatedUserAuth = null" };
                    }
                    else
                    {
                        // Update user profile entity
                        var updatedUserProfile = await _profileRepository.UpdateAsync(u => u.UserId == profileModel.UserId, new UserProfileEntity()
                        {
                            UserId = profileModel.UserId,
                            FirstName = profileModel.FirstName,
                            LastName = profileModel.LastName,
                            AddressId = await _addressService.GetAddressId(new AddressModel()
                            {
                                City = profileModel.City,
                                StreetName = profileModel.StreetName,
                                PostalCode = profileModel.PostalCode,
                            })
                        });

                        if (updatedUserProfile == null)
                        {
                            return new ServiceResponse() { Message = "updatedUserProfile = null" };
                        }
                        else
                        {
                            var updatedUserFromDb = await _userRepository.ReadOneAsync(u => u.UserId == profileModel.UserId);

                            if (updatedUserFromDb.UserProfile!.FirstName == profileModel.FirstName)
                            {
                                return new ServiceResponse()
                                {
                                    Success = true,
                                    Code = ServiceCode.UPDATED,
                                };
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> DeleteUserById(int id)
    {
        try
        {
            var entityToDelete = await _userRepository.ReadOneAsync(u => u.UserId == id);

            if (entityToDelete != null)
            {
                if (await _userRepository.DeleteAsync(u => u.UserId == id, entityToDelete))
                    return new ServiceResponse { Success = true, Code = ServiceCode.DELETED };
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse(); 
    }

    public async Task<ServiceResponse> DeleteUserByEmail(string email)
    {
        try
        {
            var entityToDelete = await _userRepository.ReadOneAsync(u => u.UserAuthentication!.Email == email);

            if (entityToDelete != null)
            {
                if (await _userRepository.DeleteAsync(u => u.UserAuthentication!.Email == email, entityToDelete))
                    return new ServiceResponse { Success = true, Code = ServiceCode.DELETED };
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> DeleteUserProfileById(int id)
    {
        try
        {
            var profileToDelete = await _profileRepository.ReadOneAsync(u => u.UserId == id);

            if (profileToDelete != null)
            {
                if (await _profileRepository.DeleteAsync(u => u.UserId == id, profileToDelete))
                    return new ServiceResponse { Success = true, Code = ServiceCode.DELETED};
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> DeleteUserProfileByEmail(string email)
    {
        try
        {
            var authenticationTodelete = await _authenticationRepository.ReadOneAsync(u => u.Email == email);

            if (authenticationTodelete != null)
            {
                if (await _authenticationRepository.DeleteAsync(u => u.Email == email, authenticationTodelete))
                    return new ServiceResponse { Success = true, Code = ServiceCode.DELETED };
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> DeleteUserAuthenticationById(int id)
    {
        try
        {
            var authenticationTodelete = await _authenticationRepository.ReadOneAsync(u => u.UserId == id);

            if (authenticationTodelete != null)
            {
                if (await _authenticationRepository.DeleteAsync(u => u.UserId == id, authenticationTodelete))
                    return new ServiceResponse { Success = true, Code = ServiceCode.DELETED };
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> DeleteUserAuthenticationByEmail(string email)
    {
        try
        {
            var authenticationTodelete = await _authenticationRepository.ReadOneAsync(u => u.Email == email);

            if (authenticationTodelete != null)
            {
                if (await _authenticationRepository.DeleteAsync(u => u.Email == email, authenticationTodelete))
                    return new ServiceResponse { Success = true, Code = ServiceCode.DELETED };
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> ValidateEmail(string email)
    {
        try
        {
            // Validate user email
            var emailValid = await _validation.ValidateEmail(email);

            // Return error codes if invalid
            if (emailValid.Code == ValidationCode.ALREADY_EXISTS) return new ServiceResponse() { Code = ServiceCode.ALREADY_EXISTS };
            if (emailValid.Code == ValidationCode.INVALID_EMAIL) return new ServiceResponse() { Code = ServiceCode.INVALID_EMAIL };

            else return new ServiceResponse() { Success = true };
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

    public  ServiceResponse ValidatePassword(string password)
    {
        try
        {
            // Validate user password
            var passwordValid = _validation.ValidatePassword(password);

            // Return error codes if invalid
            if (passwordValid.Code == ValidationCode.INVALID_PASSWORD) return new ServiceResponse() { Code = ServiceCode.INVALID_PASSWORD };
            else return new ServiceResponse() { Success = true };
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse();
    }

}
