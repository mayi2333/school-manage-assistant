using DncZeus.Api.Entities;
using System;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.教务中心.Trainees
{
    public class TraineesJsonModel
    {
        /// <summary>
        /// 学员GUID
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
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 关联客户Guid
        /// </summary>
        public Guid? CustomerGuid { get; set; }
        /// <summary>
        /// 是否已删
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 已购买且未上完的课时
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 已分配的班级
        /// </summary>
        public string ClassName { get; set; }
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
