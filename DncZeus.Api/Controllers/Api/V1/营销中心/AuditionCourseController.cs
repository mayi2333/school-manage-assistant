using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DncZeus.Api.Extensions.CustomException;
using DncZeus.Api.RequestPayload.营销中心.AuditionCourse;
using System;
using DncZeus.Api.ViewModels.营销中心.AuditionCourse;

namespace DncZeus.Api.Controllers.Api.V1.营销中心
{
    /// <summary>
    /// 预约试听课
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AuditionCourseController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuditionCourseController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost]
        [CustomAuthorize("auditioncourse_view")]
        public IActionResult List(AuditionCourseRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.AuditionCourse.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Trainees.FullName.Contains(payload.Kw.Trim()) || x.Trainees.Telephone.Contains(payload.Kw.Trim()));
                }
                if (payload.ClassGradeGuid.HasValue && payload.ClassGradeGuid != Guid.Empty)
                {
                    query = query.Where(x => x.ClassGradeGuid == payload.ClassGradeGuid);
                }
                if (!string.IsNullOrEmpty(payload.CourseCode))
                {
                    query = query.Where(x => x.CourseCode == payload.CourseCode);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize)
                                .Include(x => x.Trainees)
                                .Include(x => x.ClassGrade)
                                .Include(x => x.CourseSubject)
                                .ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<AuditionCourse, AuditionCourseJsonModel>);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }
    }
}
