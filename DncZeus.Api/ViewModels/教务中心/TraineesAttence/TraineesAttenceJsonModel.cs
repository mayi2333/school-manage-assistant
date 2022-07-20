using DncZeus.Api.Entities;
using System;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.教务中心.TraineesAttence
{
    public class TraineesAttenceJsonModel
    {
        /// <summary>
        /// 学员考勤Guid
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 是否出勤
        /// </summary>
        public YesOrNo IsAttend { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        public string AttenceTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public string? ModifiedOn { get; set; }
        /// <summary>
        /// 最近修改者姓名
        /// </summary>
        public string ModifiedByUserName { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 学员姓名
        /// </summary>
        public string TraineesName { get; set; }
    }
}
