using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using DncZeus.Api.RequestPayload.教务中心.CourseSchedule;
using DncZeus.Api.ViewModels.教务中心.CourseSchedule;
using System.Collections.Generic;
using DncZeus.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Authorization;

namespace DncZeus.Api.Controllers.Api.V1.教务中心
{
    /// <summary>
    /// 课表管理
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CourseScheduleController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public CourseScheduleController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 课表列表加载
        /// </summary>
        /// <param name="payload">课表列表数据加载请求载体</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize("courseschedule_view")]
        public IActionResult List(CourseScheduleRequestPayload payload)
        {
            using (_dbContext)
            {
                var now = DateTime.Now;
                var query = _dbContext.CourseSchedule.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Memo.Contains(payload.Kw.Trim()));
                                        
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDeleted == payload.IsDeleted);
                }
                if (payload.IsEnabled >= CommonEnum.IsEnabled.No)
                {
                    query = query.Where(x => x.IsEnabled == payload.IsEnabled);
                }
                if ((int)payload.DayOfWeek > -1)
                {
                    query = query.Where(x => x.DayOfWeek == payload.DayOfWeek);
                }
                if (payload.ClassGradeGuid.HasValue && payload.ClassGradeGuid != Guid.Empty)
                {
                    query = query.Where(x => x.ClassGradeCourseSchedule.Any(a => a.ClassGradeGuid == payload.ClassGradeGuid));
                }
                if (!string.IsNullOrEmpty(payload.CourseCode))
                {
                    query = query.Where(x => x.CourseCode == payload.CourseCode);
                }
                if (payload.TeacherGuid.HasValue && payload.TeacherGuid != Guid.Empty)
                {
                    query = query.Where(x => x.TeacherGuid == payload.TeacherGuid);
                }
                if (payload.StartDate != null && payload.EndDate != null)
                {
                    query = query.Where(x => payload.StartDate <= x.EndDate && payload.EndDate >= x.StartDate);
                }
                if (payload.StartTime != null && payload.EndTime != null)
                {
                    query = query.Where(x => payload.StartTime.Value.TimeOfDay <= x.EndTime && payload.EndTime.Value.TimeOfDay >= x.StartTime);
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize)
                    .Include(x => x.Teacher).ToList();
                    
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<CourseSchedule, CourseScheduleJsonModel>);

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 课表日程表网格数据加载
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize("courseschedule_view")]
        public IActionResult CalendarGrid(CourseScheduleRequestPayload payload)
        {
            using (_dbContext)
            {
                var now = DateTime.Now;
                
                var query = _dbContext.CourseSchedule.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Memo.Contains(payload.Kw.Trim()));

                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDeleted == payload.IsDeleted);
                }
                if (payload.IsEnabled >= CommonEnum.IsEnabled.No)
                {
                    query = query.Where(x => x.IsEnabled == payload.IsEnabled);
                }
                if ((int)payload.DayOfWeek > -1)
                {
                    query = query.Where(x => x.DayOfWeek == payload.DayOfWeek);
                }
                if (payload.ClassGradeGuid.HasValue && payload.ClassGradeGuid != Guid.Empty)
                {
                    query = query.Where(x => x.ClassGradeCourseSchedule.Any(a => a.ClassGradeGuid == payload.ClassGradeGuid));
                }
                if (!string.IsNullOrEmpty(payload.CourseCode))
                {
                    query = query.Where(x => x.CourseCode == payload.CourseCode);
                }
                if (payload.TeacherGuid.HasValue && payload.TeacherGuid != Guid.Empty)
                {
                    query = query.Where(x => x.TeacherGuid == payload.TeacherGuid);
                }
                int day = 0;
                if (payload.StartDate != null && payload.EndDate != null)
                {
                    if (now.Date == payload.StartDate.Value.Date && now.Date == payload.EndDate.Value.Date)
                    {
                        payload.EndDate.Value.AddDays(+6);
                    }
                    //query = query.Where(x => payload.StartDate <= x.EndDate && payload.EndDate >= x.StartDate);
                    //day = (int)(payload.StartDate.Value.Date - payload.EndDate.Value.Date).TotalDays;
                }
                else
                {
                    payload.StartDate = now.Date;
                    payload.EndDate = now.Date.AddDays(+6);
                }
                query = query.Where(x => payload.StartDate <= x.EndDate && payload.EndDate >= x.StartDate);
                day = (int)(payload.EndDate.Value.Date - payload.StartDate.Value.Date).TotalDays;
                if (payload.StartTime != null && payload.EndTime != null)
                {
                    query = query.Where(x => payload.StartTime.Value.TimeOfDay <= x.EndTime && payload.EndTime.Value.TimeOfDay >= x.StartTime);
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Include(x => x.Teacher).ToList();
                var gridItem = new List<CalendarGridItem>();

                for (int i = 0; i <= day; i++)
                {
                    var datetemp = payload.StartDate.Value.Date.AddDays(i);
                    foreach (var item in list)
                    {
                        if (item.StartDate.Date <= datetemp && datetemp <= item.EndDate.Date 
                            && ((int)datetemp.DayOfWeek == (int)item.DayOfWeek || item.DayOfWeek == ScheduleOfWeek.特约课))
                        {
                            gridItem.Add(new CalendarGridItem()
                            {
                                Color = item.BackColor,
                                Title = item.CourseName,
                                Start = datetemp + item.StartTime,
                                End = datetemp + item.EndTime,
                                CourseCode = item.CourseCode,
                                Guid = item.Guid,
                            });
                        }
                    }
                }
                var data = new CourseScheduleCalendarGridViewModel();
                data.WeekDay = (int)payload.StartDate.Value.DayOfWeek;
                data.GridItem = gridItem;
                data.InitialDate = payload.StartDate.Value.Date;
                data.TextColor = "#FFFFFF";
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, gridItem.Count);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取课程详情
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [CustomAuthorize("courseschedule_view")]
        public IActionResult GetCourseDetail(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.CourseSchedule.Where(x => x.Guid == guid)
                    .Select(x => new
                    {
                        ClassNameList = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade.ClassName).ToList(),
                        TraineesNameList = x.CourseHourCourseSchedule.Select(s => s.CourseHour.Trainees.FullName).ToList(),
                        TeacherName = x.Teacher.FullName,
                        ClassRoomName = x.ClassRoomName,
                    }).First();
                    
                var response = ResponseModelFactory.CreateInstance;
                string className = string.Empty;
                string traineesName = string.Empty;
                entity.ClassNameList.ForEach(x => className = className + x + ",");
                entity.TraineesNameList.ForEach(x => traineesName = traineesName + x + ",");
                var data = new { teacherName = entity.TeacherName, className = className.TrimEnd(','), traineesName = traineesName.TrimEnd(','), classRoomName = entity.ClassRoomName ?? string.Empty };
                response.SetData(data);
                return Ok(response);
            }
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("courseschedule_create")]
        [OperationLog("课表控制器", "创建课表", "新增课程安排")]
        public IActionResult Create(CourseScheduleCreateViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.StartDate == DateTime.MinValue 
                || model.StartDate == DateTime.MaxValue 
                || model.StartDate == null 
                || model.EndDate == DateTime.MinValue 
                || model.EndDate == DateTime.MaxValue 
                || model.EndDate == null)
            {
                response.SetFailed("请选择上课日期");
                return Ok(response);
            }
            if (model.StartTime == null || model.EndTime == null || model.StartTime == model.EndTime)
            {
                response.SetFailed("请选择上课时间");
                return Ok(response);
            }
            if (string.IsNullOrEmpty(model.CourseCode))
            {
                response.SetFailed("请选择课程科目");
                return Ok(response);
            }
            if (model.TeacherGuid == Guid.Empty)
            {
                response.SetFailed("请选择上课教师");
                return Ok(response);
            }
            
            //using (_dbContext)
            //{
                string courseName = _dbContext.CourseSubject.FirstOrDefault(x => x.Code == model.CourseCode).CourseName;
                if (string.IsNullOrEmpty(courseName))
                {
                    response.SetFailed("参数错误,添加失败");
                    return Ok(response);
                }
                model.StartDate = model.StartDate.Date;
                model.EndDate = model.EndDate.Date;
                var entity = _mapper.Map<CourseScheduleCreateViewModel, CourseSchedule>(model);
                entity.CourseName = courseName;
                entity.CreatedOn = DateTime.Now;
                entity.Guid = Guid.NewGuid();
                entity.IsDeleted = CommonEnum.IsDeleted.No;
                _dbContext.CourseSchedule.Add(entity);
                if (model.ClassGradeGuids.Any())
                {
                    var classGradeCourseScheduleList = new List<ClassGradeCourseScheduleMapping>();
                    foreach (var item in model.ClassGradeGuids)
                    {
                        classGradeCourseScheduleList.Add(new ClassGradeCourseScheduleMapping()
                        {
                            ClassGradeGuid = item,
                            CourseScheduleGuid = entity.Guid,
                            CreatedOn = DateTime.Now
                        });
                    }
                    _dbContext.ClassGradeCourseScheduleMapping.AddRangeAsync(classGradeCourseScheduleList);
                }
                if (model.CourseHourGuids.Any())
                {
                    var courseHourCourseScheduleList = new List<CourseHourCourseScheduleMapping>();
                    foreach (var item in model.CourseHourGuids)
                    {
                        courseHourCourseScheduleList.Add(new CourseHourCourseScheduleMapping()
                        {
                            CourseHourGuid = item,
                            CourseScheduleGuid = entity.Guid,
                            CreatedOn = DateTime.Now
                        });
                    }
                    _dbContext.CourseHourCourseScheduleMapping.AddRangeAsync(courseHourCourseScheduleList);
                }
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            //}
        }
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("courseschedule_view")]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.CourseSchedule.FirstOrDefault(x => x.Guid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<CourseSchedule, CourseScheduleEditViewModel>(entity));
                return Ok(response);
            }
        }
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("courseschedule_edit")]
        [OperationLog("课表控制器", "编辑课表", "修改课程安排")]
        public IActionResult Edit(CourseScheduleEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.StartDate == DateTime.MinValue
                || model.StartDate == DateTime.MaxValue
                || model.StartDate == null
                || model.EndDate == DateTime.MinValue
                || model.EndDate == DateTime.MaxValue
                || model.EndDate == null)
            {
                response.SetFailed("请选择上课日期");
                return Ok(response);
            }
            if (model.StartTime == null || model.EndTime == null || model.StartTime == model.EndTime)
            {
                response.SetFailed("请选择上课时间");
                return Ok(response);
            }
            if (model.TeacherGuid == Guid.Empty)
            {
                response.SetFailed("请选择上课教师");
                return Ok(response);
            }

            //using (_dbContext)
            //{
                var entity = _dbContext.CourseSchedule
                    //.Include(x => x.ClassGradeCourseSchedule)
                    //.Include(x => x.CourseHourCourseSchedule)
                    .FirstOrDefault(x => x.Guid == model.Guid);
                if (entity == null)
                {
                    response.SetFailed("参数错误，修改失败");
                    return Ok(response);
                }
                entity.TeacherGuid = model.TeacherGuid;
                entity.DayOfWeek = model.DayOfWeek;
                entity.IsEnabled = model.IsEnabled;
                entity.LoopOfYear = model.LoopOfYear;
                entity.StartDate = model.StartDate;
                entity.EndDate = model.EndDate;
                entity.StartTime = model.StartTime;
                entity.EndTime = model.EndTime;
                entity.Memo = model.Memo;
                entity.BackColor = model.BackColor;
                entity.ClassRoomName = model.ClassRoomName;
                //var classGradeCourseScheduleList = new List<ClassGradeCourseScheduleMapping>();
                //foreach (var item in model.ClassGradeGuids)
                //{
                //    classGradeCourseScheduleList.Add(new ClassGradeCourseScheduleMapping()
                //    {
                //        ClassGradeGuid = item,
                //        CourseScheduleGuid = entity.Guid,
                //        CreatedOn = DateTime.Now
                //    });
                //}
                //_dbContext.Database.ExecuteSqlCommand("DELETE FROM ClassGradeCourseScheduleMapping WHERE CourseScheduleGuid={0}", entity.Guid);
                //_dbContext.ClassGradeCourseScheduleMapping.AddRangeAsync(classGradeCourseScheduleList);
                //var courseHourCourseScheduleList = new List<CourseHourCourseScheduleMapping>();
                //foreach (var item in model.CourseHourGuids)
                //{
                //    courseHourCourseScheduleList.Add(new CourseHourCourseScheduleMapping()
                //    {
                //        CourseHourGuid = item,
                //        CourseScheduleGuid = entity.Guid,
                //        CreatedOn = DateTime.Now
                //    });
                //}
                //_dbContext.Database.ExecuteSqlCommand("DELETE FROM CourseHourCourseScheduleMapping WHERE CourseScheduleGuid={0}", entity.Guid);
                //_dbContext.CourseHourCourseScheduleMapping.AddRangeAsync(courseHourCourseScheduleList);
                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            //}
        }

        /// <summary>
        /// 保存班级-课表，学员课时-课表 关系映射
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize("courseschedule_assign")]
        [OperationLog("课表控制器", "课表分配", "课程学员分配")]
        public IActionResult SaveAssignCoursesChedule(SaveAssignCourseScheduleViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            var classGradeCourseScheduleList = model.AssignedClassGrades.Select(x => new ClassGradeCourseScheduleMapping
            {
                CourseScheduleGuid = model.CourseScheduleGuid,
                CreatedOn = DateTime.Now,
                ClassGradeGuid = x,
            }).ToList();
            var courseHoursCourseScheduleList = model.AssignedCourseHours.Select(x => new CourseHourCourseScheduleMapping
            {
                CourseScheduleGuid = model.CourseScheduleGuid,
                CreatedOn = DateTime.Now,
                CourseHourGuid = x,
            }).ToList();
            //using (_dbContext)
            //{
                _dbContext.Database.ExecuteSqlCommand("DELETE FROM ClassGradeCourseScheduleMapping WHERE CourseScheduleGuid={0}", model.CourseScheduleGuid);
                var success = true;
                if (classGradeCourseScheduleList.Count > 0)
                {
                    _dbContext.ClassGradeCourseScheduleMapping.AddRange(classGradeCourseScheduleList);
                }
                _dbContext.Database.ExecuteSqlCommand("DELETE FROM CourseHourCourseScheduleMapping WHERE CourseScheduleGuid={0}", model.CourseScheduleGuid);
                if (courseHoursCourseScheduleList.Count > 0)
                {
                    _dbContext.CourseHourCourseScheduleMapping.AddRange(courseHoursCourseScheduleList);
                }
                success = _dbContext.SaveChanges() > 0;
                if (success)
                {
                    response.SetSuccess();
                }
                else
                {
                    response.SetFailed("保存分配课表数据失败");
                }
                return Ok(response);
            //}
        }
        /// <summary>
        /// 获取今日待签到课程列表信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize("signindesk_view")]
        public IActionResult GetSignInDeskListAndStatis()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var now = DateTime.Now;
                int dayOfWeek = (int)now.DayOfWeek;
                var query = _dbContext.CourseSchedule.AsQueryable().AsNoTracking();
                query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No);
                query = query.Where(x => x.IsEnabled == CommonEnum.IsEnabled.Yes);
                query = query.Where(x => x.DayOfWeek == ScheduleOfWeek.特约课 || x.DayOfWeek == (ScheduleOfWeek)dayOfWeek);
                query = query.Where(x => now.Date <= x.EndDate && now.Date >= x.StartDate);
                query = query.OrderBy("StartTime", false);
                var list = query.Include(x => x.Teacher)
                                .Include(x => x.TeacherAttences)
                                .Include(x => x.TraineesAttences)
                                .Include(x => x.CourseHourCourseSchedule)
                                .Include(x => x.ClassGradeCourseSchedule)
                                .ThenInclude(x => x.ClassGrade.CourseHours)
                                .Select(x => new
                                {
                                    CourseSchedule = x,
                                    TeacherAttences = x.TeacherAttences.Where(p => now.Date <= p.CreatedOn && now.AddDays(1).Date > p.CreatedOn),
                                    TraineesAttences = x.TraineesAttences.Where(p => now.Date <= p.CreatedOn && now.AddDays(1).Date > p.CreatedOn),
                                    CourseHourCourseSchedule = x.CourseHourCourseSchedule,
                                    ClassGradeCourseSchedule = x.ClassGradeCourseSchedule,
                                    ClassGrade = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade),
                                    CourseHours = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade.CourseHours),
                                })
                                .ToList()
                                .Select(x => x.CourseSchedule)
                                .ToList();
                var listItem = new List<SignInDeskListItemViewModel>();
                var statis = new SignInDeskStatisViewModel();
                foreach (var item in list)
                {
                    bool isAttend = item.TeacherAttences.Any(a => a.IsAttend == CommonEnum.YesOrNo.Yes || a.IsSubstitute == CommonEnum.YesOrNo.Yes);
                    int signInTrainees = item.TraineesAttences.Count(p => p.IsAttend == CommonEnum.YesOrNo.Yes);
                    int courseHourCount = item.ClassGradeCourseSchedule.Select(s => s.ClassGrade.CourseHours.Count).Sum() + item.CourseHourCourseSchedule.Count;
                    int traineesAttencesCount = item.TraineesAttences.Count;
                    listItem.Add(new SignInDeskListItemViewModel()
                    {
                        CourseScheduleGuid = item.Guid,
                        Title = item.CourseName + "-" + item.Teacher.FullName,
                        Description = string.Format("上课时间{0}-{1} 学员总人数{2}人 教师{3} 已签到学员{4}人",
                                                    item.StartTime.ToString(@"hh\:mm"),
                                                    item.EndTime.ToString(@"hh\:mm"),
                                                    isAttend ? traineesAttencesCount : courseHourCount,
                                                    isAttend ? "已签到" : "未签到",
                                                    signInTrainees),
                    });
                    statis.SignInTeacher += isAttend ? 1 : 0;
                    statis.ReadyToSignInTeacher += isAttend ? 0 : 1;
                    statis.SignInTrainees += signInTrainees;
                    statis.ReadyToSignInTrainees += isAttend ? (traineesAttencesCount - signInTrainees) : courseHourCount;
                }
                //statis.SignInTeacher = list.Count(x => x.TeacherAttences.Any(a => a.IsAttend == CommonEnum.YesOrNo.Yes));
                //statis.ReadyToSignInTeacher = list.Count - statis.SignInTeacher;
                //statis.SignInTrainees = list.Sum(x => x.TraineesAttences.Count(p => p.IsAttend == CommonEnum.YesOrNo.Yes));
                //statis.ReadyToSignInTrainees = list.Sum(x => x.ClassGradeCourseSchedule.Select(s => s.ClassGrade.CourseHours.Count).Sum() + x.CourseHourCourseSchedule.Count) - statis.SignInTrainees;
                response.SetData(new { listItem = listItem, statis = statis});
                return Ok(response);
            }
        }
        /// <summary>
        /// 获取今日签到数据统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize("signindesk_view")]
        public IActionResult GetSignInDeskStatis()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var now = DateTime.Now;
                int dayOfWeek = (int)now.DayOfWeek;
                var query = _dbContext.CourseSchedule.AsQueryable().AsNoTracking();
                query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No);
                query = query.Where(x => x.IsEnabled == CommonEnum.IsEnabled.Yes);
                query = query.Where(x => x.DayOfWeek == ScheduleOfWeek.特约课 || x.DayOfWeek == (ScheduleOfWeek)dayOfWeek);
                query = query.Where(x => now.Date <= x.EndDate && now.Date >= x.StartDate);
                var list = query.Include(x => x.TeacherAttences)
                                .Include(x => x.TraineesAttences)
                                .Include(x => x.CourseHourCourseSchedule)
                                .Include(x => x.ClassGradeCourseSchedule)
                                .ThenInclude(x => x.ClassGrade.CourseHours)
                                .Select(x => new
                                {
                                    CourseSchedule = x,
                                    TeacherAttences = x.TeacherAttences.Where(p => now.Date <= p.CreatedOn && now.AddDays(1).Date > p.CreatedOn),
                                    TraineesAttences = x.TraineesAttences.Where(p => now.Date <= p.CreatedOn && now.AddDays(1).Date > p.CreatedOn),
                                    CourseHourCourseSchedule = x.CourseHourCourseSchedule,
                                    ClassGradeCourseSchedule = x.ClassGradeCourseSchedule,
                                    ClassGrade = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade),
                                    CourseHours = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade.CourseHours),
                                })
                                .ToList()
                                .Select(x => x.CourseSchedule)
                                .ToList();
                //var list = query.Select(x => new 
                //{
                //    SignInTeacher = x.TeacherAttences.Any(a => a.IsAttend == CommonEnum.YesOrNo.Yes) ? 1 : 0,
                //    ReadyToSignInTeacher = 1,
                //    ReadyToSignInTrainees = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade.CourseHours.Count).Sum() + x.CourseHourCourseSchedule.Count,
                //    SignInTrainees = x.TraineesAttences.Count(p => p.IsAttend == CommonEnum.YesOrNo.Yes),
                //}).ToList();
                var data = new SignInDeskStatisViewModel();
                foreach (var item in list)
                {
                    bool isAttend = item.TeacherAttences.Any(a => a.IsAttend == CommonEnum.YesOrNo.Yes || a.IsSubstitute == CommonEnum.YesOrNo.Yes);
                    int signInTrainees = item.TraineesAttences.Count(p => p.IsAttend == CommonEnum.YesOrNo.Yes);
                    data.SignInTeacher += isAttend ? 1 : 0;
                    data.ReadyToSignInTeacher += isAttend ? 0 : 1;
                    data.SignInTrainees += signInTrainees;
                    data.ReadyToSignInTrainees += isAttend ? (item.TraineesAttences.Count - signInTrainees) : (item.ClassGradeCourseSchedule.Select(s => s.ClassGrade.CourseHours.Count).Sum() + item.CourseHourCourseSchedule.Count);
                }
                //data.SignInTeacher = list.Count(x => x.TeacherAttences.Any(a => a.IsAttend == CommonEnum.YesOrNo.Yes));
                //data.ReadyToSignInTeacher = list.Count - data.SignInTeacher;
                //data.SignInTrainees = list.Sum(x => x.TraineesAttences.Count(p => p.IsAttend == CommonEnum.YesOrNo.Yes));
                //data.ReadyToSignInTrainees = list.Sum(x => x.ClassGradeCourseSchedule.Select(s => s.ClassGrade.CourseHours.Count).Sum() + x.CourseHourCourseSchedule.Count) - data.SignInTrainees;
                response.SetData(data);
                return Ok(response);
            }
        }
        #region 内部方法
        /// <summary>
        /// 学员课程时间冲突验证
        /// </summary>
        /// <param name="TraineesGuid"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="DayOfWeek"></param>
        /// <returns>返回true表示有冲突</returns>
        private bool TraineesIsTimeConflict(List<Guid> TraineesGuid, DateTime StartDate, DateTime EndDate, TimeSpan StartTime, TimeSpan EndTime, ScheduleOfWeek DayOfWeek)
        {
            bool flag = true;
            using (_dbContext)
            {
                flag = _dbContext.CourseSchedule.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No)
                                         .Where(x => x.IsEnabled == CommonEnum.IsEnabled.Yes)
                                         .Where(x => x.DayOfWeek == DayOfWeek)
                                         .Where(x => StartDate <= x.EndDate && EndDate >= x.StartDate)
                                         .Where(x => StartTime <= x.EndTime && EndTime >= x.StartTime)
                                         .Where(x => x.CourseHourCourseSchedule.Any(a => TraineesGuid.Contains(a.CourseHour.TraineesGuid) && a.CourseHour.ExpiryDate >= DateTime.Now && a.CourseHour.Surplus > 0)
                                                    || x.ClassGradeCourseSchedule.Any(a => a.ClassGrade.CourseHours.Any(a => TraineesGuid.Contains(a.TraineesGuid) && a.ExpiryDate >=DateTime.Now && a.Surplus > 0)))
                                         .Any();
            }
            return flag;
        }
        /// <summary>
        /// 一个班级里面的学员时间冲突验证
        /// </summary>
        /// <param name="ClassGradeGuid"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="DayOfWeek"></param>
        /// <returns>返回冲突的学员信息</returns>
        private List<Trainees> TraineesIsTimeConflictOfClassGrade(List<Guid> ClassGradeGuid, DateTime StartDate, DateTime EndDate, TimeSpan StartTime, TimeSpan EndTime, ScheduleOfWeek DayOfWeek)
        {
            var traineesList = new List<Trainees>();
            using (_dbContext)
            {
                var query = _dbContext.Trainees.AsQueryable().AsNoTracking();
                traineesList = query.Where(x => x.CourseHours.Any(x => ClassGradeGuid.Any(a => a == x.ClassGradeGuid) && x.ExpiryDate >= DateTime.Now && x.Surplus > 0))
                                      .Where(x => x.CourseHours.Any(x => x.CourseHourCourseSchedule.Any(x =>
                                                                                                        x.CourseSchedule.DayOfWeek == DayOfWeek
                                                                                                        && StartDate <= x.CourseSchedule.EndDate
                                                                                                        && EndDate >= x.CourseSchedule.StartDate
                                                                                                        && StartTime <= x.CourseSchedule.EndTime
                                                                                                        && EndTime >= x.CourseSchedule.StartTime
                                                                                                        && x.CourseSchedule.IsDeleted == CommonEnum.IsDeleted.No
                                                                                                        && x.CourseSchedule.IsEnabled == CommonEnum.IsEnabled.Yes)))
                                      .Where(x => x.CourseHours.Any(x => x.ClassGrade.ClassGradeCourseSchedule.Any(x =>
                                                                                                                   x.CourseSchedule.DayOfWeek == DayOfWeek
                                                                                                                   && StartDate <= x.CourseSchedule.EndDate
                                                                                                                   && EndDate >= x.CourseSchedule.StartDate
                                                                                                                   && StartTime <= x.CourseSchedule.EndTime
                                                                                                                   && EndTime >= x.CourseSchedule.StartTime
                                                                                                                   && x.CourseSchedule.IsDeleted == CommonEnum.IsDeleted.No
                                                                                                                   && x.CourseSchedule.IsEnabled == CommonEnum.IsEnabled.Yes)))
                                      .ToList();
            }
            return traineesList;
        }
        #endregion
    }
}
