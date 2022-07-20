using AutoMapper;
using DncZeus.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DncZeus.Api.Controllers.Api.V1.公众号
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WeChatNotifyController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WeChatNotifyController(DncZeusDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 微信通知入口
        /// </summary>
        /// <returns></returns>
        public IActionResult Access()
        {
            var context = _httpContextAccessor.HttpContext;
            string result = "";
            return Content(result);
        }
    }
}
