using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.AuthContext;
using DncZeus.Api.Extensions.CustomException;
using DncZeus.Api.Extensions.DataAccess;
using DncZeus.Api.Models;
using DncZeus.Api.Models.Response;
using DncZeus.Api.ViewModels.教务中心.SignInDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DncZeus.Api.Controllers.Api.V1.教务中心
{
    /// <summary>
    /// 签到台
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SignInDeskController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;

        public SignInDeskController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 刷卡签到
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        [HttpGet("{card}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("signindesk_view")]
        public IActionResult SignInByCard(string card)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(card))
            {
                response.SetFailed("参数错误");
                return Ok(response);
            }
            var now = DateTime.Now;
            using (_dbContext)
            {
                var attenceList = _dbContext.TraineesAttence.Where(x => x.Trainees.IdCardBindInfo == card.Trim() && now.Date <= x.CreatedOn && now.AddDays(1).Date > x.CreatedOn)
                                                            .OrderBy(k => k.CourseSchedule.StartTime)
                                                            .ToList();
                if (attenceList != null && attenceList.Count > 0)
                {
                    var attence = attenceList.FirstOrDefault(x => x.IsAttend == CommonEnum.YesOrNo.No);
                    if (attence != null)
                    {
                        attence.AttenceTime = now;
                        attence.IsAttend = CommonEnum.YesOrNo.Yes;
                        attence.AttenceType = CommonEnum.AttenceType.刷卡;
                        if (attence.CourseHour.CourseSubject.ChargeType != ChargeType.按学期)
                        {
                            attence.IsDeduct = CommonEnum.YesOrNo.Yes;
                            attence.CourseHour.Surplus -= 1;
                        }
                        //准备发送签到通知
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.TemplateId.Attend) && attence.Trainees.CustomerGuid != null && !string.IsNullOrEmpty(attence.Trainees.RelationCust.WxOpenid))
                        {
                            string fullName = attence.Trainees.FullName;
                            string className = attence.CourseHour.ClassGrade.ClassName;
                            string dateTime = string.Format("{0}({1}-{2})", now.ToString("yyyy年MM月dd日"), attence.CourseSchedule.StartTime.ToString(@"hh\:mm"), attence.CourseSchedule.EndTime.ToString(@"hh\:mm"));
                            string deduct = (attence.IsDeduct == CommonEnum.YesOrNo.Yes) ? "1" : "0";
                            string surplus = attence.CourseHour.Surplus.ToString();

                            _dbContext.TemplateMsg.Add(new TemplateMsg()
                            {
                                Guid = Guid.NewGuid(),
                                TemplateId = ConfigurationManager.AppSettings.TemplateId.Attend,
                                ToUser = attence.Trainees.RelationCust.WxOpenid,
                                SendNum = 0,
                                SendStatus = TemplateMsgSendStatus.未发送,
                                Data = JsonConvert.SerializeObject(new TemplateMsgAttendModel(fullName, className, dateTime, deduct, surplus)),
                                MsgId = string.Empty,
                                CreatedOn = now,
                                Timing = now,
                            });
                        }
                        _dbContext.SaveChanges();
                        response.SetSuccess("签到成功");
                        return Ok(response);
                    }
                    else
                    {
                        response.SetFailed("请勿重复签到");
                        return Ok(response);
                    }
                }
                var teacher = _dbContext.Teacher.Include(x => x.CourseSchedules)
                                                .FirstOrDefault(x => x.IdCardBindInfo == card.Trim() && x.IsDeleted == CommonEnum.IsDeleted.No);
                if (teacher != null)
                {
                    int dayOfWeek = (int)now.DayOfWeek;
                    var coursesScheduleList = teacher.CourseSchedules.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No)
                                           .Where(x => x.IsEnabled == CommonEnum.IsEnabled.Yes)
                                           .Where(x => x.DayOfWeek == ScheduleOfWeek.特约课 || x.DayOfWeek == (ScheduleOfWeek)dayOfWeek)
                                           .Where(x => now.Date <= x.EndDate && now.Date >= x.StartDate)
                                           .OrderBy(k => k.StartTime)
                                           .ToList();
                    if (coursesScheduleList != null && coursesScheduleList.Count > 0)
                    {
                        var teacherAttence = coursesScheduleList.FirstOrDefault(x => x.TeacherAttences.Any(p => p.IsAttend == CommonEnum.YesOrNo.No && p.InverseParentGuidNavigation == null))?
                                                                .TeacherAttences.FirstOrDefault();
                        if (teacherAttence != null)
                        {
                            teacherAttence.IsAttend = CommonEnum.YesOrNo.Yes;
                            teacherAttence.AttenceTime = now;
                            teacherAttence.AttenceType = CommonEnum.AttenceType.刷卡;
                            _dbContext.SaveChanges();
                            response.SetSuccess("签到成功");
                            return Ok(response);
                        }
                        var coursesSchedule = coursesScheduleList.FirstOrDefault(x => !x.TeacherAttences.Any());
                        if (coursesSchedule != null)
                        {
                            if (CreateTeacherAttence(_dbContext.CourseSchedule.AsQueryable().AsNoTracking().Where(x => x.Guid == coursesSchedule.Guid)
                                .Include(x => x.CourseHourCourseSchedule)
                                .ThenInclude(x => x.CourseHour)
                                .Include(x => x.ClassGradeCourseSchedule)
                                .ThenInclude(x => x.ClassGrade.CourseHours)
                                .Select(x => new
                                {
                                    CourseSchedule = x,
                                    CourseHourCourseSchedule = x.CourseHourCourseSchedule,
                                    ClassGradeCourseSchedule = x.ClassGradeCourseSchedule,
                                    ClassGrade = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade),
                                    CourseHours = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade.CourseHours),
                                    CourseHour = x.CourseHourCourseSchedule.Select(s => s.CourseHour),
                                })
                                .ToList()
                                .Select(x => x.CourseSchedule)
                                .FirstOrDefault(), CommonEnum.AttenceType.刷卡))
                            {
                                response.SetSuccess("签到成功");
                            }
                            else
                            {
                                response.SetFailed("系统繁忙,签到失败");
                            }
                            return Ok(response);
                        }
                        response.SetFailed("请勿重复签到");
                        return Ok(response);
                    }
                    else
                    {
                        response.SetFailed("今日没有该教师的课程");
                        return Ok(response);
                    }
                }
                if (_dbContext.Trainees.Any(x => x.IdCardBindInfo == card.Trim() && x.IsDeleted == CommonEnum.IsDeleted.No))
                {
                    response.SetFailed("教师还未签到，或者今日没有课程");
                    return Ok(response);
                }
                else
                {
                    response.SetFailed("此卡还未绑定任何人");
                    return Ok(response);
                }
            }
        }


        /// <summary>
        /// 刷脸签到
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("signindesk_view")]
        public IActionResult SignInByFace(SignInByFaceViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(model.img))
            {
                response.SetFailed("参数错误");
                return Ok(response);
            }
            string base64 = model.img.Substring(model.img.IndexOf(',') + 1);
            var now = DateTime.Now;
            using (_dbContext)
            {
                if (FaceServer.FaceEncodingCount <= 0)
                {
                    var m_dic = _dbContext.FaceFeature.AsNoTracking().Where(x => (x.Teacher != null || x.Trainees != null)).ToDictionary(key => key.FaceEncodes, val => val.Guid);
                    FaceServer.LoadFaceEncodings(m_dic);
                }
                if (FaceServer.FaceCheck(base64) > 0)
                {
                    int dayOfWeek = (int)now.DayOfWeek;
                    //TODO:业务逻辑暂未完善
                    try
                    {
                        var faceGuidList = FaceServer.FaceRecognitionForImage(base64).Select(x => x.Key).ToList();
                        var teacherOrTraineesList = _dbContext.FaceFeature.Where(x => faceGuidList.Contains(x.Guid))
                                                                    //.Include(x => x.Trainees)
                                                                    //.ThenInclude(x => x.TraineesAttences)
                                                                    //.Include(x => x.Teacher)
                                                                    //.ThenInclude(x => x.TeacherAttences)
                                                                    .Include(x => x.Teacher)
                                                                    .ThenInclude(x => x.CourseSchedules)
                                                                    .Select(x => new
                                                                    {
                                                                        Trainees = x.Trainees,
                                                                        Teacher = x.Teacher,
                                                                    }).ToList();
                        var teacherList = teacherOrTraineesList.Where(x => x.Teacher != null).Select(x => x.Teacher).ToList();
                        var traineesList = teacherOrTraineesList.Where(x => x.Trainees != null).Select(x => x.Trainees).ToList();
                        int successNum = 0;
                        string res = string.Empty;
                        if (teacherList.Count > 0)
                        {
                            foreach (var teacher in teacherList)
                            {

                                var coursesScheduleList = teacher.CourseSchedules.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No)
                                                                                 .Where(x => x.IsEnabled == CommonEnum.IsEnabled.Yes)
                                                                                 .Where(x => x.DayOfWeek == ScheduleOfWeek.特约课 || x.DayOfWeek == (ScheduleOfWeek)dayOfWeek)
                                                                                 .Where(x => now.Date <= x.EndDate && now.Date >= x.StartDate)
                                                                                 .OrderBy(k => k.StartTime)
                                                                                 .ToList();
                                if (coursesScheduleList != null && coursesScheduleList.Count > 0)
                                {
                                    var teacherAttence = coursesScheduleList.FirstOrDefault(x => x.TeacherAttences.Any(p => p.IsAttend == CommonEnum.YesOrNo.No && p.InverseParentGuidNavigation == null))?
                                                                            .TeacherAttences.FirstOrDefault();
                                    if (teacherAttence != null)
                                    {
                                        teacherAttence.IsAttend = CommonEnum.YesOrNo.Yes;
                                        teacherAttence.AttenceTime = now;
                                        teacherAttence.AttenceType = CommonEnum.AttenceType.刷脸;
                                        successNum++;
                                        res += "教师姓名:" + teacher.FullName + ",";
                                        //return Ok(response);
                                        continue;
                                    }
                                    var coursesSchedule = coursesScheduleList.FirstOrDefault(x => !x.TeacherAttences.Any());
                                    if (coursesSchedule != null)
                                    {
                                        if (CreateTeacherAttence(_dbContext.CourseSchedule.AsQueryable().AsNoTracking().Where(x => x.Guid == coursesSchedule.Guid)
                                            .Include(x => x.CourseHourCourseSchedule)
                                            .ThenInclude(x => x.CourseHour)
                                            .Include(x => x.ClassGradeCourseSchedule)
                                            .ThenInclude(x => x.ClassGrade.CourseHours)
                                            .Select(x => new
                                            {
                                                CourseSchedule = x,
                                                CourseHourCourseSchedule = x.CourseHourCourseSchedule,
                                                ClassGradeCourseSchedule = x.ClassGradeCourseSchedule,
                                                ClassGrade = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade),
                                                CourseHours = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade.CourseHours),
                                                CourseHour = x.CourseHourCourseSchedule.Select(s => s.CourseHour),
                                            })
                                            .ToList()
                                            .Select(x => x.CourseSchedule)
                                            .FirstOrDefault(), CommonEnum.AttenceType.刷脸))
                                        {
                                            successNum++;
                                            res += "教师姓名:" + teacher.FullName + ",";
                                        }
                                    }
                                }
                            }
                        }
                        if (traineesList.Count > 0)
                        {
                            foreach (var trainees in traineesList)
                            {
                                var attenceList = trainees.TraineesAttences.Where(x => now.Date <= x.CreatedOn && now.AddDays(1).Date > x.CreatedOn)
                                                                           .OrderBy(k => k.CourseSchedule.StartTime)
                                                                           .ToList();
                                if (attenceList != null && attenceList.Count > 0)
                                {
                                    var attence = attenceList.FirstOrDefault(x => x.IsAttend == CommonEnum.YesOrNo.No);
                                    if (attence != null)
                                    {
                                        attence.AttenceTime = now;
                                        attence.IsAttend = CommonEnum.YesOrNo.Yes;
                                        attence.AttenceType = CommonEnum.AttenceType.刷脸;
                                        if (attence.CourseHour.CourseSubject.ChargeType != ChargeType.按学期)
                                        {
                                            attence.IsDeduct = CommonEnum.YesOrNo.Yes;
                                            attence.CourseHour.Surplus -= 1;
                                        }
                                        //准备发送签到通知
                                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.TemplateId.Attend) && trainees.CustomerGuid != null && !string.IsNullOrEmpty(trainees.RelationCust.WxOpenid))
                                        {
                                            string fullName = trainees.FullName;
                                            string className = attence.CourseHour.ClassGrade.ClassName;
                                            string dateTime = string.Format("{0}({1}-{2})", now.ToString("yyyy年MM月dd日"), attence.CourseSchedule.StartTime.ToString(@"hh\:mm"), attence.CourseSchedule.EndTime.ToString(@"hh\:mm"));
                                            string deduct = (attence.IsDeduct == CommonEnum.YesOrNo.Yes) ? "1" : "0";
                                            string surplus = attence.CourseHour.Surplus.ToString();

                                            _dbContext.TemplateMsg.Add(new TemplateMsg()
                                            {
                                                Guid = Guid.NewGuid(),
                                                TemplateId = ConfigurationManager.AppSettings.TemplateId.Attend,
                                                ToUser = trainees.RelationCust.WxOpenid,
                                                SendNum = 0,
                                                SendStatus = TemplateMsgSendStatus.未发送,
                                                Data = JsonConvert.SerializeObject(new TemplateMsgAttendModel(fullName, className, dateTime, deduct, surplus)),
                                                MsgId = string.Empty,
                                                CreatedOn = now,
                                                Timing = now,
                                            });
                                        }
                                        successNum++;
                                        res += "学员姓名:" + trainees.FullName + ",";
                                    }
                                }
                            }
                        }
                        _dbContext.SaveChanges();
                        res = "识别成功" + teacherOrTraineesList.Count + "人,签到" + successNum + "人。" + res.TrimEnd(',');
                        response.SetSuccess(res);
                    }
                    catch (Exception e)
                    {
                        response.SetError(e.Message);
                    }
                    return Ok(response);
                }
                else
                {
                    response.SetFailed("未检测到人脸信息");
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// 教师签到
        /// </summary>
        /// <param name="guid">课表Guid</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [CustomAuthorize("signindesk_teacher")]
        [OperationLog("签到台控制器", "教师签到", "手动给教师签到")]
        public IActionResult SignInTeacher(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            var now = DateTime.Now;
            //using (_dbContext)
            //{
                var entity = _dbContext.TeacherAttence.FirstOrDefault(x => x.CourseScheduleGuid == guid && x.IsSubstitute == CommonEnum.YesOrNo.No && now.Date <= x.CreatedOn && now.AddDays(1).Date > x.CreatedOn);
                if (entity != null)
                {
                    if (entity.IsAttend == CommonEnum.YesOrNo.Yes)
                    {
                        response.SetFailed("该教师已签到");
                    }
                    else if (entity.InverseParentGuidNavigation == null)
                    {
                        entity.IsAttend = CommonEnum.YesOrNo.Yes;
                        entity.AttenceType = CommonEnum.AttenceType.后台;
                        entity.AttenceTime = now;
                        _dbContext.SaveChanges();
                        response.SetSuccess("签到成功");
                    }
                    else
                    {
                        response.SetFailed("代课教师已签到");
                    }
                }
                else
                {
                    response = CreateTeacherAttence(guid);
                }
            //}
            return Ok(response);
        }

        #region 私有方法
        private ResponseModel CreateTeacherAttence(Guid CourseScheduleGuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            response.SetFailed();
            var now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;
            var query = _dbContext.CourseSchedule.AsQueryable().AsNoTracking();
            query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No);
            query = query.Where(x => x.IsEnabled == CommonEnum.IsEnabled.Yes);
            query = query.Where(x => x.DayOfWeek == ScheduleOfWeek.特约课 || x.DayOfWeek == (ScheduleOfWeek)dayOfWeek);
            query = query.Where(x => now.Date <= x.EndDate && now.Date >= x.StartDate);
            var courseScheduleEntity = query.Where(x => x.Guid == CourseScheduleGuid)
                            .Include(x => x.CourseHourCourseSchedule)
                            .ThenInclude(x => x.CourseHour)
                            .Include(x => x.ClassGradeCourseSchedule)
                            .ThenInclude(x => x.ClassGrade.CourseHours)
                            .Select(x => new
                            {
                                CourseSchedule = x,
                                CourseHourCourseSchedule = x.CourseHourCourseSchedule,
                                ClassGradeCourseSchedule = x.ClassGradeCourseSchedule,
                                ClassGrade = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade),
                                CourseHours = x.ClassGradeCourseSchedule.Select(s => s.ClassGrade.CourseHours),
                                CourseHour = x.CourseHourCourseSchedule.Select(s => s.CourseHour),
                            })
                            .ToList()
                            .Select(x => x.CourseSchedule)
                            .FirstOrDefault();
            if (courseScheduleEntity == null)
            {
                response.SetFailed("参数错误");
            }
            else
            {
                if (CreateTeacherAttence(courseScheduleEntity, CommonEnum.AttenceType.后台))
                {
                    response.SetSuccess("签到成功");
                }
                else
                {
                    response.SetError("系统繁忙,签到失败");
                }
            }
            return response;
        }
        private bool CreateTeacherAttence(CourseSchedule courseScheduleEntity, CommonEnum.AttenceType attenceType)
        {
            var teacherAttenceEntity = new TeacherAttence()
            {
                Guid = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                CreatedByUserGuid = AuthContextService.CurrentUser.Guid,
                CreatedByUserName = AuthContextService.CurrentUser.DisplayName,
                CourseScheduleGuid = courseScheduleEntity.Guid,
                IsAttend = CommonEnum.YesOrNo.Yes,
                IsSubstitute = CommonEnum.YesOrNo.No,
                TeacherGuid = courseScheduleEntity.TeacherGuid,
                AttenceTime = DateTime.Now,
                AttenceType = attenceType,
            };
            _dbContext.TeacherAttence.Add(teacherAttenceEntity);
            var traineesAttenceList = new List<TraineesAttence>();
            foreach (var item1 in courseScheduleEntity.ClassGradeCourseSchedule)
            {
                foreach (var item2 in item1.ClassGrade.CourseHours)
                {
                    traineesAttenceList.Add(new TraineesAttence()
                    {
                        Guid = Guid.NewGuid(),
                        CreatedOn = DateTime.Now,
                        CreatedByUserGuid = AuthContextService.CurrentUser.Guid,
                        CreatedByUserName = AuthContextService.CurrentUser.DisplayName,
                        CourseScheduleGuid = courseScheduleEntity.Guid,
                        IsAttend = CommonEnum.YesOrNo.No,
                        IsDeduct = CommonEnum.YesOrNo.No,
                        CourseHourGuid = item2.Guid,
                        TeacherAttenceGuid = teacherAttenceEntity.Guid,
                        TraineesGuid = item2.TraineesGuid,
                    });
                }
            }
            foreach (var item in courseScheduleEntity.CourseHourCourseSchedule)
            {
                traineesAttenceList.Add(new TraineesAttence()
                {
                    Guid = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    CreatedByUserGuid = AuthContextService.CurrentUser.Guid,
                    CreatedByUserName = AuthContextService.CurrentUser.DisplayName,
                    CourseScheduleGuid = courseScheduleEntity.Guid,
                    IsAttend = CommonEnum.YesOrNo.No,
                    IsDeduct = CommonEnum.YesOrNo.No,
                    CourseHourGuid = item.CourseHourGuid,
                    TeacherAttenceGuid = teacherAttenceEntity.Guid,
                    TraineesGuid = item.CourseHour.TraineesGuid,
                });
            }
            _dbContext.TraineesAttence.AddRange(traineesAttenceList);
            if (_dbContext.SaveChanges() > 0)
            {
                return true;
                //response.SetSuccess("签到成功");
            }
            else
            {
                return false;
                //response.SetError("系统繁忙,签到失败");
            }
        }
        #endregion
    }
}
