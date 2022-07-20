using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using DncZeus.Api.RequestPayload.教务中心.CourseHour;
using DncZeus.Api.ViewModels.教务中心.CourseHour;
using DncZeus.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Authorization;

namespace DncZeus.Api.Controllers.Api.V1.教务中心
{
    /// <summary>
    /// 学员课时
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CourseHourController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;

        public CourseHourController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 学员课时列表加载
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize("coursehour_view")]
        public IActionResult List(CourseHourRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.CourseHour.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Trainees.FullName.Contains(payload.Kw.Trim())
                                        || x.Trainees.Telephone.Contains(payload.Kw.Trim())
                                        || x.OperationLog.Contains(payload.Kw.Trim()));
                }
                if (!string.IsNullOrEmpty(payload.CourseCode))
                {
                    query = query.Where(x => x.CourseCode == payload.CourseCode);
                }
                //if (payload.ClassGradeGuid != null && payload.ClassGradeGuid != Guid.Empty)
                if (payload.ClassGradeGuid.HasValue && payload.ClassGradeGuid != Guid.Empty)
                {
                    query = query.Where(x => x.ClassGradeGuid == payload.ClassGradeGuid);
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize)
                                .Include(x => x.Trainees)
                                .Include(x => x.ClassGrade)
                                .Include(x => x.CourseSubject).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<CourseHour, CourseHourJsonModel>);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 根据科目代码查询特约班的学员课时
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("{code}")]
        public IActionResult IsSpecialOfFindByCourseCode(string code)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            if (string.IsNullOrEmpty(code))
            {
                response.SetFailed("没有查询到数据");
                return Ok(response);
            }
            using (_dbContext)
            {
                var query = _dbContext.CourseHour.Where(x => x.CourseCode == code.ToUpper() && x.ClassGrade.IsSpecial == CommonEnum.YesOrNo.Yes
                    && x.Surplus > 0 && x.ExpiryDate.Date >= DateTime.Now)
                    .Include(x => x.Trainees)
                    .OrderBy("CreatedOn", true);
                var list = query.ToList();
                var data = list.Select(x => new { x.Guid, x.Trainees.FullName, x.Trainees.Telephone });
                response.SetData(data);
                return Ok(response);
            }
        }

        /// <summary>
        /// 根据课表Guid和科目代码查询学员课时
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [CustomAuthorize("courseschedule_assign")]
        public IActionResult FindListByCourseScheduleGuid(Guid guid, string code)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                //var courseCode = _dbContext.CourseSchedule.FirstOrDefault(x => x.Guid == guid).CourseCode;
                var query = _dbContext.CourseHourCourseScheduleMapping.Where(x => x.CourseScheduleGuid == guid).Include(x => x.CourseHour);
                var assignedCourseHours = query.ToList().Select(x => x.CourseHour.Guid).ToList();
                var courseHours = _dbContext.CourseHour.Where(x => x.CourseCode == code.ToUpper() && x.ClassGrade.IsSpecial == CommonEnum.YesOrNo.Yes && x.Surplus > 0 && x.ExpiryDate.Date >= DateTime.Now).Include(x => x.Trainees).ToList().Select(x => new { label = x.Trainees.FullName, key = x.Guid });
                response.SetData(new { courseHours, assignedCourseHours });
                return Ok(response);
            }
        }
        /// <summary>
        /// 创建课时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("coursehour_create")]
        [OperationLog("学员课时控制器", "创建课时", "创建学员课时")]
        public IActionResult Create(CourseHourCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.CourseCode.Trim().Length <= 0)
            {
                response.SetFailed("请输入课程编码");
                return Ok(response);
            }
            if (model.Surplus <= 0)
            {
                response.SetFailed("课时数需要大于0");
                return Ok(response);
            }
            if (Guid.Empty == model.TraineesGuid)
            {
                response.SetFailed("请输入学员Guid");
                return Ok(response);
            }
            //using (_dbContext)
            //{
                if (_dbContext.CourseHour.Count(x => x.CourseCode == model.CourseCode && x.TraineesGuid == model.TraineesGuid) > 0)
                {
                    response.SetFailed("该课时已存在，创建失败");
                    return Ok(response);
                }
                if (_dbContext.Trainees.Count(x => x.Guid == model.TraineesGuid) <= 0 
                    || _dbContext.CourseSubject.Count(x => x.Code == model.CourseCode) <= 0
                    || (model.ClassGradeGuid.HasValue && model.ClassGradeGuid != Guid.Empty && _dbContext.ClassGrade.Count(x => x.Guid == model.ClassGradeGuid && x.CourseCode == model.CourseCode) <=0))
                {
                    response.SetFailed("参数错误，创建失败");
                    return Ok(response);
                }
                //var entity = new Trainees();
                var entity = _mapper.Map<CourseHourCreateOrEditViewModel, CourseHour>(model);
                entity.CreatedOn = DateTime.Now;
                entity.Guid = Guid.NewGuid();
                entity.OperationLog = DateTime.Now.ToString("yyyy-MM-dd|") + "后台操作|0->" + entity.Surplus + "课时\r\n" 
                    + (string.IsNullOrEmpty(model.Memo) ? "" : ("备注：" + model.Memo.Replace("\r\n", " ").Replace("\n", " ") + "\r\n"));
                _dbContext.CourseHour.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            //}
        }

        /// <summary>
        /// 编辑课时
        /// </summary>
        /// <param name="guid">课时GUID</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.CourseHour.FirstOrDefault(x => x.Guid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<CourseHour, CourseHourCreateOrEditViewModel>(entity));
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的课时信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("coursehour_edit")]
        [OperationLog("学员课时控制器", "编辑课时", "编辑学员课时")]
        public IActionResult Edit(CourseHourCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            if (model.CourseCode.Trim().Length <= 0)
            {
                response.SetFailed("请输入课程编码");
                return Ok(response);
            }
            if (model.Surplus < 0)
            {
                response.SetFailed("请输入正确的剩余课时数");
                return Ok(response);
            }
            if (Guid.Empty == model.TraineesGuid)
            {
                response.SetFailed("请输入学员Guid");
                return Ok(response);
            }
            //using (_dbContext)
            //{
                if (_dbContext.CourseHour.Count(x => x.Guid == model.Guid 
                && x.CourseCode == model.CourseCode 
                && x.TraineesGuid == model.TraineesGuid) <= 0)
                {
                    response.SetFailed("参数错误，修改失败");
                    return Ok(response);
                }
                if (_dbContext.Trainees.Count(x => x.Guid == model.TraineesGuid) <= 0
                    || _dbContext.CourseSubject.Count(x => x.Code == model.CourseCode) <= 0
                    || (model.ClassGradeGuid.HasValue && model.ClassGradeGuid != Guid.Empty && _dbContext.ClassGrade.Count(x => x.Guid == model.ClassGradeGuid && x.CourseCode == model.CourseCode) <= 0))
                {
                    response.SetFailed("参数错误，修改失败");
                    return Ok(response);
                }
                var entity = _dbContext.CourseHour.FirstOrDefault(x => x.Guid == model.Guid);
                if (entity.ClassGradeGuid != model.ClassGradeGuid)
                {
                    if (entity.CourseHourCourseSchedule.Any(x => x.CourseSchedule.IsEnabled == CommonEnum.IsEnabled.Yes && DateTime.Now < x.CourseSchedule.EndDate))
                    {
                        response.SetFailed("该学员有分配课程，无法修改班级");
                        return Ok(response);
                    }
                    else
                    {
                        entity.ClassGradeGuid = model.ClassGradeGuid;
                    }
                }
                entity.ModifiedOn = DateTime.Now;
                if (entity.Surplus != model.Surplus)
                {
                    entity.OperationLog += DateTime.Now.ToString("yyyy-MM-dd|") + "后台操作|" + entity.Surplus + "->" + model.Surplus + "课时\r\n"
                        + (string.IsNullOrEmpty(model.Memo) ? "" : ("备注：" + model.Memo.Replace("\r\n", " ").Replace("\n", " ") + "\r\n"));
                    entity.Surplus = model.Surplus;
                }
                if (model.IsMaxExpiryDate)
                {
                    entity.ExpiryDate = DateTime.MaxValue;
                }
                else
                {
                    entity.ExpiryDate = DateTime.Parse(model.ExpiryDate);
                }
                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            //}
        }
    }
}
