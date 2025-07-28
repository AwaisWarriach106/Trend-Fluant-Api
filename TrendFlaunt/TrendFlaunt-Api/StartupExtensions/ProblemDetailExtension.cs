using Microsoft.Extensions.Diagnostics.HealthChecks;
using TrendFlaunt.Domain.Common;

namespace TrendFlaunt_Api.StartupExtensions;

public static class ProblemDetailExtension
{
    public static IResult ToProblemDetails<T>(this ServiceResponse<T> result)
    {
        if (result.Success)
        {
            throw new InvalidOperationException("Can't convert success result to problem");
        }

        int statusCode = StatusCodes.Status400BadRequest;
        string title = "Bad Request";

        switch (result.ErrorCode)
        {
            case ErrorCode.ValidationError:
                statusCode = StatusCodes.Status400BadRequest;
                title = "Validation Error";
                break;
            case ErrorCode.NotFound:
                statusCode = StatusCodes.Status404NotFound;
                title = "Not Found";
                break;
            case ErrorCode.UnauthorizedAccess:
                statusCode = StatusCodes.Status401Unauthorized;
                title = "Unauthorized";
                break;
            case ErrorCode.Error:
                statusCode = StatusCodes.Status500InternalServerError;
                title = "Internal Server Error";
                break;
            case ErrorCode.AlreadyExists:
                statusCode = StatusCodes.Status409Conflict;
                title = "Conflict";
                break;
            case ErrorCode.Locked:
                statusCode = StatusCodes.Status423Locked;
                title = "Locked";
                break;
            default:
                break;
        }

        return Results.Problem(
            statusCode: statusCode,
            title: title,
            extensions: new Dictionary<string, object>
            {
            { "Errors", new object[]{ result.Message } }
            });
    }
    public static HealthCheckResult HealthProblemDetails<T>(this ServiceResponse<T> result)
    {
        if (result.Success)
        {
            throw new InvalidOperationException("Cannot convert a successful result to a HealthCheckResult.");
        }

        HealthStatus overallStatus = HealthStatus.Unhealthy;
        var data = new Dictionary<string, object>
        {
            { "Message", result.Message },
            { "ErrorCode", result.ErrorCode.ToString() }
        };

        if (!string.IsNullOrEmpty(result.ErrorMessage.ToString()))
        {
            data["ErrorMessage"] = result.ErrorMessage;
        }
        return new HealthCheckResult(
            overallStatus,
            "Health check indicates an issue.",
            null,
            data
        );
    }
}
