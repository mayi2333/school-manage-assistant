using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.RequestPayload.系统管理.OperationLog
{
    /// <summary>
    /// 操作日志列表数据加载请求载体
    /// </summary>
    public class OperationLogRequestPayload: RequestPayload
    {
        //public OperationLogRequestPayload()
        //{
        //    DateTime now = DateTime.Now;
        //    StartTime = new DateTime(now.Year, now.Month, now.Day);
        //    EndTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);
        //}
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
