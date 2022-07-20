using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using DncZeus.Api.RequestPayload.教务中心.ClassGrade;
using DncZeus.Api.ViewModels.教务中心.ClassGrade;
using static DncZeus.Api.Entities.Enums.CommonEnum;
using DncZeus.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Authorization;

namespace DncZeus.Api.Controllers.Api.V1.教务中心
{
    /// <summary>
    /// 班级管理
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ClassGradeController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;

        public ClassGradeController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 获取班级列表
        /// </summary>
        /// <param name="payload">班级列表数据加载请求载体</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize("classgrade_view")]
        public IActionResult List(ClassGradeRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.ClassGrade.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.ClassName.Contains(payload.Kw.Trim())
                                        || x.CourseSubject.CourseName.Contains(payload.Kw.Trim())
                                        || x.CourseCode.Contains(payload.Kw.Trim())
                                        || x.Memo.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDeleted == payload.IsDeleted);
                }
                if (payload.IsFull > CommonEnum.YesOrNo.All)
                {
                    query = payload.IsFull == CommonEnum.YesOrNo.Yes ?
                        query.Where(x => x.TotalPeople <= x.CourseHours.Count)
                        : query.Where(x => x.TotalPeople > x.CourseHours.Count);
                }
                if (payload.IsSpecial > CommonEnum.YesOrNo.All)
                {
                    query = query.Where(x => x.IsSpecial == payload.IsSpecial);
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize)
                                .Include(x => x.CourseSubject)
                                .Include(x => x.CourseHours).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<ClassGrade, ClassGradeJsonModel>);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }
        /// <summary>
        /// 根据关键字查询班级列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{kw}")]
        public IActionResult FindByKeyword(string kw)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            if (string.IsNullOrEmpty(kw))
            {
                response.SetFailed("没有查询到数据");
                return Ok(response);
            }
            using (_dbContext)
            {
                var query = _dbContext.ClassGrade.Where(x => x.ClassName.Contains(kw) && x.IsDeleted == CommonEnum.IsDeleted.No);
                //query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No);
                var list = query.ToList();
                var data = list.Select(x => new { x.Guid, x.ClassName, x.CourseCode });
                //var data = query.Select(x => new { x.CourseName, x.Code });

                response.SetData(data);
                return Ok(response);
            }
        }
        /// <summary>
        /// 根据课程代码查询班级列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{code}")]
        public IActionResult FindByCourseCode(string code)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            if (string.IsNullOrEmpty(code))
            {
                response.SetFailed("没有查询到数据");
                return Ok(response);
            }
            using (_dbContext)
            {
                var query = _dbContext.ClassGrade.Where(x => x.CourseCode == code.ToUpper() && x.IsDeleted == CommonEnum.IsDeleted.No);
                //query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No);
                var list = query.ToList();
                var data = list.Select(x => new { x.Guid, x.ClassName });
                //var data = query.Select(x => new { x.CourseName, x.Code });
                response.SetData(data);
                return Ok(response);
            }
        }
        /// <summary>
        /// 根据课表Guid和科目代码查询班级列表
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
                var query = _dbContext.ClassGradeCourseScheduleMapping.Where(x => x.CourseScheduleGuid == guid).Include(x => x.ClassGrade);
                var assignedClassGrades = query.ToList().Select(x => x.ClassGrade.Guid).ToList();
                var classGrades = _dbContext.ClassGrade.Where(x => x.CourseCode == code.ToUpper() && x.IsSpecial == CommonEnum.YesOrNo.No && x.IsDeleted == IsDeleted.No).ToList().Select(x => new { label = x.ClassName, key = x.Guid });
                response.SetData(new { classGrades, assignedClassGrades });
                return Ok(response);
            }
        }
        /// <summary>
        /// 根据课程代码查询班级列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{code}")]
        public IActionResult FindByCourseCodeAndIsSpecial(string code, YesOrNo isspecial)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            if (string.IsNullOrEmpty(code))
            {
                response.SetFailed("没有查询到数据");
                return Ok(response);
            }
            using (_dbContext)
            {
                var query = _dbContext.ClassGrade.Where(x => x.CourseCode == code.ToUpper() && x.IsSpecial == isspecial && x.IsDeleted == CommonEnum.IsDeleted.No);
                //query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No);
                var list = query.ToList();
                var data = list.Select(x => new { x.Guid, x.ClassName });
                //var data = query.Select(x => new { x.CourseName, x.Code });
                response.SetData(data);
                return Ok(response);
            }
        }
        /// <summary>
        /// 创建班级
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("classgrade_create")]
        [OperationLog("班级管理控制器", "创建班级", "新增班级保存")]
        public IActionResult Create(ClassGradeCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.ClassName.Trim().Length <= 0)
            {
                response.SetFailed("请输入班级名称");
                return Ok(response);
            }
            if (model.TotalPeople <= 0)
            {
                response.SetFailed("请输入正确的额定总人数");
                return Ok(response);
            }
            //using (_dbContext)
            //{
                if (_dbContext.ClassGrade.Count(x => x.ClassName == model.ClassName && x.IsDeleted == CommonEnum.IsDeleted.No) > 0)
                {
                    response.SetFailed("该班级已存在，创建失败");
                    return Ok(response);
                }
                if (_dbContext.CourseSubject.Count(x => x.Code == model.CourseCode) <= 0)
                {
                    response.SetFailed("参数错误，创建失败");
                    return Ok(response);
                }
                var entity = new ClassGrade();
                entity.ClassName = model.ClassName;
                entity.CourseCode = model.CourseCode;
                entity.Memo = model.Memo;
                entity.TotalPeople = model.TotalPeople;
                entity.IsSpecial = model.IsSpecial;
                entity.CreatedOn = DateTime.Now;
                entity.Guid = Guid.NewGuid();
                entity.IsDeleted = CommonEnum.IsDeleted.No;
                _dbContext.ClassGrade.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            //}
        }

        /// <summary>
        /// 编辑班级
        /// </summary>
        /// <param name="guid">用户GUID</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("classgrade_view")]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.ClassGrade.FirstOrDefault(x => x.Guid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(new ClassGradeCreateOrEditViewModel()
                {
                    Guid = entity.Guid,
                    ClassName = entity.ClassName,
                    CourseCode = entity.CourseCode,
                    TotalPeople = entity.TotalPeople,
                    IsSpecial = entity.IsSpecial,
                    Memo = entity.Memo
                });
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的班级信息
        /// </summary>
        /// <param name="model">用户视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("classgrade_edit")]
        [OperationLog("班级管理控制器", "编辑班级", "编辑班级保存")]
        public IActionResult Edit(ClassGradeCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            if (model.ClassName.Trim().Length <= 0)
            {
                response.SetFailed("请输入班级名称");
                return Ok(response);
            }
            if (model.TotalPeople <= 0)
            {
                response.SetFailed("请输入正确的额定总人数");
                return Ok(response);
            }
            //using (_dbContext)
            //{
                if (_dbContext.ClassGrade.Count(x => x.Guid != model.Guid && x.ClassName == model.ClassName && x.IsDeleted == CommonEnum.IsDeleted.No) > 0)
                {
                    response.SetFailed("该班级已存在，修改失败");
                    return Ok(response);
                }
                if (_dbContext.CourseSubject.Count(x => x.Code == model.CourseCode) <= 0)
                {
                    response.SetFailed("参数错误，修改失败");
                    return Ok(response);
                }
                var entity = _dbContext.ClassGrade.FirstOrDefault(x => x.Guid == model.Guid);
                if (entity == null)
                {
                    response.SetFailed("参数错误，修改失败");
                    return Ok(response);
                }
                if (entity.IsSpecial != model.IsSpecial)
                {
                    if (entity.IsSpecial == CommonEnum.YesOrNo.No && entity.ClassGradeCourseSchedule.Any())
                    {
                        response.SetFailed("该班级有分配课程，无法修改为特约班");
                        return Ok(response);
                    }
                    else if (entity.IsSpecial == CommonEnum.YesOrNo.Yes && entity.CourseHours.Any(x => x.CourseHourCourseSchedule.Any()))
                    {
                        response.SetFailed("该班级有分配课程，无法修改为普通班");
                        return Ok(response);
                    }
                    else
                    {
                        entity.IsSpecial = model.IsSpecial;
                    }
                }
                if (entity.CourseCode != model.CourseCode)
                {
                    if (entity.CourseHours.Count > 0)
                    {
                        response.SetFailed("该班级还有在班学员，无法修改课程科目");
                        return Ok(response);
                    }
                    else
                    { 
                        entity.CourseCode = model.CourseCode;
                    }
                }
                entity.ClassName = model.ClassName;
                entity.Memo = model.Memo;
                entity.TotalPeople = model.TotalPeople;
                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            //}
        }

        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            response.SetFailed("该功能接口暂未完善");
            return Ok(response);
        }

        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Recover(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            response.SetFailed("该功能接口暂未完善");
            return Ok(response);
        }
    }
}
