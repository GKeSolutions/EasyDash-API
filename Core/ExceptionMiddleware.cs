using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Core
{
    public class ExceptionMiddleware
    {
        private RequestDelegate Next { get; set; }
        private Exception Exception { get; set; }
        private HttpContext HttpContext { get; set; }
        private HttpResponse HttpResponse { get; set; }
        private ILogger<ExceptionMiddleware> Logger { get; set; }

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            Logger = logger;
            Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                HttpContext = context;
                await Next(context);
            }
            catch (Exception ex)
            {
                Exception = ex;
                await HandleException();
            }
        }

        private async Task HandleException()
        {
            CreateApiErrorResponse();
            SetResponseStatusCode();
            await SetResponseBody();
        }

        private async Task SetResponseBody()
        {
            var result = JsonConvert.SerializeObject(new { message = Exception?.Message });
            Logger.LogError(result);
            await HttpResponse.WriteAsync(result);
        }

        private void SetResponseStatusCode()
        {
            HttpResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        private void CreateApiErrorResponse()
        {
            HttpResponse = HttpContext.Response;
            HttpResponse.ContentType = "application/json";
        }
    }
}
