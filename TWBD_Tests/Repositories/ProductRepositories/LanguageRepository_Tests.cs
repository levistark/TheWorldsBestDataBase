using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories.ProductRepositories;
public class LanguageRepository_Tests
{
    private readonly static ProductDataContext _productDataContext =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly ManufacturerRepository _manufacturerRepository = new(_productDataContext);
    private readonly ProductRepository _productRepository = new(_productDataContext);
    private readonly LanguageRepository _languageRepository = new(_productDataContext);
    private readonly ProductCategoryRepository _categoryRepository = new(_productDataContext);
    private readonly ProductReviewRepository _reviewRepository = new(_productDataContext);
    private readonly ProductDescriptionRepository _descriptionRepository = new(_productDataContext);

    [Fact]
    public async Task AddSampleData()
    {
        await _languageRepository.CreateAsync(new LanguageEntity() { Language = "Svenska" });
        await _languageRepository.CreateAsync(new LanguageEntity() { Language = "English" });
        await _languageRepository.CreateAsync(new LanguageEntity() { Language = "Spanish" });
        
        // Act
        var result = await _languageRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
    }

    [Fact]
    public async Task CreateLanguageShould_CreateNewLanguage_ThenReturnIt()
    {
        // Arrange
        var language = new LanguageEntity() { Language = "English" };

        // Act
        var result = await _languageRepository.CreateAsync(language);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Language == "English");
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task ReadOneLanguageByIdShould_FindLanguage_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _languageRepository.ReadOneAsync(x => x.Id == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Language == "Svenska");
    }

    [Fact]
    public async Task ReadOneLanguageByLanguageShould_FindLanguage_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _languageRepository.ReadOneAsync(x => x.Language == "Svenska");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id == 1);
    }

    [Fact]
    public async Task ReadAllLanguagesShould_RetrieveAllLanguages_ThenReturnList()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _languageRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
        Assert.True(result.Count() == 3);
    }

    [Fact]
    public async Task UpdateLanguageByIdShould_FindAndUpdateTheLanguage_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();
        var existingAddress = await _languageRepository.ReadOneAsync(x => x.Id == 1);

        // Act
        existingAddress.Language = "Svengelska";
        var result = await _languageRepository.UpdateAsync(x => x.Id == 1, existingAddress);
        var languageList = await _languageRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Language == "Svengelska");
        Assert.True(languageList.Count() == 3);
    }

    [Fact]
    public async Task DeleteLanguageByIdShould_FindAndDeleteLanguage_ThenReturnTrue()
    {
        // Arrange
        await AddSampleData();
        var entityToDelete = await _languageRepository.ReadOneAsync(a => a.Id == 1);

        // Act
        var result = await _languageRepository.DeleteAsync(x => x.Id == 1, entityToDelete);
        var languageList = await _languageRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(!languageList.Any(b => b.Id == 1));
        Assert.True(!languageList.Any(b => b.Language == "Svenska"));
        Assert.True(languageList.Count() == 2);
    }

    [Fact]
    public async Task DeleteLanguageByLanguageShould_FindAndDeleteLanguage_ThenReturnTrue()
    {
        // Arrange
        await AddSampleData();
        var entityToDelete = await _languageRepository.ReadOneAsync(a => a.Language == "Svenska");

        // Act
        var result = await _languageRepository.DeleteAsync(x => x.Language == "Svenska", entityToDelete);
        var languageList = await _languageRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(!languageList.Any(b => b.Id == 1));
        Assert.True(!languageList.Any(b => b.Language == "Svenska"));
        Assert.True(languageList.Count() == 2);
    }

    [Fact]
    public async Task ExistingShould_CheckIfEntityExists_ThenReturnTrueIfItExists()
    {
        // Arrange
        await AddSampleData();

        // Act
        var entity = await _languageRepository.Existing(a => a.Id == 1);

        // Assert
        Assert.True(entity);
    }
}
