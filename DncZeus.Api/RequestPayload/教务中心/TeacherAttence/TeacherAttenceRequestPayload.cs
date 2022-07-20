using System;
using DncZeus.Api.Entities;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.RequestPayload.教务中心.TeacherAttence
{
    public class TeacherAttenceRequestPayload : RequestPayload
    {
        //public TeacherAttenceRequestPayload()
        //{
        //    DateTime now = DateTime.Now;
        //    StartTime = new DateTime(now.Year, now.Month, now.Day);
        //    EndTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);
        //}
        /// <summary>
        /// 是否出勤
        /// </summary>
        public YesOrNo IsAttend { get; set; }
        /// <summary>
        /// 是否代课
        /// </summary>
        public YesOrNo IsSubstitute { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
