using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWBD_Infrastructure.Entities;

[Index("Category", Name = "UQ__ProductC__4BB73C32F717FC0A", IsUnique = true)]
public partial class ProductCategoryEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Category { get; set; } = null!;

    public int? ParentCategory { get; set; } = 0;

    [InverseProperty("ProductCategory")]
    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
