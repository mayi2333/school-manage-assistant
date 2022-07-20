using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using DncZeus.Api.RequestPayload.教务中心.CourseSubject;
using DncZeus.Api.ViewModels.教务中心.CourseSubject;
using DncZeus.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Authorization;

namespace DncZeus.Api.Controllers.Api.V1.教务中心
{
    /// <summary>
    /// 课程科目
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CourseSubjectController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;

        public CourseSubjectController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 课程科目列表加载
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize("coursesubject_view")]
        public IActionResult List(CourseSubjectRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.CourseSubject.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Code.Contains(payload.Kw.Trim())
                                        || x.CourseName.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDeleted == payload.IsDeleted);
                }
                if (payload.ChargeType > ChargeType.未指定)
                {
                    query = query.Where(x => x.ChargeType == payload.ChargeType);
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).Include(x => x.ClassGrades).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<CourseSubject, CourseSubjectJsonModel>);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 根据关键字查询科目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{kw}")]
        [CustomAuthorize("coursehour_view", "coursehour_create", "coursehour_edit")]
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

                var query = _dbContext.CourseSubject.Where(x => (x.CourseName.Contains(kw) || x.Code.Contains(kw.ToUpper()) && x.IsDeleted == CommonEnum.IsDeleted.No));
                //query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No);
                var list = query.ToList();
                var data = list.Select(x => new { x.Code, x.CourseName });
                //var data = query.Select(x => new { x.CourseName, x.Code });

                response.SetData(data);
                //System.Threading.Thread.Sleep(3000);
                return Ok(response);
            }
        }

        /// <summary>
        /// 创建课程科目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("coursesubject_create")]
        public IActionResult Create(CourseSubjectCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.CourseName.Trim().Length <= 0)
            {
                response.SetFailed("请输入科目名称");
                return Ok(response);
            }
            if (model.Price < 0)
            {
                response.SetFailed("请输入正确的金额");
                return Ok(response);
            }
            if (model.Code.Trim().Length <= 0 || !Utils.RegexHelper.Check(model.Code.Trim().ToUpper(), Utils.RegexHelper.Type.大写字母))
            {
                response.SetFailed("请输入正确的科目编码，必须为大写字母");
                return Ok(response);
            }
            using (_dbContext)
            {
                if (_dbContext.CourseSubject.Count(x => x.Code == model.Code.ToUpper()) > 0)
                {
                    response.SetFailed("该科目编码已存在，创建失败");
                    return Ok(response);
                }
                var entity = new CourseSubject();
                entity.Code = model.Code.Trim().ToUpper();
                entity.CourseName = model.CourseName.Trim();
                entity.Price = model.Price;
                entity.ChargeType = model.ChargeType;
                entity.CreatedOn = DateTime.Now;
                entity.IsDeleted = CommonEnum.IsDeleted.No;
                _dbContext.CourseSubject.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑课程科目
        /// </summary>
        /// <param name="guid">用户GUID</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("coursesubject_view")]
        public IActionResult Edit(string code)
        {
            using (_dbContext)
            {
                var entity = _dbContext.CourseSubject.FirstOrDefault(x => x.Code == code);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(new CourseSubjectCreateOrEditViewModel()
                {
                    Code = entity.Code,
                    ChargeType = entity.ChargeType,
                    CourseName = entity.CourseName,
                    Price = entity.Price
                });
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的课程科目
        /// </summary>
        /// <param name="model">用户视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("coursesubject_edit")]
        public IActionResult Edit(CourseSubjectCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            if (model.CourseName.Trim().Length <= 0)
            {
                response.SetFailed("请输入科目名称");
                return Ok(response);
            }
            if (model.Price < 0)
            {
                response.SetFailed("请输入正确的金额");
                return Ok(response);
            }
            if (model.Code.Trim().Length <= 0 || !Utils.RegexHelper.Check(model.Code.Trim().ToUpper(), Utils.RegexHelper.Type.大写字母))
            {
                response.SetFailed("参数错误，修改失败");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.CourseSubject.Include(x => x.CourseSchedules).FirstOrDefault(x => x.Code == model.Code);
                if (entity == null)
                {
                    response.SetFailed("参数错误，修改失败");
                    return Ok(response);
                }
                entity.CourseName = model.CourseName;
                entity.ChargeType = model.ChargeType;
                entity.Price = model.Price;
                entity.ModifiedOn = DateTime.Now;

                entity.CourseSchedules.ToList().ForEach(x => x.CourseName = entity.CourseName);

                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
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
