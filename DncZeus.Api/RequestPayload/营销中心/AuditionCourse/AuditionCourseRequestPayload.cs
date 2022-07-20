using System;
using DncZeus.Api.Entities;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.RequestPayload.营销中心.AuditionCourse
{
    public class AuditionCourseRequestPayload : RequestPayload
    {
        /// <summary>
        /// 预约试听状态
        /// </summary>
        public AuditionCourseState State { get; set; }
        /// <summary>
        /// 课程科目Guid
        /// </summary>
        public string? CourseCode { get; set; }
        /// <summary>
        /// 班级Guid
        /// </summary>
        public Guid? ClassGradeGuid { get; set; }
    }
}
