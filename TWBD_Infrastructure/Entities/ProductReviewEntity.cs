using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWBD_Infrastructure.Entities;

public partial class ProductReviewEntity
{
    [Key]
    public int Id { get; set; }

    public string? Review { get; set; }

    public byte Rating { get; set; }

    [StringLength(20)]
    public string? Author { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string ArticleNumber { get; set; } = null!;

    [ForeignKey("ArticleNumber")]
    [InverseProperty("ProductReviews")]
    public virtual ProductEntity ArticleNumberNavigation { get; set; } = null!;
}
