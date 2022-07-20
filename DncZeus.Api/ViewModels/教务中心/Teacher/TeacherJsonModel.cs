using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.教务中心.Teacher
{
    public class TeacherJsonModel
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
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 给老师分配的课程名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 老师的本月上课次数
        /// </summary>
        public int Attendance { get; set; }
        /// <summary>
        /// 是否绑定ID卡
        /// </summary>
        public bool IdCardBind { get; set; }
        /// <summary>
        /// 是否绑定人脸信息
        /// </summary>
        public bool FaceBind { get; set; }
    }
}
