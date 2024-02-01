using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWBD_Infrastructure.Entities;

[Index(nameof(Email), IsUnique = true)]
public class UserAuthenticationEntity
{
    [Key, Required, ForeignKey(nameof(UserEntity))]
    public int UserId { get; set; }

    [Required, MaxLength(200)]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}
