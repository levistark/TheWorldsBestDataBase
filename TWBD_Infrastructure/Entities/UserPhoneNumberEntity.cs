using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWBD_Infrastructure.Entities;
public class UserPhoneNumberEntity
{
    [Key, Required]
    public int PhoneId { get; set; }

    [Required, MaxLength(30)]
    public string PhoneNumber { get; set; } = null!;

    [Required, ForeignKey(nameof(UserProfileEntity))]
    public int UserId { get; set; }

    public virtual ICollection<UserProfileEntity> UserProfiles { get; set; } = new HashSet<UserProfileEntity>();    
}
