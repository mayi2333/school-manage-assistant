using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.AuthContext;
using DncZeus.Api.ViewModels.公众号.Personal;
using DncZeus.Api.ViewModels.教务中心.Trainees;
using DncZeus.Api.ViewModels.营销中心.AuditionCourse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DncZeus.Api.Controllers.Api.V1.公众号
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;
        public PersonalController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        ///// <summary>
        ///// 获取个人信息
        ///// </summary>
        ///// <returns></returns>
        //public IActionResult GetPersonalInfo(string wxopenid)
        //{
        //    return null;
        //}

        /// <summary>
        /// 获取宝贝信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTraineesList()
        {
            using (_dbContext)
            {
                var guid = AuthContextService.CurrentUser.Guid;
                var traineesList = _dbContext.Trainees.AsNoTracking()
                    .Where(x => x.CustomerGuid == guid && x.IsDeleted == CommonEnum.IsDeleted.No).Select(x => new { 
                        guid = x.Guid,
                        fullName = x.FullName,
                        telephone = x.Telephone
                    }).ToList();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(traineesList);
                return Ok(response);
            }
        }

        /// <summary>
        /// 添加或绑定宝贝信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddTrainees(TraineesCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.FullName.Trim().Length <= 0)
            {
                response.SetFailed("请输入宝贝姓名");
                return Ok(response);
            }
            if (model.Telephone.Trim().Length <= 0 || !Utils.RegexHelper.Check(model.Telephone, Utils.RegexHelper.Type.电话))
            {
                response.SetFailed("请输入正确的联系电话");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Trainees.FirstOrDefault(x => x.FullName == model.FullName && x.Telephone == model.Telephone && x.IsDeleted == CommonEnum.IsDeleted.No);
                if (entity != null)
                {
                    if (entity.RelationCust != null)
                    {
                        response.SetFailed("请勿重复添加宝贝信息");
                        return Ok(response);
                    }
                    else
                    {
                        entity.CustomerGuid = AuthContextService.CurrentUser.Guid;
                        _dbContext.SaveChanges();
                        response.SetSuccess();
                        return Ok(response);
                    }
                }
                if (model.Age < 0)
                {
                    response.SetFailed("请输入正确的年龄");
                    return Ok(response);
                }
                entity = _mapper.Map<TraineesCreateOrEditViewModel, Trainees>(model);
                entity.CreatedOn = DateTime.Now;
                entity.Guid = Guid.NewGuid();
                entity.IsDeleted = CommonEnum.IsDeleted.No;
                entity.CustomerGuid = AuthContextService.CurrentUser.Guid;
                entity.IdCardBindInfo = string.Empty;
                entity.Memo = "通过客户个人中心添加";
                _dbContext.Trainees.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 获取试听课预约信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAuditionCourseList()
        {
            using (_dbContext)
            {
                var guid = AuthContextService.CurrentUser.Guid;
                var auditionCourseList = _dbContext.AuditionCourse.AsNoTracking()
                                .Where(x => x.Trainees.CustomerGuid == guid && x.Trainees.IsDeleted == CommonEnum.IsDeleted.No)
                                .Include(x => x.Trainees)
                                .Include(x => x.ClassGrade)
                                .Include(x => x.CourseSubject)
                                .ToList();
                var data = auditionCourseList.Select(_mapper.Map<AuditionCourse, AuditionCourseJsonModel>);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(data);
                return Ok(response);
            }
        }
        /// <summary>
        /// 领取试听课
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddAuditionCourse(AddAuditionCourseViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.FullName.Trim().Length <= 0)
            {
                response.SetFailed("请输入宝贝姓名");
                return Ok(response);
            }
            if (model.Telephone.Trim().Length <= 0 || !Utils.RegexHelper.Check(model.Telephone, Utils.RegexHelper.Type.电话))
            {
                response.SetFailed("请输入正确的联系电话");
                return Ok(response);
            }
            using (_dbContext)
            {
                if (!_dbContext.CourseSubject.Any(x => x.Code == model.CourseCode && x.IsDeleted == CommonEnum.IsDeleted.No))
                {
                    response.SetFailed("所选科目不存在");
                    return Ok(response);
                }
                var entity = _dbContext.Trainees.FirstOrDefault(x => x.FullName == model.FullName && x.Telephone == model.Telephone && x.IsDeleted == CommonEnum.IsDeleted.No);
                if (entity != null)
                {
                    if (entity.RelationCust == null)
                    {
                        entity.CustomerGuid = AuthContextService.CurrentUser.Guid;
                        _dbContext.SaveChanges();
                    }
                    if (entity.AuditionCourses.Any(x => x.CourseCode == model.CourseCode))
                    {
                        response.SetFailed("领取失败,该课程已经领取过");
                        return Ok(response);
                    }
                }
                else
                {
                    if (model.Age < 0)
                    {
                        response.SetFailed("请输入正确的年龄");
                        return Ok(response);
                    }
                    entity.Guid = Guid.NewGuid();
                    entity.FullName = model.FullName;
                    entity.Telephone = model.Telephone;
                    entity.Address = model.Address;
                    entity.Age = model.Age;
                    entity.CreatedOn = DateTime.Now;
                    entity.IsDeleted = CommonEnum.IsDeleted.No;
                    entity.CustomerGuid = AuthContextService.CurrentUser.Guid;
                    entity.IdCardBindInfo = string.Empty;
                    entity.Memo = "通过客户个人中心领取试课时添加";
                    _dbContext.Trainees.Add(entity);
                    _dbContext.SaveChanges();
                }
                _dbContext.AuditionCourse.Add(new AuditionCourse()
                {
                    Guid = Guid.NewGuid(),
                    CourseCode = model.CourseCode,
                    CreatedOn = DateTime.Now,
                    State = CommonEnum.AuditionCourseState.初始提交,
                    TraineesGuid = entity.Guid
                });
                _dbContext.SaveChanges();
                response.SetSuccess("领取成功");
                return Ok(response);
            }
        }
        /// <summary>
        /// 获取课表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetCourseSchedule()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var courseScheduleList = _dbContext.Trainees.AsNoTracking()
                                     .Where(x => x.CustomerGuid == AuthContextService.CurrentUser.Guid)
                                     .Join(_dbContext.CourseHour, a => a.Guid, b => b.TraineesGuid, (a, b) => new {
                                         FullName = a.FullName,
                                         ClassGradeGuid = b.ClassGradeGuid,
                                         CourseHourGuid = b.Guid,
                                     }).GroupJoin(_dbContext.ClassGradeCourseScheduleMapping.Include(x => x.CourseSchedule), a => a.ClassGradeGuid, b => b.ClassGradeGuid, (a, b) => new {
                                         FullName = a.FullName,
                                         CourseHourGuid = a.CourseHourGuid,
                                         CourseSchedule = b.Select(x => x.CourseSchedule)
                                     }).GroupJoin(_dbContext.CourseHourCourseScheduleMapping.Include(x => x.CourseSchedule), a => a.CourseHourGuid, b => b.CourseHourGuid, (a, b) => new {
                                         FullName = a.FullName,
                                         CourseSchedule1 = a.CourseSchedule,
                                         CourseSchedule2 = b.Select(x => x.CourseSchedule)
                                     }).ToList();
                
                //.Include(x => x.CourseHours)
                //.ThenInclude(x => x.CourseHourCourseSchedule)
                //.ThenInclude(x => x.CourseSchedule)
                //.Include(x => x.CourseHours)
                //.ThenInclude(x => x.ClassGrade)
                //.ThenInclude(x => x.ClassGradeCourseSchedule)
                //.ThenInclude(x => x.CourseSchedule)
                //.Include(x => x.CourseHours)
                //.ToList();
                List<GetCourseScheduleViewModel> data = new List<GetCourseScheduleViewModel>();
                foreach (var item in courseScheduleList)
                {
                    GetCourseScheduleViewModel viewDateItem = new GetCourseScheduleViewModel();
                    foreach (var item1 in item.CourseSchedule1)
                    {
                        viewDateItem.FullName = item.FullName;
                        viewDateItem.DisplayTime = item1.DayOfWeek == ScheduleOfWeek.特约课 ? (item1.StartDate.ToString("yyyy年MM月dd日")
                                                    + " 至 " + item1.EndDate.ToString("yyyy年MM月dd日"))
                                                    : ("每周" + item1.DayOfWeek.ToString())
                                                    + " " + item1.StartTime.ToString(@"hh\:mm")
                                                    + "-" + item1.EndTime.ToString(@"hh\:mm")
                                                    + " " + item1.CourseName;
                        data.Add(viewDateItem);
                    }
                    foreach (var item2 in item.CourseSchedule2)
                    {
                        viewDateItem.FullName = item.FullName;
                        viewDateItem.DisplayTime = item2.DayOfWeek == ScheduleOfWeek.特约课 ? (item2.StartDate.ToString("yyyy年MM月dd日")
                                                    + " 至 " + item2.EndDate.ToString("yyyy年MM月dd日"))
                                                    : ("每周" + item2.DayOfWeek.ToString())
                                                    + " " + item2.StartTime.ToString(@"hh\:mm")
                                                    + "-" + item2.EndTime.ToString(@"hh\:mm")
                                                    + " " + item2.CourseName;
                        data.Add(viewDateItem);
                    }
                }
                response.SetData(data.GroupBy(x => x.FullName).ToDictionary(g => g.Key, g => g.Select(x => x.DisplayTime)));
            }
            return Ok(response);
        }
    }
}
