using System;
using DncZeus.Api.Entities;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.RequestPayload.教务中心.Trainees
{
    /// <summary>
    /// 学员列表数据加载请求载体
    /// </summary>
    public class TraineesRequestPayload : RequestPayload
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 是否绑定了班级
        /// </summary>
        public YesOrNo IsBindClassGrade { get; set; }
        /// <summary>
        /// 是否购买了课时且未上完课
        /// </summary>
        public YesOrNo IsBindCourseHour { get; set; }
        /// <summary>
        /// 班级Guid-查询当前班级下的学员
        /// </summary>
        public Guid? ClassGradeGuid { get; set; }
        /// <summary>
        /// 科目Code-查询已购买当前课程科目的学员
        /// </summary>
        public string? CourseCode { get; set; }
    }
}
