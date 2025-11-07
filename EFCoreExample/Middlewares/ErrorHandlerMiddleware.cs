using EFCoreExample.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EFCoreExample.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;


        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                //error = error.FlattenEx();

                var problemDetails = new ProblemDetails();
                problemDetails.Extensions.Add("traceId", Activity.Current?.Id ?? context.TraceIdentifier);

                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case EFCoreExampleException:
                        problemDetails.Type = ((EFCoreExampleException)error).Code.ToString();
                        problemDetails.Title = error.Message;
                        problemDetails.Detail = ((EFCoreExampleException)error).Details;

                        problemDetails.Status = error switch
                        {
                            DuplicateResourceException => StatusCodes.Status409Conflict,
                            NegativePriceException => StatusCodes.Status400BadRequest,
                            OrderItemNotFoundException => StatusCodes.Status400BadRequest,
                            OrderNotFoundException => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        };
                        break;

                    default:
                        // Unhandled error
                        // We don't expose any details of unhandled errors
                        // because we are not certain of what data it might contain.
                        problemDetails.Type = ExceptionCode.E_UNEXPECTED_ERROR.ToString();
                        problemDetails.Title = "An unexpected error occurred.";
                        problemDetails.Detail = "";
                        problemDetails.Status = StatusCodes.Status500InternalServerError;
                        break;
                }

                // TODO We could log specifically to an error log for unexpected errors?

                response.StatusCode = (int)problemDetails.Status;

                //_logger.LogError(error.ToString());

                await response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
