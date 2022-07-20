using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.ViewModels.教务中心.CourseSchedule
{
    public class SignInDeskListItemViewModel
    {
        /// <summary>
        /// 课程时间表Guid
        /// </summary>
        public Guid CourseScheduleGuid { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
    public class SignInDeskStatisViewModel
    {
        /// <summary>
        /// 待签到教师
        /// </summary>
        public int ReadyToSignInTeacher { get; set; }
        /// <summary>
        /// 已签到教师
        /// </summary>
        public int SignInTeacher { get; set; }
        /// <summary>
        /// 待签到学员
        /// </summary>
        public int ReadyToSignInTrainees { get; set; }
        /// <summary>
        /// 已签到学员
        /// </summary>
        public int SignInTrainees { get; set; }
    }
}
