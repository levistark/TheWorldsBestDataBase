namespace TWBD_Domain.DTOs.Models.Product;
public class DescriptionModel
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Specifications { get; set; } = null!;
    public string Language { get; set; } = null!;
    public string? Ingress { get; set; }
    public string ArticleNumber { get; set; } = null!;
}
