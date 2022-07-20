using DncZeus.Api.Entities;

namespace DncZeus.Api.ViewModels.教务中心.CourseSubject
{
    public class CourseSubjectCreateOrEditViewModel
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
    }
}
