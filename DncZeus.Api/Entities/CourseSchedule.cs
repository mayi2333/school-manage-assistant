using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    public class CourseSchedule
    {
        public CourseSchedule()
        {
            TeacherAttences = new HashSet<TeacherAttence>();
            TraineesAttences = new HashSet<TraineesAttence>();
            ClassGradeCourseSchedule = new HashSet<ClassGradeCourseScheduleMapping>();
            CourseHourCourseSchedule = new HashSet<CourseHourCourseScheduleMapping>();
        }
        /// <summary>
        /// 课程时间表GUID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 上课起始日期
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 上课结束日期
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 按年循环
        /// </summary>
        public YesOrNo LoopOfYear { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        [Required]
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// 下课时间
        /// </summary>
        [Required]
        public TimeSpan EndTime { get; set; }
        /// <summary>
        /// 当前日期的星期数
        /// </summary>
        [Required]
        public ScheduleOfWeek DayOfWeek { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 是生效
        /// </summary>
        public IsEnabled IsEnabled { get;set;}
        /// <summary>
        /// 是否已删
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 课表填充背景色
        /// </summary>
        public string BackColor { get; set; }
        /// <summary>
        /// 上课教室
        /// </summary>
        public string ClassRoomName { get; set; }
        /// <summary>
        /// 上课老师Guid
        /// </summary>
        [Required]
        public Guid TeacherGuid { get; set; }
        /// <summary>
        /// 老师实体
        /// </summary>
        public virtual Teacher Teacher { get; set; }
        /// <summary>
        /// 课程科目Guid
        /// </summary>
        [Required]
        public string CourseCode { get; set; }
        /// <summary>
        /// 课程科目名称
        /// </summary>
        [Required]
        public string CourseName { get; set; }
        /// <summary>
        /// 课程科目实体
        /// </summary>
        public virtual CourseSubject CourseSubject { get; set; }
        /// <summary>
        /// 教师上课或代课出勤记录
        /// </summary>
        public virtual ICollection<TeacherAttence> TeacherAttences { get; set; }
        /// <summary>
        /// 学员上课出勤记录
        /// </summary>
        public virtual ICollection<TraineesAttence> TraineesAttences { get; set; }
        /// <summary>
        /// 班级课表映射
        /// </summary>
        public virtual ICollection<ClassGradeCourseScheduleMapping> ClassGradeCourseSchedule { get; set; }
        /// <summary>
        /// 学员课表映射
        /// </summary>
        public virtual ICollection<CourseHourCourseScheduleMapping> CourseHourCourseSchedule { get; set; }
    }
    public enum ScheduleOfWeek
    {
        星期天,
        星期一,
        星期二,
        星期三,
        星期四,
        星期五,
        星期六,
        特约课,
    }
}
