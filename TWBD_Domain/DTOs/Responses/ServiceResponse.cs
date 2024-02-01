using TWBD_Domain.DTOs.Enums;
using TWBD_Domain.Interfaces;

namespace TWBD_Domain.DTOs.Responses;
public class ServiceResponse : IServiceResponse
{
    public bool Success { get; set; } = false;
    public string? Message { get; set; }
    public ServiceCode Code { get; set; }
    public object ReturnObject { get; set; } = null!;
}
