using DncZeus.Api.Entities;
using System;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.ViewModels.营销中心.Customer
{
    public class CustomerJsonModel
    {
        /// <summary>
        /// 客户GUID
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 微信openid 识别用户的唯一标识
        /// </summary>
        public string WxOpenid { get; set; }
        public string WxUnionid { get; set; }
        /// <summary>
        /// 微信用户昵称
        /// </summary>
        public string WxNickname { get; set; }
        /// <summary>
        /// 微信用户性别
        /// </summary>
        public Sex WxSex { get; set; }
        /// <summary>
        /// 微信用户个人资料填写的省份
        /// </summary>
        public string WxProvince { get; set; }
        /// <summary>
        /// 微信用户个人资料填写的城市
        /// </summary>
        public string WxCity { get; set; }
        /// <summary>
        /// 微信用户个人资料填写的国家，如中国为CN
        /// </summary>
        public string WxCountry { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string WxHeadimgurl { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public string LastLogin { get; set; }
        /// <summary>
        /// 当前已绑定学员数量
        /// </summary>
        public int BindTraineesCount { get; set; }
    }
}
