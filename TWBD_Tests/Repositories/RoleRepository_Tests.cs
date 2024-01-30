using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories;
public class RoleRepository_Tests
{
    private readonly UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public async Task CreateEntityShould_CreateNewEntity_ThenReturnWithCreatedEntity()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        var roleEntity = new UserRoleEntity()
        {
            RoleType = "Free"
        };

        // Act
        var result = await _roleRepository.CreateAsync(roleEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.RoleType, roleEntity.RoleType);
    }

    [Fact]
    public async Task ReadOneShould_RetriveOneEntity_ThenReturnEntity()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        var sampleEntity = CreateEntityShould_CreateNewEntity_ThenReturnWithCreatedEntity();

        // Act
        var result = await _roleRepository.ReadOneAsync(x => x.RoleType == "Free");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.RoleType == "Free");
    
    }

    [Fact]
    public async Task ReadAllShould_RetrieveAllTableEntities_ThenReturnList()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        var sampleEntity = CreateEntityShould_CreateNewEntity_ThenReturnWithCreatedEntity();

        // Act
        var result = await _roleRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() == 1);
    }

    [Fact]
    public async Task UpdateShould_FindAndUpdateTheGivenEntity_ThenReturnUpdatedEntity()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        var sampleEntity = CreateEntityShould_CreateNewEntity_ThenReturnWithCreatedEntity();
        var newEntity = new UserRoleEntity() { RoleType = "Partner" };

        // Act
        var result = await _roleRepository.UpdateAsync(x => x.RoleType == "Free", newEntity);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.RoleType == "Partner");

    }
}
