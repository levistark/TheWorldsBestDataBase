using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories.ProductRepositories;
public class ProductCategoryRepository_Tests
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
        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Datorer", ParentCategory = 2 });
        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Elektronik" });
        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Mobiltelefoner" });

        // Act
        var result = await _categoryRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
    }

    [Fact]
    public async Task CreateCategoryShould_CreateNewCategory_ThenReturnIt()
    {
        // Arrange
        var category = new ProductCategoryEntity() { Category = "Datorer", ParentCategory = 2 };

        // Act
        var result = await _categoryRepository.CreateAsync(category);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Category == "Datorer");
    }

    [Fact]
    public async Task ReadOneCategoryByIdShould_FindCategory_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _categoryRepository.ReadOneAsync(x => x.Id == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Category == "Datorer");
        Assert.True(result.ParentCategory == 2);
    }

    [Fact]
    public async Task ReadOneCategoryByCategoryShould_FindCategory_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _categoryRepository.ReadOneAsync(x => x.Category == "Datorer");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id == 1);
    }

    [Fact]
    public async Task ReadAllCategoriesShould_RetrieveAllCategories_ThenReturnList()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _categoryRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
        Assert.True(result.Count() == 3);
    }

    [Fact]
    public async Task UpdateCategoryByIdShould_FindAndUpdateTehCategory_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();
        var existingCategory = await _categoryRepository.ReadOneAsync(x => x.Id == 1);

        // Act
        existingCategory.Category = "Computers";
        var result = await _categoryRepository.UpdateAsync(x => x.Id == 1, existingCategory);
        var categoryList = await _categoryRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Category == "Computers");
        Assert.True(categoryList.Count() == 3);
    }

    [Fact]
    public async Task DeleteCategoryByIdShould_FindAndDeleteCategory_ThenReturnTrue()
    {
        // Arrange
        await AddSampleData();
        var entityToDelete = await _categoryRepository.ReadOneAsync(a => a.Id == 1);

        // Act
        var result = await _categoryRepository.DeleteAsync(x => x.Id == 1, entityToDelete);
        var categoryList = await _categoryRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(!categoryList.Any(b => b.Id == 1));
        Assert.True(!categoryList.Any(b => b.Category == "Datorer"));
        Assert.True(categoryList.Count() == 2);
    }

    [Fact]
    public async Task DeleteCategoryByCategoryShould_FindAndDeleteCategory_ThenReturnTrue()
    {
        // Arrange
        await AddSampleData();
        var entityToDelete = await _categoryRepository.ReadOneAsync(a => a.Category == "Datorer");

        // Act
        var result = await _categoryRepository.DeleteAsync(x => x.Category == "Datorer", entityToDelete);
        var categoryList = await _categoryRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(!categoryList.Any(b => b.Id == 1));
        Assert.True(!categoryList.Any(b => b.Category == "Datorer"));
        Assert.True(categoryList.Count() == 2);
    }

    [Fact]
    public async Task ExistingShould_CheckIfEntityExists_ThenReturnTrueIfItExists()
    {
        // Arrange
        await AddSampleData();

        // Act
        var entity = await _categoryRepository.Existing(a => a.Id == 1);

        // Assert
        Assert.True(entity);
    }
}
