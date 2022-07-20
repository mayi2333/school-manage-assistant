using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.ViewModels.教务中心.Trainees
{
    public class TraineesCreateOrEditViewModel
    {
        /// <summary>
        /// 学员Guid
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 学员姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
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
