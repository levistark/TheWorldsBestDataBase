using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWBD_Infrastructure.Entities;

[Index("Language", Name = "UQ__Language__8B12195F0B821C3E", IsUnique = true)]
public partial class LanguageEntity
{
    [Key]
    public int Id { get; set; }

    [Column("LanguageType")]
    [StringLength(20)]
    [Unicode(false)]
    public string Language { get; set; } = null!;

    [InverseProperty("Language")]
    public virtual ICollection<ProductDescriptionEntity> ProductDescriptions { get; set; } = new List<ProductDescriptionEntity>();
}
