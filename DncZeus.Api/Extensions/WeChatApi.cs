using DncZeus.Api.Entities;
using DncZeus.Api.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DncZeus.Api.Extensions
{
    public class WeChatApi
    {
        private static IServiceProvider _serviceProvider;
        private static IHttpClientFactory _httpClientFactory;
        private static string _appSecret;
        private static string _appId;
        private static string _accessToken;
        private static DateTime _accessTokenExpires;

        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            _appSecret = ConfigurationManager.AppSettings.WeChatApiInfo.AppSecret;
            _appId = ConfigurationManager.AppSettings.WeChatApiInfo.AppId;
            _accessTokenExpires = DateTime.Now.AddSeconds(-1);
        }

        /// <summary>
        /// 微信授权登陆接口
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static JObject WeChatAuth(string code)
        {
            string get_access_token_url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", _appId, _appSecret, code);
            string res = HttpGet(get_access_token_url).Result;
            var jo = (JObject)JsonConvert.DeserializeObject(res);
            string access_token = string.Empty;
            string openid = string.Empty;
            if (jo.ContainsKey("access_token") && jo.ContainsKey("openid"))
            {
                access_token = jo["access_token"].ToString();
                openid = jo["openid"].ToString();
            }
            else
            {
                throw new Exception("access_token获取失败," + res);
            }
            string get_userinfo_url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);
            res = HttpGet(get_userinfo_url).Result;
            jo = (JObject)JsonConvert.DeserializeObject(res);
            if (jo.ContainsKey("nickname") && jo.ContainsKey("openid"))
            {
                return jo;
            }
            else
            {
                throw new Exception("用户信息获取失败," + res);
            }
        }

        /// <summary>
        /// 获取微信api访问凭证,如果未过期直接返回true
        /// </summary>
        /// <returns></returns>
        public static void GetAccessToken()
        {
            if (DateTime.Now > _accessTokenExpires.AddSeconds(-30))
            {
                string get_access_token_url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", _appId, _appSecret);
                string res = HttpGet(get_access_token_url).Result;
                var jo = (JObject)JsonConvert.DeserializeObject(res);

                if (jo.ContainsKey("access_token") && jo.ContainsKey("expires_in"))
                {
                    _accessToken = jo["access_token"].ToString();
                    _accessTokenExpires = DateTime.Now.AddSeconds(double.Parse(jo["expires_in"].ToString()));
                }
                else
                {
                    throw new Exception("access_token获取失败," + res);
                }
            }
        }

        /// <summary>
        /// 模板消息发送
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public static string TemplateMsgSend(string jsondata)
        {
            GetAccessToken();
            string templatemsg_send_url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", _accessToken);
            string res = HttpPost(templatemsg_send_url, jsondata).Result;
            var jo = (JObject)JsonConvert.DeserializeObject(res);

            if (jo.ContainsKey("errcode"))
            {
                string errcode = jo["errcode"].ToString();
                if (errcode == "0")
                {
                    return jo["msgid"].ToString();
                }
                else
                {
                    string errmsg = jo["errmsg"].ToString();
                    throw new Exception(errmsg);
                }
            }
            else
            {
                throw new Exception("发送失败," + res);
            }
        }

        private static async Task<string> HttpGet(string url)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            using (httpClient)
            {
                var result = await httpClient.GetAsync(url);
                return await result.Content.ReadAsStringAsync();
            }
        }
        private static async Task<string> HttpPost(string url, string jsondata)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            using (httpClient)
            {
                var result = await httpClient.PostAsync(url, new StringContent(jsondata, Encoding.UTF8, "application/json"));
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
