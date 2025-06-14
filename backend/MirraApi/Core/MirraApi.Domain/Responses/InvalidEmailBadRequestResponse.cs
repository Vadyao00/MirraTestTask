﻿namespace MirraApi.Models.Responses;

public sealed class InvalidEmailBadRequestResponse : ApiBadRequestResponse
{
    public InvalidEmailBadRequestResponse() : base("User with this email already exists.")
    {
    }
}