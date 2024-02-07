using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWBD_Infrastructure.Entities;

public partial class ProductEntity
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string ArticleNumber { get; set; } = null!;

    [StringLength(50)]
    public string Title { get; set; } = null!;

    public int ManufacturerId { get; set; }

    public int ProductCategoryId { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [Column(TypeName = "money")]
    public decimal? DiscountPrice { get; set; }

    [ForeignKey("ManufacturerId")]
    [InverseProperty("Products")]
    public virtual ManufacturerEntity Manufacturer { get; set; } = null!;

    [ForeignKey("ProductCategoryId")]
    [InverseProperty("Products")]
    public virtual ProductCategoryEntity ProductCategory { get; set; } = null!;

    [InverseProperty("ArticleNumberNavigation")]
    public virtual ICollection<ProductDescriptionEntity>? ProductDescriptions { get; set; } = new List<ProductDescriptionEntity>();

    [InverseProperty("ArticleNumberNavigation")]
    public virtual ICollection<ProductReviewEntity>? ProductReviews { get; set; } = new List<ProductReviewEntity>();
}
