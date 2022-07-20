using DncZeus.Api.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.Job
{
    [DisallowConcurrentExecution]
    public class HelloWorldJob : IJob
    {
        private readonly ILogger<HelloWorldJob> _logger;
        //private readonly DncZeusDbContext _dbContext;
        private readonly IServiceScopeFactory _scopeFactory;
        public HelloWorldJob(ILogger<HelloWorldJob> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DncZeusDbContext>();
            _logger.LogInformation("Hello world!");
            _logger.LogInformation(dbContext.Trainees.FirstOrDefault().FullName);
            return Task.CompletedTask;
        }
    }
}
