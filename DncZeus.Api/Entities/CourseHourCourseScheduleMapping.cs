using System;
using System.ComponentModel.DataAnnotations;

namespace DncZeus.Api.Entities
{
    public class CourseHourCourseScheduleMapping
    {
        /// <summary>
        /// 学生剩余课时Guid
        /// </summary>
        [Required]
        public Guid CourseHourGuid { get; set; }
        /// <summary>
        /// 课表Guid
        /// </summary>
        [Required]
        public Guid CourseScheduleGuid { get; set; }
        /// <summary>
        /// 学生剩余课时实体
        /// </summary>
        public virtual CourseHour CourseHour { get; set; }
        /// <summary>
        /// 课表实体
        /// </summary>
        public virtual CourseSchedule CourseSchedule { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
