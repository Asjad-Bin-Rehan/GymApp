using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSAPI.Common.Filters
{
    public class RequestLoggingFilter(ICustomLogger logger) : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            logger.LogInformation("HTTP {Method} {Path} received", context.HttpContext.Request.Method, context.HttpContext.Request.Path);
            return await next(context);
        }
    }
}