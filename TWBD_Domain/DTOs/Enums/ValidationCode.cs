﻿namespace TWBD_Domain.DTOs.Enums;
public enum ValidationCode
{
    CREATED = 0,
    READ = 1,
    UPDATED = 2,
    NOT_FOUND = 3,
    ALREADY_EXISTS = 4,
    INVALID_PASSWORD = 5,
    INVALID_EMAIL = 6,
    FAILED = 7,
    PASSWORD_NOT_MATCH = 8,
}
