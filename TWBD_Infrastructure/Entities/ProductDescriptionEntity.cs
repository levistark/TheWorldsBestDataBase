using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWBD_Infrastructure.Entities;

[PrimaryKey("ArticleNumber", "LanguageId")]
public partial class ProductDescriptionEntity
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string Specifications { get; set; } = null!;

    [StringLength(200)]
    public string? Ingress { get; set; }

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string ArticleNumber { get; set; } = null!;

    [Key]
    public int LanguageId { get; set; }

    [ForeignKey("ArticleNumber")]
    [InverseProperty("ProductDescriptions")]
    public virtual ProductEntity ArticleNumberNavigation { get; set; } = null!;

    [ForeignKey("LanguageId")]
    [InverseProperty("ProductDescriptions")]
    public virtual LanguageEntity Language { get; set; } = null!;
}
