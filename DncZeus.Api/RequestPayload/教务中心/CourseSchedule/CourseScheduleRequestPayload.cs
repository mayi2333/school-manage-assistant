using System;
using DncZeus.Api.Entities;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.RequestPayload.教务中心.CourseSchedule
{
    public class CourseScheduleRequestPayload : RequestPayload
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 是否生效
        /// </summary>
        public IsEnabled IsEnabled { get; set; }
        /// <summary>
        /// 星期数
        /// </summary>
        public ScheduleOfWeek DayOfWeek { get; set; }
        /// <summary>
        /// 课程科目Guid
        /// </summary>
        public string? CourseCode { get; set; }
        /// <summary>
        /// 教师Guid
        /// </summary>
        public Guid? TeacherGuid { get; set; }
        /// <summary>
        /// 班级Guid
        /// </summary>
        public Guid? ClassGradeGuid { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 下课时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 上课起始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 上课截至时间
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
