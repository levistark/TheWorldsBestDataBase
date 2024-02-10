using Microsoft.EntityFrameworkCore;
using TWBD_Domain.Services.ProductServices;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services.Product;
public class LanguageService_Tests
{
    private readonly static ProductDataContext _productDataContext =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly LanguageRepository _languageRepository = new(_productDataContext);

    private async void AddSampleData()
    {
        await _languageRepository.CreateAsync(new LanguageEntity() { Language = "English" });
        await _languageRepository.CreateAsync(new LanguageEntity() { Language = "Swedish"});
        await _languageRepository.CreateAsync(new LanguageEntity() { Language = "Spanish" });
    }

    //[Fact]
    //public async Task CreateLanguageShould_AddNewLanguageToDb_ThenReturnAsModel()
    //{
    //    // Arrange
    //    LanguageService _languageService = new LanguageService(_languageRepository);
    //
    //    // Act
    //    // Assert
    //
    //}

    [Fact]
    public async Task GetLanguageIdByNameShould_FindLanguageId_ThenReturnIt()
    {
        // Arrange
        LanguageService _languageService = new LanguageService(_languageRepository);
        AddSampleData();

        // Act
        var result = await _languageService.GetLanguageId("German");
        var name = await _languageService.GetLanguageName(result);

        // Assert
        Assert.True(result > 0);
        Assert.True(name == "German");
    }

    [Fact]
    public async Task GetLanguageByIdShould_FindLanguageName_ThenReturnIt()
    {
        // Arrange
        LanguageService _languageService = new LanguageService(_languageRepository);
        AddSampleData();

        // Act
        var result = await _languageService.GetLanguageName(1);

        // Assert
        Assert.True(result == "English");
    }
}
