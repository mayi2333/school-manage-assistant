using System;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.营销中心.AuditionCourse
{
    public class AuditionCourseJsonModel
    {
        /// <summary>
        /// 试课Guid
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 当前试课状态
        /// </summary>
        public AuditionCourseState State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
        /// <summary>
        /// 关联学员姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 关联学员联系手机
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 科目名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 关联的班级名称
        /// </summary>
        public string ClassName { get; set; }
    }
}
