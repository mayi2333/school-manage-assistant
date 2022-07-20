using System;
using DncZeus.Api.Entities;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.RequestPayload.教务中心.CourseHour
{
    /// <summary>
    /// 学员课时列表数据加载请求载体
    /// </summary>
    public class CourseHourRequestPayload : RequestPayload
    {
        /// <summary>
        /// 课程科目代码Code
        /// </summary>
        public string? CourseCode { get; set; }
        /// <summary>
        /// 所属班级Guid
        /// </summary>
        public Guid? ClassGradeGuid { get; set; }
    }
}
