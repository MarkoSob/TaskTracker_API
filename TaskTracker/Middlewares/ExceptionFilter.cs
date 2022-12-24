using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskTracker.Core.Exceptions.Exceptions;

namespace TaskTracker.Middlewares
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception is HttpResponseException httpResponseExeption)
            {
                context.Result = FilterHttpResponseException(httpResponseExeption);
                return;
            };

            _logger.LogError(context.Exception, "Unhandled exception happened");
            context.Result = new ObjectResult(new BadResponseObject(context.Exception.Message))
            {
                StatusCode = 500
            };
        }

        private IActionResult FilterHttpResponseException(HttpResponseException exception)
        {
            var responseObject = new BadResponseObject
            {
                Message = exception.Message,
                ResponseObject = exception.Object
            };

            return new ObjectResult(responseObject)
            {
                StatusCode = exception.StatusCode
            };
        }


}
}
