/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

namespace DncZeus.Api.Entities.Enums
{
    /// <summary>
    /// 通用枚举类
    /// </summary>
    public class CommonEnum
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        public enum UserType
        {
            /// <summary>
            /// 超级管理员
            /// </summary>
            SuperAdministrator = 0,
            /// <summary>
            /// 管理员
            /// </summary>
            Admin = 1,
            /// <summary>
            /// 一般用户
            /// </summary>
            GeneralUser = 2,
            /// <summary>
            /// 微信授权登录客户_Customer表
            /// </summary>
            Customer = 100
        }

        /// <summary>
        /// 是否已删
        /// </summary>
        public enum IsDeleted
        {
            /// <summary>
            /// 所有
            /// </summary>
            All=-1,
            /// <summary>
            /// 否
            /// </summary>
            No = 0,
            /// <summary>
            /// 是
            /// </summary>
            Yes = 1
        }

        /// <summary>
        /// 是否已被锁定
        /// </summary>
        public enum IsLocked
        {
            /// <summary>
            /// 未锁定
            /// </summary>
            UnLocked = 0,
            /// <summary>
            /// 已锁定
            /// </summary>
            Locked = 1
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        public enum IsEnabled
        {
            /// <summary>
            /// 否
            /// </summary>
            No = 0,
            /// <summary>
            /// 是
            /// </summary>
            Yes = 1
        }


        /// <summary>
        /// 用户状态
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// 未指定
            /// </summary>
            All = -1,
            /// <summary>
            /// 已禁用
            /// </summary>
            Forbidden = 0,
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 1
        }

        /// <summary>
        /// 权限类型
        /// </summary>
        public enum PermissionType
        {
            /// <summary>
            /// 菜单
            /// </summary>
            Menu = 0,
            /// <summary>
            /// 按钮/操作/功能
            /// </summary>
            Action = 1
        }

        /// <summary>
        /// 是否枚举
        /// </summary>
        public enum YesOrNo
        {
            /// <summary>
            /// 所有
            /// </summary>
            All = -1,
            /// <summary>
            /// 否
            /// </summary>
            No = 0,
            /// <summary>
            /// 是
            /// </summary>
            Yes = 1
        }
        /// <summary>
        /// 性别
        /// </summary>
        public enum Sex
        {
            空缺 = 0,
            男 = 1,
            女 = 2,
        }
        public enum AttenceType
        {
            空缺 = -1,
            刷脸 = 0,
            刷卡 = 1,
            后台 = 2,
        }
        public enum AuditionCourseState
        {
            初始提交=0,
            预约成功,
            取消预约,
            已体验,
            未到课,
        }
    }
}
