namespace TWBD_Domain.DTOs.Models.Product;
public class ProductUpdateModel
{
    public string ArticleNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public CategoryModel Category { get; set; } = null!;
}
