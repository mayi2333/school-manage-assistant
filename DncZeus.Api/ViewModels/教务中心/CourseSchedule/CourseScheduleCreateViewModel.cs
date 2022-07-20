using DncZeus.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.教务中心.CourseSchedule
{
    public class CourseScheduleCreateViewModel
    {
        public CourseScheduleCreateViewModel()
        {
            ClassGradeGuids = new List<Guid>();
            CourseHourGuids = new List<Guid>();
        }
        /// <summary>
        /// 上课起始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 上课结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 按年循环
        /// </summary>
        public YesOrNo LoopOfYear { get; set; }
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
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 是生效
        /// </summary>
        public IsEnabled IsEnabled { get; set; }
        /// <summary>
        /// 上课老师Guid
        /// </summary>
        public Guid TeacherGuid { get; set; }
        /// <summary>
        /// 课程科目代码
        /// </summary>
        public string CourseCode { get; set; }
        /// <summary>
        /// 课表填充背景色
        /// </summary>
        public string BackColor { get; set; }
        /// <summary>
        /// 上课教室
        /// </summary>
        public string ClassRoomName { get; set; }
        /// <summary>
        /// 绑定的班级Guid
        /// </summary>
        public ICollection<Guid> ClassGradeGuids { get; set; }
        /// <summary>
        /// 绑定的学员课时Guid
        /// </summary>
        public ICollection<Guid> CourseHourGuids { get; set; }
    }
    //public class GuidItem
    //{
    //    public Guid Value { get; set; }
    //    public string Label { get; set; }
    //}
}
