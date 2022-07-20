namespace DncZeus.Api.Configurations
{
    /// <summary>
    /// 程序配置选项
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 是否是体验版
        /// </summary>
        public bool IsTrialVersion { get; set; }
        public WeChatApiInfo WeChatApiInfo { get; set; }
        /// <summary>
        /// 微信消息模板ID
        /// </summary>
        public TemplateId TemplateId { get; set; }
    }
    public class WeChatApiInfo
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
    public class TemplateId
    {
        public string Attend { get; set; }
    }
}
