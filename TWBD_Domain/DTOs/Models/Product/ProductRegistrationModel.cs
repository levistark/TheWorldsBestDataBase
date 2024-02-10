namespace TWBD_Domain.DTOs.Models.Product;
public class ProductRegistrationModel
{
    public string ArticleNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Manufacturer {  get; set; } = null!;
    public string Description { get; set; } = null!;
    public string DescriptionLanguage { get; set; } = null!;
    public string Specifications { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string Category { get; set; } = null!;
    public string? ParentCategory { get; set; }
}
