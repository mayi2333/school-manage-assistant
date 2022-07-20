using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DncZeus.Api.Entities
{
    public class TemplateMsg
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string ToUser { get; set; }
        /// <summary>
        /// 消息模板ID
        /// </summary>
        public string TemplateId { get;set;}
        /// <summary>
        /// 消息内容 JSON格式
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 消息ID发送成功后返回
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 当前发送次数
        /// </summary>
        public int SendNum { get; set; }
        /// <summary>
        /// 发送状态
        /// </summary>
        public TemplateMsgSendStatus SendStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 定时
        /// </summary>
        public DateTime Timing { get; set; }
    }
    public enum TemplateMsgSendStatus
    {
        未发送=0,
        请求中,
        请求成功,
        发送成功,
        请求失败,
        发送失败
    }
}
