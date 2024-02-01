using TWBD_Domain.DTOs.Responses;

namespace TWBD_Domain.Interfaces;
public interface IServiceResponse
{
    bool Success { get; set; }
    string? Message { get; set; }
    enum ServiceCode;
}
