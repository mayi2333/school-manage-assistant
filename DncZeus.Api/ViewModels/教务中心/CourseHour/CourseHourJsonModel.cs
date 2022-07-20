using DncZeus.Api.Entities;
using System;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.教务中心.CourseHour
{
    public class CourseHourJsonModel
    {
        /// <summary>
        /// 课时GUID
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 剩余课时数
        /// </summary>
        public int Surplus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public string? ModifiedOn { get; set; }
        /// <summary>
        /// 截至日期
        /// </summary>
        public string? ExpiryDate { get; set; }
        /// <summary>
        /// 修改课时所产生的操作日志，只增不减，打卡消课不做记录
        /// </summary>
        public string OperationLog { get; set; }
        /// <summary>
        /// 关联学员姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 学员电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 关联的课程名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 关联的班级名称
        /// </summary>
        public string ClassName { get; set; }
    }
}
