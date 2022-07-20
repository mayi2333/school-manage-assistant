using DncZeus.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.教务中心.CourseSchedule
{
    public class CourseScheduleJsonModel
    {
        /// <summary>
        /// 课表Guid
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 上课起始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 上课截至时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 展示时间
        /// </summary>
        public string DisplayTime { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// 下课时间
        /// </summary>
        public TimeSpan EndTime { get; set; }
        /// <summary>
        /// 当前日期的星期数
        /// </summary>
        public ScheduleOfWeek DayOfWeek { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 是生效
        /// </summary>
        public IsEnabled IsEnabled { get; set; }
        /// <summary>
        /// 是否已删
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 上课老师名称
        /// </summary>
        public string TeacherName { get; set; }
        /// <summary>
        /// 课程科目名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 课程科目代码
        /// </summary>
        public string CourseCode { get; set; }
        /// <summary>
        /// 教室名称
        /// </summary>
        public string ClassRoomName { get; set; }
    }
}
