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

    public string ProfileImage { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
    public virtual UserAddressEntity Address { get; set; } = null!;
    public virtual UserPhoneNumberEntity PhoneNumber { get; set; } = null!;
}
