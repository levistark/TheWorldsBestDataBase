using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWBD_Infrastructure.Entities;
public class UserAddressEntity
{
    [Key]
    public int AddressId { get; set; }

    [Required, MaxLength(50)]
    public string City { get; set; } = null!;

    [Required, MaxLength(50)]
    public string StreetName { get; set; } = null!;

    [Required, MaxLength(50)]
    public string PostalCode { get; set; } = null!;

    public virtual ICollection<UserProfileEntity> UserProfiles { get; set; } = new List<UserProfileEntity>();
}
