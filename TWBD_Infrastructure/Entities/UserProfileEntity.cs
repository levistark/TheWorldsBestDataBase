using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWBD_Infrastructure.Entities;
public class UserProfileEntity
{
    [Key, Required, ForeignKey(nameof(UserEntity))]
    public int UserId { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = null!;

    [MaxLength(50)]
    public string? PhoneNumber { get; set; }

    [Required]
    [ForeignKey(nameof(UserAddressEntity))]
    public int AddressId { get; set; }

    public string? ProfileImage { get; set; }

    public virtual UserEntity User { get; set; } = null!;
    public virtual UserAddressEntity Address { get; set; } = null!;
}
