using Microsoft.EntityFrameworkCore;
using TWBD_Domain.DTOs.Models.Product;
using TWBD_Domain.Services.ProductServices;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services.Product;
public class DescriptionService_Tests
{
    private readonly static ProductDataContext _productDataContext =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly ProductDescriptionRepository _descriptionRepository = new(_productDataContext);
    private readonly LanguageRepository _languageRepository = new(_productDataContext);

    [Fact]
    public async Task AddSampleData()
    {
        LanguageService _languageService = new LanguageService(_languageRepository);

        var description1 = await _descriptionRepository.CreateAsync(new ProductDescriptionEntity()
        {
            Description = "a",
            Specifications = "b",
            Language = new LanguageEntity()
            {
                Language = "Svenska"
            },
            LanguageId = 1,
            Ingress = "",
            ArticleNumberNavigation = new ProductEntity()
            {
                ArticleNumber = "A1",
                Title = "iPhone",
                Manufacturer = new ManufacturerEntity()
                {
                    Manufacturer = "Apple"
                },
                ManufacturerId = 1,
                ProductCategory = new ProductCategoryEntity()
                {
                    Category = "Cellphones",
                    ParentCategory = 0,
                },
                Price = 10,
            },
            ArticleNumber = "A1",
        });

        var description2 = await _descriptionRepository.CreateAsync(new ProductDescriptionEntity()
        {
            Description = "b",
            Specifications = "c",
            LanguageId = 1,
            Ingress = "",
            ArticleNumberNavigation = new ProductEntity()
            {
                ArticleNumber = "A2",
                Title = "iPhone Pro",
                ManufacturerId = 1,
                Price = 10,
            },
            ArticleNumber = "A2",
        });

        var descriptionList = await _descriptionRepository.ReadAllAsync();

        Assert.NotNull(descriptionList);
        Assert.True(descriptionList.Any());
        Assert.Contains(descriptionList, x => x.ArticleNumber == "A1");
        Assert.Contains(descriptionList, x => x.ArticleNumber == "A2");
        Assert.True(descriptionList.Count() > 1);
    }

    [Fact]
    public async Task GetAllDescriptionsShould_RetrieveAllDescriptions_ThenReturnAsModelList()
    {
        // Arrange
        LanguageService _languageService = new LanguageService(_languageRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);
        await AddSampleData();

        // Act
        var descriptionList = await _descriptionService.GetAllDescriptions();

        // Assert
        Assert.NotNull(descriptionList);
        Assert.True(descriptionList.Any());
        Assert.Contains(descriptionList, x => x.ArticleNumber == "A1");
        Assert.Contains(descriptionList, x => x.ArticleNumber == "A2");
        Assert.True(descriptionList.Count() > 1);
    }

    [Fact]
    public async Task GetDescriptionsByPropertyShould_FindAssociatedDescriptions_ThenReturnList()
    {
        // Arrange
        LanguageService _languageService = new LanguageService(_languageRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);
        await AddSampleData();

        // Act
        var descriptionList = await _descriptionService.GetDescriptionsByProperty(x => x.LanguageId == 1);

        // Assert
        Assert.NotNull(descriptionList);
        Assert.True(descriptionList.Any());
        Assert.True(descriptionList.Count() == 2);
    }

    [Fact]
    public async Task CreateDescriptionShould_CreateNewDescriptionInDb_ThenReturnItAsModel()
    {
        // Arrange
        LanguageService _languageService = new LanguageService(_languageRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);

        // Act
        var result = await _descriptionService.CreateDescription(new DescriptionModel()
        {
            Description = "Telefon",
            Specifications = "Svart",
            Language = "Svenska",
            Ingress = "blabla",
            ArticleNumber = "A1"
        });

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Description == "Telefon");
    }
    
    [Fact]
    public async Task UpdateDescriptionShould_UpdateDescription_ThenReturnWithUpdatedModel()
    {
        // Arrange
        LanguageService _languageService = new LanguageService(_languageRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);
        await AddSampleData();

        // Act
        var result = await _descriptionService.UpdateDescription(new DescriptionModel()
        {
            Description = "awesome gear",
            Specifications = "1 terrabyte",
            Ingress = "asdjhdfkjsdh",
            Language = "Svenska",
            ArticleNumber = "A1"
        });


        // Assert
        Assert.NotNull(result);
        Assert.True(result.Description == "awesome gear");
    }

    [Fact]
    public async Task DeleteDescriptionShould_DeleteDescription_ThenReturnTrue()
    {
        // Arrange
        LanguageService _languageService = new LanguageService(_languageRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);
        await AddSampleData();

        // Act
        var result = await _descriptionService.DeleteDescription(new DescriptionModel()
        {
            Description = "awesome gear",
            Specifications = "1 terrabyte",
            Ingress = "asdjhdfkjsdh",
            Language = "Svenska",
            ArticleNumber = "A1"
        });
        var descriptionList = await _descriptionService.GetAllDescriptions();

        // Assert
        Assert.True(result);
        Assert.True(descriptionList.Count() == 1);
    }
}
