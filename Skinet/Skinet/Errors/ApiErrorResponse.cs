﻿namespace Skinet.Errors
{
    public class ApiErrorResponse(int statusCode,string message,string? details)
    {
        public int StatusCode { get; set; }=statusCode;

        public string ErrorMessage { get; set; } = message;

        public string? Details { get; set; }=details;
    }
}
