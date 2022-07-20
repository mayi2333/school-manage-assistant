/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DncZeus.Api.Extensions.CustomException
{
    /// <summary>
    /// 异常中间件
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                await HandleExceptionAsync(httpContext, ex, _logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            var error = new ErrorDetails {
                StatusCode= 500,
                Message=$"资源服务器忙,请稍候再试,原因:{exception.Message}"
            };
            if (exception is UnauthorizeException)
            {
                error.StatusCode= (int)HttpStatusCode.Unauthorized;
                error.Message = "未授权的访问(登录已超时或者当前用户没有操作权限)";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = error.StatusCode;
            //var logger = (ILogger)context.RequestServices.GetService(typeof(ILogger<>));
            logger.LogError(error.ToString());
            return context.Response.WriteAsync(error.ToString());
        }
    }
}
