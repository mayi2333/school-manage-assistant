/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using DncZeus.Api.Entities;
using DncZeus.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using DncZeus.Api.Auth;
using static DncZeus.Api.Entities.Enums.CommonEnum;
using System.Net.Http;
using System;
using DncZeus.Api.Utils;

namespace DncZeus.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OauthController : ControllerBase
    {
        private readonly AppAuthenticationSettings _appSettings;
        private readonly DncZeusDbContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="dbContext"></param>
        /// <param name="httpClientFactory"></param>
        public OauthController(IOptions<AppAuthenticationSettings> appSettings, DncZeusDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Auth(string username, string password)
        {
            var response = ResponseModelFactory.CreateInstance;
            DncUser user;
            using (_dbContext)
            {
                user = _dbContext.DncUser.FirstOrDefault(x => x.LoginName == username.Trim());
                if (user == null || user.IsDeleted == IsDeleted.Yes)
                {
                    response.SetFailed("用户不存在");
                    return Ok(response);
                }
                if (user.Password != password.Trim())
                {
                    response.SetFailed("密码不正确");
                    return Ok(response);
                }
                if (user.IsLocked == IsLocked.Locked)
                {
                    response.SetFailed("账号已被锁定");
                    return Ok(response);
                }
                if (user.Status == UserStatus.Forbidden)
                {
                    response.SetFailed("账号已被禁用");
                    return Ok(response);
                }
            }
            var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("guid",user.Guid.ToString()),
                    new Claim("avatar",""),
                    new Claim("displayName",user.DisplayName),
                    new Claim("loginName",user.LoginName),
                    new Claim("emailAddress",""),
                    new Claim("userType",((int)user.UserType).ToString())
                });
            var token = JwtBearerAuthenticationExtension.GetJwtAccessToken(_appSettings, claimsIdentity);

            response.SetData(token);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult WeChatAuth(string code)
        {
            var response = ResponseModelFactory.CreateInstance;
            try
            {
                var jo = WeChatApi.WeChatAuth(code);
                string nickname = jo["nickname"].ToString();
                string sex = jo["sex"].ToString();
                string province = jo["province"].ToString();
                string city = jo["city"].ToString();
                string country = jo["country"].ToString();
                string headimgurl = jo["headimgurl"].ToString();
                string privilege = jo["privilege"].ToString();
                string openid = jo["openid"].ToString();
                string unionid = string.Empty;
                if (jo.ContainsKey("unionid"))
                {
                    unionid = jo["unionid"].ToString();
                }
                using (_dbContext)
                {
                    var entity = _dbContext.Customer.FirstOrDefault(x => x.WxOpenid == openid);
                    if (entity != null)
                    {
                        entity.LastLogin = DateTime.Now;
                        entity.WxOpenid = openid;
                        entity.WxUnionid = unionid;
                        entity.WxHeadimgurl = headimgurl;
                        entity.WxNickname = nickname;
                        entity.WxCountry = country;
                        entity.WxProvince = province;
                        entity.WxCity = city;
                        entity.WxSex = (Sex)int.Parse(sex);
                    }
                    else
                    {
                        entity = new Customer()
                        {
                            Guid = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            LastLogin = DateTime.Now,
                            WxOpenid = openid,
                            WxUnionid = unionid,
                            WxHeadimgurl = headimgurl,
                            WxNickname = nickname,
                            WxCountry = country,
                            WxProvince = province,
                            WxCity = city,
                            WxSex = (Sex)int.Parse(sex),
                        };
                        _dbContext.Customer.Add(entity);
                    }
                    _dbContext.SaveChanges();
                    var claimsIdentity = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, entity.WxOpenid),
                            new Claim("guid",entity.Guid.ToString()),
                            new Claim("avatar",""),
                            new Claim("displayName",entity.WxNickname),
                            new Claim("loginName",entity.WxOpenid),
                            new Claim("emailAddress",""),
                            new Claim("userType",((int)UserType.Customer).ToString())
                        });
                    var token = JwtBearerAuthenticationExtension.GetJwtAccessToken(_appSettings, claimsIdentity);
                    var user = new { name = entity.WxNickname, openid = entity.WxOpenid, headimgurl = entity.WxHeadimgurl };
                    response.SetData(new { token = token, user = user });
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
                response.SetFailed("登录失败");
            }
            return Ok(response);
        }
    }
}