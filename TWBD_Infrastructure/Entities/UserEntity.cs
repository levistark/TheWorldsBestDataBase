using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWBD_Infrastructure.Entities;
public class UserEntity
{
    [Key]
    public int UserId { get; set; }

    [Required, DataType(DataType.DateTime)]
    public DateTime RegistrationDate { get; set; }

    [Required, DataType(DataType.DateTime)]
    public DateTime LastLogin {  get; set; }

    [Required]
    public bool IsActive { get; set; } = false;

    [Required]
    [ForeignKey(nameof(UserRoleEntity))]
    public int RoleId { get; set; }

    public virtual UserRoleEntity Role { get; set; } = null!;
    public virtual UserProfileEntity UserProfile { get; set; } = null!;
    public virtual UserAuthenticationEntity UserAuthentication { get; set; } = null!;
}
