using DncZeus.Api.Entities;
using DncZeus.Api.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.Job
{
    [DisallowConcurrentExecution]
    public class TemplateMsgSendJob : IJob
    {
        private readonly ILogger<TemplateMsgSendJob> _logger;
        //private readonly DncZeusDbContext _dbContext;
        private readonly IServiceScopeFactory _scopeFactory;
        public TemplateMsgSendJob(ILogger<TemplateMsgSendJob> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DncZeusDbContext>();
            using (dbContext)
            {
                var list = dbContext.TemplateMsg.Where(x => x.Timing <= DateTime.Now
                                                    && (x.SendStatus == TemplateMsgSendStatus.请求失败 || x.SendStatus == TemplateMsgSendStatus.未发送)
                                                    && x.SendNum < 3).Take(60).ToList();
                list.ForEach(x => x.SendStatus = TemplateMsgSendStatus.请求中);
                dbContext.SaveChanges();
                foreach (var item in list)
                {
                    var senddata = new
                    {
                        touser = item.ToUser,
                        template_id = item.TemplateId,
                        url = string.Empty,
                        topcolor = "#FF0000",
                        data = JsonConvert.DeserializeObject(item.Data)
                    };
                    try
                    {
                        item.MsgId = WeChatApi.TemplateMsgSend(JsonConvert.SerializeObject(senddata));
                        item.SendStatus = TemplateMsgSendStatus.请求成功;
                    }
                    catch (Exception ex)
                    {
                        item.SendStatus = TemplateMsgSendStatus.请求失败;
                        _logger.LogError(ex, "模板消息请求异常");
                    }
                    finally
                    {
                        item.SendNum += 1;
                        dbContext.SaveChanges();
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
