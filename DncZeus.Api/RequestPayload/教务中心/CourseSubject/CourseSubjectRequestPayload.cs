using System;
using DncZeus.Api.Entities;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.RequestPayload.教务中心.CourseSubject
{
    /// <summary>
    /// 课程科目列表加载请求载体
    /// </summary>
    public class CourseSubjectRequestPayload : RequestPayload
    {
        public IsDeleted IsDeleted { get; set; }
        public ChargeType ChargeType { get; set; }
    }
}
