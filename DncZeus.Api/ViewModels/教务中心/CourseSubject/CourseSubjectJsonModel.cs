using DncZeus.Api.Entities;
using System;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.教务中心.CourseSubject
{
    public class CourseSubjectJsonModel
    {
        /// <summary>
        /// 科目编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 科目名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 收费方式
        /// </summary>
        public ChargeType ChargeType { get; set; }
        /// <summary>
        /// 课程价格(单位为分)，按课时为课时价格，按学期为学期价
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public string? ModifiedOn { get; set; }
        /// <summary>
        /// 是否已删
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 当前课程关联的班级数量
        /// </summary>
        public int ClassGradesCount { get; set; }
    }
}
