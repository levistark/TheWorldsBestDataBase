using Microsoft.EntityFrameworkCore;
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
        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Electronics" });
        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Computers", ParentCategory = 1 });
        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Laptops" });
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
