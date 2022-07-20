using DncZeus.Api.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DncZeus.Api.Job
{
    [DisallowConcurrentExecution]
    public class ResetFaceEncodingsJob : IJob
    {
        private readonly ILogger<ResetFaceEncodingsJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public ResetFaceEncodingsJob(ILogger<ResetFaceEncodingsJob> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DncZeusDbContext>();
            var now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;
            using (dbContext)
            {
                var query = dbContext.CourseSchedule.AsQueryable().AsNoTracking();
                query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No);
                query = query.Where(x => x.IsEnabled == CommonEnum.IsEnabled.Yes);
                query = query.Where(x => x.DayOfWeek == ScheduleOfWeek.特约课 || x.DayOfWeek == (ScheduleOfWeek)dayOfWeek);
                query = query.Where(x => now.Date <= x.EndDate && now.Date >= x.StartDate);
                //query = query.OrderBy("StartTime", false);
                var list = query.Include(x => x.Teacher)
                                .ThenInclude(x => x.FaceFeature)
                                .Include(x => x.CourseHourCourseSchedule)
                                .ThenInclude(x => x.CourseHour.Trainees.FaceFeature)
                                .Include(x => x.ClassGradeCourseSchedule)
                                .ThenInclude(x => x.ClassGrade.CourseHours)
                                .ThenInclude(x => x.Trainees.FaceFeature)
                                .Select(x => new
                                {
                                    TeacherFaceFeature = x.Teacher.FaceFeature,
                                    CourseHourFaceFeature = x.CourseHourCourseSchedule.Select(x => x.CourseHour.Trainees.FaceFeature).ToList(),
                                    ClassGradeFaceFeature = x.ClassGradeCourseSchedule.Select(x => x.ClassGrade.CourseHours.Select(x => x.Trainees.FaceFeature).ToList()).ToList()
                                }).ToList();

                List<FaceFeature> faceFeaturesList = new List<FaceFeature>();
                foreach (var item in list)
                {
                    faceFeaturesList.Add(item.TeacherFaceFeature);
                    faceFeaturesList.AddRange(item.CourseHourFaceFeature);
                    item.ClassGradeFaceFeature.ForEach(x => faceFeaturesList.AddRange(x));
                }
                FaceServer.ResetFaceEncodings(faceFeaturesList.Distinct().ToDictionary(x => x.FaceEncodes, x => x.Guid));
            }
            return Task.CompletedTask;
        }
    }
}
