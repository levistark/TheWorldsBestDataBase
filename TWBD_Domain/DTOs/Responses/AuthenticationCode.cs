namespace TWBD_Domain.DTOs.Responses;
public enum AuthenticationCode
{
    CREATED = 0,
    READ = 1,
    UPDATED = 2,
    NOT_FOUND = 3,
    ALREADY_EXISTS = 4,
    INVALID_PASSWORD = 5,
    INVALID_EMAIL = 6,
    FAILED = 7
}
