using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ASP.NET.Core.WebAPI.Helpers.Middlewares
{
    /// <summary>
    /// This middleware component logs all incoming HTTP requests and outgoing HTTP responses.
    /// </summary>
    public class RequestResponseLogger
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<RequestResponseLogger> _logger;

        public RequestResponseLogger(RequestDelegate requestDelegate, ILogger<RequestResponseLogger> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            JObject request = await FormatRequest(context.Request);
            _logger.LogDebug("{@data}", new { Category = "REQUEST", Data = request });

            Stream originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            await _requestDelegate(context);

            JObject response = await FormatResponse(context.Response);
            _logger.LogDebug("{@data}", new { Category = "RESPONSE", Data = response });
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static async Task<JObject> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            request.Body.Seek(0, SeekOrigin.Begin);
            var bodytext = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);

            return JObject.FromObject(new
            {
                request.Method,
                URL = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                Body = bodytext,
                request.Headers
            });
        }

        private static async Task<JObject> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return JObject.FromObject(new
            {
                Request = $"{response.HttpContext.Request.Scheme}://{response.HttpContext.Request.Host}{response.HttpContext.Request.Path}{response.HttpContext.Request.QueryString}",
                response.StatusCode,
                response.ContentType,
                Response = bodyText
            });
        }
    }
}