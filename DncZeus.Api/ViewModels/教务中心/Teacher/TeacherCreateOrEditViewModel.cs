using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.ViewModels.教务中心.Teacher
{
    public class TeacherCreateOrEditViewModel
    {
        /// <summary>
        /// 老师GUID
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 老师姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 家庭住址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }
    }
}
