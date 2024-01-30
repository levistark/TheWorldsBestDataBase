using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories;
public class ProfileRepository_Tests
{
    private readonly static UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly RoleRepository _roleRepository = new(_userDataContext);
    private readonly UserRepository _userRepository = new(_userDataContext);
    private readonly AuthenticationRepository _uaRepository = new(_userDataContext);
    private readonly ProfileRepository _profileRepository = new(_userDataContext);

    [Fact]
    public async Task<IEnumerable<UserEntity>> AddSampleDataShould_AddDataToTables_ReturnWithUserList()
    {
        // Arrange
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Manager" });

        var user1 = await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        var user2 = await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });
        var user3 = await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 3 });

        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user1.UserId, Email = "asd@asd.com", PasswordHash = "123", PasswordSalt = "123" });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user2.UserId, Email = "dsa@dsa.com", PasswordHash = "321", PasswordSalt = "321" });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user3.UserId, Email = "singer@singer.com", PasswordHash = "111", PasswordSalt = "222" });

        await _profileRepository.CreateAsync(new UserProfileEntity() { UserId = 1, FirstName = "Levi", LastName = "Stark"});
        await _profileRepository.CreateAsync(new UserProfileEntity() { UserId = 2, FirstName = "Adelina", LastName = "Claesson" });
        await _profileRepository.CreateAsync(new UserProfileEntity() { UserId = 3, FirstName = "Richard", LastName = "Nikolausson" });


        // Act
        var result = await _userRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
        return result;
    }

    [Fact]
    public async Task CreateUserProfileShould_CreateNewUserProfile_ThenReturnIt()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithUserList();

        var userProfile = new UserProfileEntity()
        {
            UserId = 1,
            FirstName = "Levi",
            LastName = "Stark",
        };

        // Act
        var result = await _profileRepository.CreateAsync(userProfile);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.UserId == userProfile.UserId);
    }


    [Fact]
    public async Task ReadOneUserProfileShould_FindUserProfile_ThenReturnIt()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithUserList();

        // Act
        var result = await _profileRepository.ReadOneAsync(x => x.UserId == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.FirstName == "Levi");
    }

    [Fact]
    public async Task ReadAllUserProfilesShould_RetrieveAllProfiles_ThenReturnList()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithUserList();

        // Act
        var result = await _profileRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
    }

    [Fact]
    public async Task UpdateUserProfileShould_FindAndUpdateTheUserProfileWithGivenId_ThenReturnIt()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithUserList();

        var newUserProfile = new UserProfileEntity() { UserId = 1, FirstName = "Psalm" };

        // Act
        var result = await _profileRepository.UpdateAsync(x => x.UserId == 1, newUserProfile);
        var profileList = await _profileRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.FirstName == "Psalm");
        Assert.True(profileList.Count() == 3);
    }

    [Fact]
    public async Task DeleteUserShould_FindAndDeleteCorrectUser_ThenReturnTrue()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithUserList();
        var entityToDelete = await _profileRepository.ReadOneAsync(x => x.UserId == 1);

        // Act
        var result = await _profileRepository.DeleteAsync(x => x.UserId == 1, entityToDelete);
        var profileList = await _profileRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(profileList.Count() == 2);
    }
}
