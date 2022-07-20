/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using DncZeus.Api.Entities;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions.AuthContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace DncZeus.Api.Extensions.CustomException
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        // https://tpodolak.com/blog/2017/12/13/asp-net-core-memorycache-getorcreate-calls-factory-method-multiple-times/
        private IMemoryCache _memoryCache;
        private DncZeusDbContext _dbContext;
        private string[] _actionCodes;
        /// <summary>
        /// 
        /// </summary>
        public CustomAuthorizeAttribute(params string[] actionCodes)
        {
            _actionCodes = actionCodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //return;
            // 以下权限拦截器未现实，所以直接return
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                throw new UnauthorizeException();
            }

            switch (AuthContextService.CurrentUser.UserType)
            {
                case CommonEnum.UserType.GeneralUser:
                case CommonEnum.UserType.Admin:
                    CheckPermission(context);
                    break;
                case CommonEnum.UserType.SuperAdministrator:
                    //超级管理员不检查权限
                    break;
                case CommonEnum.UserType.Customer:
                    break;
                default:
                    throw new UnauthorizeException();
            }
        }

        /// <summary>
        /// 当用户类型是普通用户或者管理员用户时  检查操作权限
        /// </summary>
        /// <param name="context"></param>
        private void CheckPermission(AuthorizationFilterContext context)
        {
            //var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            //string controllerName = controllerActionDescriptor?.ControllerName;
            //string actionName = controllerActionDescriptor?.ActionName;
            OwnedApiPermission entry;
            _memoryCache = (IMemoryCache)context.HttpContext.RequestServices.GetService(typeof(IMemoryCache));
            _dbContext = (DncZeusDbContext)context.HttpContext.RequestServices.GetService(typeof(DncZeusDbContext));
            entry = _memoryCache.GetOrCreate("CK_PERMISSION_" + AuthContextService.CurrentUser.LoginName, (cache) =>
            {
                //TODO: load real permission list from db
                //var permissionList = _dbContext.DncPermission.AsQueryable().AsNoTracking()
                //                        .Where(x => x.IsDeleted == CommonEnum.IsDeleted.No && x.Roles.Any())
                var sqlPermission = @"SELECT P.ActionCode AS PermissionActionCode,P.Name AS PermissionName,P.Type AS PermissionType,M.Name AS MenuName,M.Guid AS MenuGuid,M.Alias AS MenuAlias,M.IsDefaultRouter FROM DncRolePermissionMapping AS RPM 
LEFT JOIN DncPermission AS P ON P.Code = RPM.PermissionCode
INNER JOIN DncMenu AS M ON M.Guid = P.MenuGuid
WHERE P.IsDeleted=0 AND P.Status=1 AND EXISTS (SELECT 1 FROM DncUserRoleMapping AS URM WHERE URM.UserGuid={0} AND URM.RoleCode=RPM.RoleCode)";
                var actionCodeList = _dbContext.DncPermissionWithMenu.FromSqlRaw(sqlPermission, AuthContextService.CurrentUser.Guid).Select(s => s.PermissionActionCode).ToList();
                entry = new OwnedApiPermission(actionCodeList);

                cache.SlidingExpiration = TimeSpan.FromMinutes(30);
                cache.Value = entry;
                return entry;
            });
            if (!entry.Can(_actionCodes))
            {
                throw new UnauthorizeException();
            }
        }
    }
}
