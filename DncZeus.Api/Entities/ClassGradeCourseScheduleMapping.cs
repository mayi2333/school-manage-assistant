using System;
using System.ComponentModel.DataAnnotations;

namespace DncZeus.Api.Entities
{
    public class ClassGradeCourseScheduleMapping
    {
        /// <summary>
        /// 班级Guid
        /// </summary>
        [Required]
        public Guid ClassGradeGuid { get; set; }
        /// <summary>
        /// 课表Guid
        /// </summary>
        [Required]
        public Guid CourseScheduleGuid { get; set; }
        /// <summary>
        /// 班级实体
        /// </summary>
        public virtual ClassGrade ClassGrade { get; set; }
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
