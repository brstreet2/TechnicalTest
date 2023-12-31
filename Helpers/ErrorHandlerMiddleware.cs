﻿namespace TechnicalTest.Helpers;

using System.Net;
using System.Text.Json;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    public ErrorHandlerMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (ex)
            {
                case AppException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest; break;
                case KeyNotFoundException e:
                    response.StatusCode= (int)HttpStatusCode.NotFound; break;
                default:
                    response.StatusCode= (int)HttpStatusCode.InternalServerError; break;
            }

            var result = JsonSerializer.Serialize(new { message = ex?.Message });
            await response.WriteAsync(result);
        }
    }
}
