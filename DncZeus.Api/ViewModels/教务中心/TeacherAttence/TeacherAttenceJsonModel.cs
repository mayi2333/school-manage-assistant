using DncZeus.Api.Entities;
using System;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.教务中心.TeacherAttence
{
    public class TeacherAttenceJsonModel
    {
        /// <summary>
        /// 教师考勤Guid
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 是否出勤
        /// </summary>
        public YesOrNo IsAttend { get; set; }
        /// <summary>
        /// 是否代课
        /// </summary>
        public YesOrNo IsSubstitute { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        public string AttenceTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// 创建者姓名
        /// </summary>
        public string CreatedByUserName { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public string? ModifiedOn { get; set; }
        /// <summary>
        /// 最近修改者姓名
        /// </summary>
        public string ModifiedByUserName { get; set; }
        /// <summary>
        /// 教师名称
        /// </summary>
        public string TeacherName { get; set; }
        /// <summary>
        /// 科目名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 代课老师名称或者被代课老师名称
        /// </summary>
        public string SubstituteName { get; set; }
    }
}
