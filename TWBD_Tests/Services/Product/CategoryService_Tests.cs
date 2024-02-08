using Microsoft.EntityFrameworkCore;
using TWBD_Domain.DTOs.Models.Product;
using TWBD_Domain.Services.ProductServices;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services.Product;
public class CategoryService_Tests
{
    private readonly static ProductDataContext _productDataContext =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly ProductCategoryRepository _categoryRepository = new(_productDataContext);

    private async void AddSampleData()
    {
        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Electronics", ParentCategory = 0});
        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Computers", ParentCategory = 1 });
        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Laptops" });
    }

    [Fact]
    public async Task CreateCategoryShould_AddNewCategoryToDb_ThenReturnCategoryModel()
    {
        // Arrange
        CategoryService _categoryService = new CategoryService(_categoryRepository);

        // Act
        var result = await _categoryService.CreateCategory(new CategoryRegistrationModel("Phones", "Electronics"));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Category == "Phones");
        Assert.True(result.ParentCategory == "Electronics");
    }

    [Fact]
    public async Task UpdateCategoryShould_UpdateExistingCategoryEntity_ThenReturnIt()
    {
        // Arrange
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        AddSampleData();

        // Act
        var result = await _categoryService.UpdateCategory(new CategoryModel()
        {
            Id = 1,
            Category = "Accessories",
            ParentCategory = "Other"
        });
        CategoryModel? model = result.ReturnObject as CategoryModel;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Code == TWBD_Domain.DTOs.Enums.ServiceCode.UPDATED);
        Assert.NotNull(model);
        Assert.True(model.Category == "Accessories");
        Assert.True(model.ParentCategory == "Other");
        Assert.True(model.Id == 1);
    }

    [Fact]
    public async Task DeleteCategoryByIdShould_DeleteExistingCategoryEntity_ThenReturnTrue()
    {
        // Arrange
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        AddSampleData();

        // Act
        var result = await _categoryService.DeleteCategoryById(1);
        var categoryList = await _categoryRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Code == TWBD_Domain.DTOs.Enums.ServiceCode.DELETED);
        Assert.True(!categoryList.Any(x => x.Id == 1));
    }

    [Fact]
    public async Task GetCategoryIdByNameShould_FindCategoryId_ThenReturnIt()
    {
        // Arrange
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        AddSampleData();

        // Act
        var result = await _categoryService.GetCategoryId("Electronics");

        // Assert
        Assert.True(result == 1);
    }

    [Fact]
    public async Task GetCategoryNameByIdShould_FindCategoryName_ThenReturnIt()
    {
        // Arrange
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        AddSampleData();

        // Act
        var result = await _categoryService.GetCategoryName(1);

        // Assert
        Assert.True(result == "Electronics");
    }

    [Fact]
    public async Task GetParentCategoryByIdShould_FindParentCategoryId_ThenReturnIt()
    {
        // Arrange
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        AddSampleData();

        // Act
        var parentId = await _categoryService.GetParentCategoryId(2);
        var parentName = await _categoryService.GetCategoryName(parentId);

        // Assert
        Assert.True(parentName == "Electronics");
    }
}
