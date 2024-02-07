using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWBD_Infrastructure.Entities;

[Index("Manufacturer", Name = "UQ__Manufact__D194335A9A61C47A", IsUnique = true)]
public partial class ManufacturerEntity
{
    [Key]
    public int Id { get; set; }

    [Column("Manufacturer")]
    [StringLength(50)]
    public string Manufacturer { get; set; } = null!;

    [InverseProperty("Manufacturer")]
    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
