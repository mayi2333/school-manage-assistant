using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    public class OperationLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }
        /// <summary>
        /// 操作者姓名
        /// </summary>
        public string OperationByUserName { get; set; }
        /// <summary>
        /// 操作者Guid
        /// </summary>
        public Guid OperationByUserGuid { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string MoudleName { get; set; }
        /// <summary>
        /// 模块方法
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 操作名称
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 访问参数
        /// </summary>
        public string Parameter { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Descriptor { get; set; }
    }
}
