using System;
using DncZeus.Api.Entities;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.RequestPayload.营销中心.Customer
{
    /// <summary>
    /// 学员列表数据加载请求载体
    /// </summary>
    public class CustomerRequestPayload : RequestPayload
    {
        /// <summary>
        /// 最后一次登录在查询时间内
        /// </summary>
        public DateTime? LastLogin { get; set; }
    }
}
