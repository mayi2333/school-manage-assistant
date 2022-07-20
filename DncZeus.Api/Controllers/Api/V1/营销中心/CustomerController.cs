using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DncZeus.Api.RequestPayload.营销中心.Customer;
using DncZeus.Api.ViewModels.营销中心.Customer;
using DncZeus.Api.Extensions.CustomException;

namespace DncZeus.Api.Controllers.Api.V1.营销中心
{
    /// <summary>
    /// 客户管理
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public CustomerController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="payload">客户列表数据加载请求载体</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize("customer_view")]
        public IActionResult List(CustomerRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.Customer.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.WxNickname.Contains(payload.Kw.Trim()) || x.WxCountry.Contains(payload.Kw.Trim()) || x.WxProvince.Contains(payload.Kw.Trim()) || x.WxCity.Contains(payload.Kw.Trim()));
                }
                if (payload.LastLogin != null)
                {
                    query = query.Where(x => x.LastLogin >= payload.LastLogin);
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).Include(x => x.Trainees).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<Customer, CustomerJsonModel>);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }
    }
}
