using System;
using DncZeus.Api.Entities;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.RequestPayload.教务中心.ClassGrade
{
    /// <summary>
    /// 班级列表数据加载请求载体
    /// </summary>
    public class ClassGradeRequestPayload : RequestPayload
    {
        /// <summary>
        /// 是否已删
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 当前班级是否满员了
        /// </summary>
        public YesOrNo IsFull { get; set; }
        /// <summary>
        /// 是特约班
        /// </summary>
        public YesOrNo IsSpecial { get; set; }
    }
}
