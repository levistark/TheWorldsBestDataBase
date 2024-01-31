﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TWBD_Domain.DTOs;
public class UserRegistrationModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string City { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = false;
    public string Role { get; set; } = null!;
    public string? ProfileImage { get; set; }

}
