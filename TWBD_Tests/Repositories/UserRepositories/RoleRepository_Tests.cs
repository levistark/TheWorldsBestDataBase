using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories.UserRepositories;
public class RoleRepository_Tests
{
    private readonly UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public async Task<UserRoleEntity> CreateEntityShould_CreateNewEntity_ThenReturnWithCreatedEntity()
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
        return result;
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
        await CreateEntityShould_CreateNewEntity_ThenReturnWithCreatedEntity();
        var newEntity = new UserRoleEntity() { RoleId = 1, RoleType = "Partner" };

        // Act
        var result = await _roleRepository.UpdateAsync(x => x.RoleType == "Free", newEntity);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.RoleType == "Partner");
    }

    [Fact]
    public async Task DeleteEntityShould_FindAndDeleteCorrectEntity_ThenReturnTrue()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        var sampleEntity = await CreateEntityShould_CreateNewEntity_ThenReturnWithCreatedEntity();
        var entityToDelete = await _roleRepository.ReadOneAsync(x => x.RoleType == "Free");

        // Act
        var result = await _roleRepository.DeleteAsync(x => x.RoleId == entityToDelete.RoleId, entityToDelete);
        var roleList = await _roleRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(roleList.Count() == 0);
    }

    [Fact]
    public async Task ExistingShould_CheckIfEntityExists_ThenReturnTrueIfItExists()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        await CreateEntityShould_CreateNewEntity_ThenReturnWithCreatedEntity();

        // Act
        var entity = await _roleRepository.Existing(a => a.RoleType == "Free");

        // Assert
        Assert.True(entity);
    }
}
