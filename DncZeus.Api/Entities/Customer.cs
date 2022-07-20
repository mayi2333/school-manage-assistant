using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    /// <summary>
    /// 客户实体类
    /// </summary>
    public class Customer
    {
        public Customer()
        {
            Trainees = new HashSet<Trainees>();
        }
        /// <summary>
        /// 客户GUID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 微信openid 识别用户的唯一标识
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string WxOpenid { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string WxUnionid { get; set; }
        /// <summary>
        /// 微信用户昵称
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string WxNickname { get; set; }
        /// <summary>
        /// 微信用户性别
        /// </summary>
        public Sex WxSex { get; set; }
        /// <summary>
        /// 微信用户个人资料填写的省份
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string WxProvince { get; set; }
        /// <summary>
        /// 微信用户个人资料填写的城市
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string WxCity { get; set; }
        /// <summary>
        /// 微信用户个人资料填写的国家，如中国为CN
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string WxCountry { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [Column(TypeName = "varchar(500)")]
        public string WxHeadimgurl { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime LastLogin { get; set; }
        /// <summary>
        /// 用户Guid 跟进人
        /// </summary>
        public Guid? UserGuid { get; set; }
        /// <summary>
        /// 用户来源
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// 关联的学员
        /// </summary>
        public virtual ICollection<Trainees> Trainees { get; set; }
    }
}
