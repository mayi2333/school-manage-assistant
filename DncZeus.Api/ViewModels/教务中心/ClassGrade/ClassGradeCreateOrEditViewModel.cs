using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.教务中心.ClassGrade
{
    public class ClassGradeCreateOrEditViewModel
    {
        /// <summary>
        /// 班级GUID
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 班级额定总人数
        /// </summary>
        public int TotalPeople { get; set; }
        /// <summary>
        /// 是特约班
        /// </summary>
        public YesOrNo IsSpecial { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 科目编码
        /// </summary>
        public string CourseCode { get; set; }
    }
}
