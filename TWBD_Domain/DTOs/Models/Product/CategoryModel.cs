using System.ComponentModel.DataAnnotations;

namespace TWBD_Domain.DTOs.Models.Product;
public class CategoryModel
{
    public int Id { get; set; }
    public string Category { get; set; } = null!;
    public string? ParentCategory { get; set; }
}
