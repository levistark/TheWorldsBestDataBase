using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories;
public class PhoneNumberRepository_Tests
{
    private readonly static UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly RoleRepository _roleRepository = new(_userDataContext);
    private readonly UserRepository _userRepository = new(_userDataContext);
    private readonly AuthenticationRepository _uaRepository = new(_userDataContext);
    private readonly ProfileRepository _profileRepository = new(_userDataContext);
    private readonly AddressRepository _addressRepository = new(_userDataContext);
    private readonly PhoneNumberRepository _phoneNumberRepository = new(_userDataContext);

    [Fact]
    public async Task<IEnumerable<UserAddressEntity>> AddSampleDataShould_AddDataToTables_ReturnWithAddressList()
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

        await _profileRepository.CreateAsync(new UserProfileEntity() { UserId = 1, FirstName = "Levi", LastName = "Stark" });
        await _profileRepository.CreateAsync(new UserProfileEntity() { UserId = 2, FirstName = "Stefan", LastName = "Svensson" });
        await _profileRepository.CreateAsync(new UserProfileEntity() { UserId = 3, FirstName = "Richard", LastName = "Nikolausson" });

        await _addressRepository.CreateAsync(new UserAddressEntity() { City = "Helsingborg", StreetName = "Hjälmshultsgatan 11", PostalCode = "25431", UserId = 1 });
        await _addressRepository.CreateAsync(new UserAddressEntity() { City = "Sävsjö", StreetName = "Skogsrundan 31", PostalCode = "57633", UserId = 2 });
        await _addressRepository.CreateAsync(new UserAddressEntity() { City = "Sävsjö", StreetName = "Högaholmsgatan 3", PostalCode = "57632", UserId = 3 });

        await _phoneNumberRepository.CreateAsync(new UserPhoneNumberEntity() { PhoneNumber = "070-1234567", UserId = 1 });
        await _phoneNumberRepository.CreateAsync(new UserPhoneNumberEntity() { PhoneNumber = "076-9876543", UserId = 2 });
        await _phoneNumberRepository.CreateAsync(new UserPhoneNumberEntity() { PhoneNumber = "073-9182736", UserId = 3 });

        // Act
        var result = await _addressRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
        return result;
    }

    [Fact]
    public async Task CreateUserPhoneNumberShould_CreateNewUserPhoneNumber_ThenReturnIt()
    {
        // Arrange
        var phone = new UserPhoneNumberEntity()
        {
            PhoneNumber = "070-1234567",
            UserId = 1
        };

        // Act
        var result = await _phoneNumberRepository.CreateAsync(phone);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.PhoneNumber == "070-1234567");
    }

    [Fact]
    public async Task ReadOneUserPhoneNumberShould_FindUserPhoneNumber_ThenReturnIt()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();

        // Act
        var result = await _phoneNumberRepository.ReadOneAsync(x => x.PhoneId == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.PhoneNumber == "070-1234567");
    }

    [Fact]
    public async Task ReadAllUserPhoneNumbersShould_RetrieveAllUserPhoneNumbers_ThenReturnList()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();

        // Act
        var result = await _phoneNumberRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
    }

    [Fact]
    public async Task UpdateUserPhoneNumberShould_FindAndUpdateTheUserPhoneNumberWithGivenId_ThenReturnIt()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();
        var existingPhone = await _phoneNumberRepository.ReadOneAsync(x => x.UserId == 1);

        // Act
        existingPhone.PhoneNumber = "123";
        var result = await _phoneNumberRepository.UpdateAsync(x => x.UserId == 1, existingPhone);
        var addressList = await _phoneNumberRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.PhoneNumber == "123");
        Assert.True(addressList.Count() == 3);
    }

    [Fact]
    public async Task DeleteUserPhoneNumberShould_FindAndDeleteCorrectUserPhoneNumber_ThenReturnTrue()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();
        var entityToDelete = await _phoneNumberRepository.ReadOneAsync(a => a.UserId == 1);

        // Act
        var result = await _phoneNumberRepository.DeleteAsync(x => x.PhoneNumber == "070-1234567", entityToDelete);
        var addressList = await _phoneNumberRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(!addressList.Any(b => b.PhoneNumber == "070-1234567"));
        Assert.True(addressList.Count() == 2);
    }
}
