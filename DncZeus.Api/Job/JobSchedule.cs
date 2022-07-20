using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.Job
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression, bool isStartNow=false)
        {
            JobType = jobType;
            CronExpression = cronExpression;
            IsStartNow = isStartNow;
        }
        /// <summary>
        /// 任务类
        /// </summary>
        public Type JobType { get; }
        /// <summary>
        /// Cron时间表达式
        /// </summary>
        public string CronExpression { get; }
        /// <summary>
        /// 是否立即开始任务
        /// </summary>
        public bool IsStartNow { get; }
    }
}
