using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.ViewModels.教务中心.CourseHour
{
    public class CourseHourCreateOrEditViewModel
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
        /// 截至日期
        /// </summary>
        public string ExpiryDate { get; set; }
        /// <summary>
        /// 是否无期限
        /// </summary>
        public bool IsMaxExpiryDate { get; set; }
        /// <summary>
        /// 关联学员Guid
        /// </summary>
        public Guid TraineesGuid { get; set; }
        /// <summary>
        /// 关联课程的编码
        /// </summary>
        public string CourseCode { get; set; }
        /// <summary>
        /// 关联的班级Guid
        /// </summary>
        public Guid? ClassGradeGuid { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
}
