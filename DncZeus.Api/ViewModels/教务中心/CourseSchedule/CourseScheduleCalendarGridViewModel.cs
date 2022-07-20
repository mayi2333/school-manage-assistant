using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.ViewModels.教务中心.CourseSchedule
{
    public class CourseScheduleCalendarGridViewModel
    {
        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime InitialDate { get; set; }
        /// <summary>
        /// 起始星期数
        /// </summary>
        public int WeekDay { get; set; }
        /// <summary>
        /// 日程内容文字颜色
        /// </summary>
        public string TextColor { get; set; }
        /// <summary>
        /// 日程数据
        /// </summary>
        public List<CalendarGridItem> GridItem { get; set; }
    }
    public class CalendarGridItem
    {
        /// <summary>
        /// 显示在日程表里面的内容 \n 可以换行
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime End { get; set; }
        /// <summary>
        /// 背景颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 课程Guid
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 科目代码
        /// </summary>
        public string CourseCode { get; set; }

        //public string TextColor { get; set; }//文字颜色
    }
}
