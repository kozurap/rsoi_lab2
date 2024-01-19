using Kernel.Exceptions;
using System.Net;
using System.Text.Json;

namespace Gateway
{
    public class HandleErrorsMiddleware
    {
        private readonly RequestDelegate _next;

        public HandleErrorsMiddleware(RequestDelegate next)
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
                Console.WriteLine(error.Message + "\\n" + error.StackTrace);
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    UnauthorizedAccessException => (int)HttpStatusCode.Forbidden,
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    ServiceUnavaliableException => (int)HttpStatusCode.ServiceUnavailable,
                    not null => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var result = JsonSerializer.Serialize(new { message = error?.Message ?? "Error" });
                await response.WriteAsync(result);
            }
        }
    }
}
