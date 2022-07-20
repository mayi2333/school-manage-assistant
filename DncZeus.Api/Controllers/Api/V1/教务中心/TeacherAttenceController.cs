using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.AuthContext;
using DncZeus.Api.Extensions.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using DncZeus.Api.RequestPayload.教务中心.TeacherAttence;
using DncZeus.Api.ViewModels.教务中心.TeacherAttence;
using DncZeus.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Authorization;

namespace DncZeus.Api.Controllers.Api.V1.教务中心
{
    /// <summary>
    /// 教师考勤
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TeacherAttenceController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;

        public TeacherAttenceController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpPost]
        [CustomAuthorize("teacherattence_view")]
        public IActionResult List(TeacherAttenceRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.TeacherAttence.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Teacher.FullName.Contains(payload.Kw.Trim()));
                }
                if (payload.IsAttend > CommonEnum.YesOrNo.All)
                {
                    query = query.Where(x => x.IsAttend == payload.IsAttend);
                }
                if (payload.IsSubstitute > CommonEnum.YesOrNo.All)
                {
                    query = query.Where(x => x.IsSubstitute == payload.IsSubstitute);
                }
                if (payload.StartTime != null && payload.StartTime != DateTime.MinValue && payload.StartTime != DateTime.MaxValue)
                {
                    query = query.Where(x => payload.StartTime.Date <= x.CreatedOn);
                }
                if (payload.EndTime != null && payload.EndTime != DateTime.MinValue && payload.EndTime != DateTime.MaxValue)
                {
                    query = query.Where(x => payload.EndTime.AddDays(1).Date > x.CreatedOn);
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize)
                                .Include(x => x.CourseSchedule)
                                .Include(x => x.Teacher)
                                .Include(x => x.ParentGuidNavigation.Teacher)
                                .Include(x => x.InverseParentGuidNavigation.Teacher).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<TeacherAttence, TeacherAttenceJsonModel>);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }
        [HttpGet("{attenctGuid}")]
        [OperationLog("教师考勤控制器", "设置代课教师", "教师未签到设置该节课的代课教师")]
        [CustomAuthorize("set_substitute_teacher")]
        public IActionResult SetSubstituteTeacher(Guid attenctGuid, Guid teacherGuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            var entity = _dbContext.TeacherAttence.FirstOrDefault(x => x.Guid == attenctGuid);
            bool teacher = _dbContext.Teacher.Any(x => x.Guid == teacherGuid);
            if (entity != null && teacher)
            {
                if (entity.IsSubstitute == CommonEnum.YesOrNo.No)
                {
                    if (entity.TeacherGuid != teacherGuid)
                    {
                        if (entity.InverseParentGuidNavigation != null)
                        {
                            _dbContext.TeacherAttence.Remove(entity.InverseParentGuidNavigation);
                        }
                        entity.IsAttend = CommonEnum.YesOrNo.No;
                        entity.ModifiedOn = DateTime.Now;
                        entity.ModifiedByUserGuid = AuthContextService.CurrentUser.Guid;
                        entity.ModifiedByUserName = AuthContextService.CurrentUser.DisplayName;
                        _dbContext.TeacherAttence.Add(new TeacherAttence()
                        {
                            Guid = Guid.NewGuid(),
                            ParentGuid = entity.Guid,
                            CreatedOn = DateTime.Now,
                            CreatedByUserGuid = AuthContextService.CurrentUser.Guid,
                            CreatedByUserName = AuthContextService.CurrentUser.DisplayName,
                            CourseScheduleGuid = entity.CourseScheduleGuid,
                            IsAttend = CommonEnum.YesOrNo.Yes,
                            IsSubstitute = CommonEnum.YesOrNo.Yes,
                            TeacherGuid = teacherGuid,
                            AttenceTime = DateTime.Now,
                            AttenceType = CommonEnum.AttenceType.后台,
                        });
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        response.SetFailed("不能设置当前教师为代课教师");
                    }
                }
                else
                {
                    response.SetFailed("代课记录不能设置代课教师");
                }
            }
            else
            {
                response.SetFailed("参数错误");
            }
            return Ok(response);
        }
    }
}
