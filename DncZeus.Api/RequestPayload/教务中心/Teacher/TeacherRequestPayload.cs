using System;
using DncZeus.Api.Entities;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.RequestPayload.教务中心.Teacher
{
    /// <summary>
    /// 教师列表数据加载请求载体
    /// </summary>
    public class TeacherRequestPayload: RequestPayload
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 是否分配了课程
        /// </summary>
        public YesOrNo IsBindCourseSchedule { get; set; }
    }
}
