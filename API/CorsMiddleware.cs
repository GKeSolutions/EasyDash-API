using Newtonsoft.Json;
using System.Net;

namespace API
{
    public class CorsMiddleware
    {
        private RequestDelegate Next { get; set; }
        private Exception Exception { get; set; }
        private HttpContext HttpContext { get; set; }
        private HttpResponse HttpResponse { get; set; }
        private ILogger<CorsMiddleware> Logger { get; set; }

        public CorsMiddleware(RequestDelegate next, ILogger<CorsMiddleware> logger)
        {
            Next = next;
            Logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                HttpContext = context;
                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "http://3e-dev-wapi:5010");
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    context.Response.StatusCode = 204;
                }
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
