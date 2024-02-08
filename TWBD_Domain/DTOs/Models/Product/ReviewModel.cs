using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TWBD_Domain.DTOs.Models.Product;
public class ReviewModel
{
    public int Id { get; set; }
    public string? Review { get; set; }
    public byte Rating { get; set; }
    public string? Author { get; set; }
    public string ArticleNumber { get; set; } = null!;
}
