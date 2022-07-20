using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using DncZeus.Api.RequestPayload.系统管理.OperationLog;
using DncZeus.Api.ViewModels.系统管理.OperationLog;
using DncZeus.Api.Extensions.CustomException;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DncZeus.Api.Controllers.Api.V1.系统管理
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class OperationLogController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public OperationLogController(DncZeusDbContext dbContext, IMapper mapper, ILogger<OperationLogController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpPost]
        [CustomAuthorize("operationlog_view")]
        public IActionResult List(OperationLogRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.OperationLog.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.ActionName.Contains(payload.Kw.Trim())
                                        || x.ControllerName.Contains(payload.Kw.Trim())
                                        || x.MoudleName.Contains(payload.Kw.Trim())
                                        || x.MethodName.Contains(payload.Kw.Trim())
                                        || x.OperationByUserName.Contains(payload.Kw.Trim())
                                        || x.Parameter.Contains(payload.Kw.Trim())
                                        || x.Descriptor.Contains(payload.Kw.Trim()));
                }
                if (payload.StartTime != null && payload.StartTime != DateTime.MinValue && payload.StartTime != DateTime.MaxValue)
                {
                    query = query.Where(x => payload.StartTime.Date <= x.OperationTime);
                }
                if (payload.EndTime != null && payload.EndTime != DateTime.MinValue && payload.EndTime != DateTime.MaxValue)
                {
                    query = query.Where(x => payload.EndTime.AddDays(1).Date >= x.OperationTime);
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<OperationLog, OperationLogJsonModel>);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }

        [HttpPost("/api/v1/save_error_logger")]
        [OperationLog("操作日志控制器", "错误日志保存", "记录前端错误信息")]
        public IActionResult SaveErrorLogger(SaveErrorLoggerPayload model)
        {
            //_logger.LogWarning(message: "{code:" + code + ",mes:\"" + mes + "\",type:\"" + type + "\",url:\"" + url + "\"}");
            _logger.LogWarning(message:JsonConvert.SerializeObject(model));
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetSuccess();
            return Ok(response);
        }
    }
}
