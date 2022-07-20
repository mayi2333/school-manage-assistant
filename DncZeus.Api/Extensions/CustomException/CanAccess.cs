/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2019-01-08
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace DncZeus.Api.Extensions.CustomException
{
    ///// <summary>
    ///// 用户可以访问的控制器及操作权限
    ///// </summary>
    //public class CanAccess
    //{
    //    /// <summary>
    //    /// 控制器
    //    /// </summary>
    //    public string Controller { get; set; }
    //    /// <summary>
    //    /// Action集合
    //    /// </summary>
    //    public List<string> Actions { get; set; }
    //}

    /// <summary>
    /// 用户拥有的API操作权限
    /// </summary>
    public class OwnedApiPermission
    {
        /// <summary>
        /// 
        /// </summary>
        public OwnedApiPermission()
        {
            CanAccesses = new List<string>();
        }
        public OwnedApiPermission(List<string> actionCodeList)
        {
            CanAccesses = actionCodeList;
        }
        /// <summary>
        /// 可以访问的操作码集合
        /// </summary>
        public List<string> CanAccesses { get; set; }

        /// <summary>
        /// 是否可以访问
        /// </summary>
        /// <param name="actionCode">操作码</param>
        /// <returns></returns>
        public bool Can(string[] actionCode)
        {
            //if (string.IsNullOrEmpty(controller) || string.IsNullOrEmpty(action))
            //{
            //    return false;
            //}
            //var ctrl = CanAccesses.Where(x => string.Equals(controller, x.Controller,StringComparison.OrdinalIgnoreCase)).FirstOrDefault(x => x.Controller == controller);
            //if (ctrl == null)
            //{
            //    return false;
            //}
            //return ctrl.Actions.Contains(action, StringComparer.OrdinalIgnoreCase);
            return CanAccesses.Any(x => actionCode.Contains(x, StringComparer.OrdinalIgnoreCase));
        }
    }
}
