using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DncZeus.Api.Entities;
using DncZeus.Api.Extensions.AuthContext;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DncZeus.Api.Extensions.CustomException
{
    public class OperationLogAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        private DncZeusDbContext _dbContext;
        //private IServiceScopeFactory _scopeFactory;
        private string _moudleName { get; set; }
        private string _methodName { get; set; }
        private string _descriptor { get; set; }
        private IDictionary<string, object> _paramDic { get; set; }
        /// <summary>
        /// 操作日志过滤器
        /// </summary>
        /// <param name="Moudle">模块名称</param>
        /// <param name="Method">方法名称</param>
        /// <param name="Descriptor">描述</param>

        public OperationLogAttribute(string Moudle, string Method, string Descriptor)
        {
            //_dbContext = dbContext;
            _moudleName = Moudle;
            _methodName = Method;
            _descriptor = Descriptor;
        }
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            _paramDic.Add("result", context.Result);
            _dbContext = (DncZeusDbContext)context.HttpContext.RequestServices.GetService(typeof(DncZeusDbContext));
            using (_dbContext)
            {
                var entity = new OperationLog();
                var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                entity.ControllerName = controllerActionDescriptor?.ControllerName;
                entity.ActionName = controllerActionDescriptor?.ActionName;
                entity.OperationByUserName = AuthContextService.CurrentUser.DisplayName;
                entity.OperationByUserGuid = AuthContextService.CurrentUser.Guid;
                entity.MoudleName = _moudleName;
                entity.MethodName = _methodName;
                entity.Descriptor = _descriptor;
                entity.Guid = Guid.NewGuid();
                entity.OperationTime = DateTime.Now;
                entity.Parameter = JsonConvert.SerializeObject(_paramDic);
                _dbContext.OperationLog.Add(entity);
                _dbContext.SaveChanges();
            }
            base.OnResultExecuted(context);
        }
        /// <summary>
        /// 获取访问参数模型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //_scopeFactory = scopeFactory;
            _paramDic = context.ActionArguments;
            base.OnActionExecutionAsync(context, next);
        }
    }
}
