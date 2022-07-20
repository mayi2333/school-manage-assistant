using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace DncZeus.Api.Job
{
    /// <summary>
    /// QuartzJob扩展
    /// </summary>
    public static class QuartzJobExtensions
    {
        public static void AddQuartzJob(this IServiceCollection services)
        {
            // Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Add Job
            services.AddSingleton<ResetFaceEncodingsJob>();
            services.AddSingleton<TemplateMsgSendJob>();

            //Add JobSchedule
            services.AddSingleton(new JobSchedule(
               jobType: typeof(ResetFaceEncodingsJob),
               //cronExpression: "0/30 * * * * ? *")); // 每30秒执行一次
               cronExpression: "0 0 3 1/1 * ? " //每天早上3点运行一次
               ));

            services.AddSingleton(new JobSchedule(
               jobType: typeof(TemplateMsgSendJob),
               cronExpression: "0 0-59 6-22 * * ?" // 每天早上6点到晚上10点之间每分钟执行一次
               //cronExpression: "0 0 3 1/1 * ? " //每天早上3点运行一次
               ));

            //services.AddSingleton(new JobSchedule(
            //    jobType: typeof(IntroduceJob), cronExpression: "0/5 * * * * ?"));

            //Add Quartz HostedService
            services.AddHostedService<QuartzHostedService>();
        }
    }
}
