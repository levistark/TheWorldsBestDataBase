using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TWBD_Infrastructure.Entities;

[Index(nameof(RoleType), IsUnique = true)]
public class UserRoleEntity
{
    [Key, Required]
    public int RoleId { get; set; }

    [Required]
    [StringLength(50)]
    public string RoleType { get; set; } = null!;

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
