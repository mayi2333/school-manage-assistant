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
using DncZeus.Api.RequestPayload.教务中心.TraineesAttence;
using DncZeus.Api.ViewModels.教务中心.TraineesAttence;
using DncZeus.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Authorization;
using DncZeus.Api.Models;
using Newtonsoft.Json;

namespace DncZeus.Api.Controllers.Api.V1.教务中心
{
    /// <summary>
    /// 学生考勤
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TraineesAttenceController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;

        public TraineesAttenceController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpPost]
        [CustomAuthorize("traineesattence_view")]
        public IActionResult List(TraineesAttenceRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.TraineesAttence.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Trainees.FullName.Contains(payload.Kw.Trim()));
                }
                if (payload.IsAttend > CommonEnum.YesOrNo.All)
                {
                    query = query.Where(x => x.IsAttend == payload.IsAttend);
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
                                .Include(x => x.Trainees).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<TraineesAttence, TraineesAttenceJsonModel>);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }
        [HttpGet("{guid}")]
        public IActionResult SignInTrainees(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            var now = DateTime.Now;
            using (_dbContext)
            {
                if (_dbContext.TeacherAttence.Any(x => x.CourseScheduleGuid == guid && x.IsSubstitute == CommonEnum.YesOrNo.No && now.Date <= x.CreatedOn && now.AddDays(1).Date > x.CreatedOn))
                {
                    var query = _dbContext.TraineesAttence.AsQueryable().AsNoTracking();
                    query = query.Where(x => x.CourseScheduleGuid == guid && now.Date <= x.CreatedOn && now.AddDays(1).Date > x.CreatedOn);
                    var list = query.Include(x => x.Trainees).ToList();
                    var trainees = list.Select(x => new { label = x.Trainees.FullName, key = x.Guid, disabled = x.IsAttend == CommonEnum.YesOrNo.Yes });
                    var attendTrainees = list.Where(x => x.IsAttend == CommonEnum.YesOrNo.Yes).Select(x => x.Guid);
                    response.SetData(new { trainees = trainees, attendTrainees = attendTrainees });
                }
                else
                {
                    response.SetFailed("请先给教师签到");
                }
            }
            return Ok(response);
        }
        [HttpPost]
        [OperationLog("学员考勤控制器", "学员签到", "手动给学员签到")]
        public IActionResult SignInTrainees(SignInTraineesViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            var now = DateTime.Now;
            //using (_dbContext)
            //{

            var list = _dbContext.TraineesAttence.Where(x => x.CourseScheduleGuid == model.CourseScheduleGuid && x.IsAttend == CommonEnum.YesOrNo.No && model.AttendTrainees.Contains(x.Guid))
                            .Include(x => x.CourseHour)
                            .ThenInclude(x => x.CourseSubject)
                            .Include(x => x.Trainees)
                            .ThenInclude(x => x.RelationCust)
                            .Include(x => x.CourseSchedule)
                            .ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.IsAttend = CommonEnum.YesOrNo.Yes;
                    item.AttenceTime = DateTime.Now;
                    item.AttenceType = CommonEnum.AttenceType.后台;
                    if (item.CourseHour.CourseSubject.ChargeType != ChargeType.按学期)
                    {
                        item.IsDeduct = CommonEnum.YesOrNo.Yes;
                        item.CourseHour.Surplus -= 1;
                    }
                    //准备发送签到通知
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.TemplateId.Attend) && item.Trainees.CustomerGuid != null && !string.IsNullOrEmpty(item.Trainees.RelationCust.WxOpenid))
                    {
                        string fullName = item.Trainees.FullName;
                        string className = item.CourseHour.ClassGrade.ClassName;
                        string dateTime = string.Format("{0}({1}-{2})", now.ToString("yyyy年MM月dd日"), item.CourseSchedule.StartTime.ToString(@"hh\:mm"), item.CourseSchedule.EndTime.ToString(@"hh\:mm"));
                        string deduct = (item.IsDeduct == CommonEnum.YesOrNo.Yes) ? "1" : "0";
                        string surplus = item.CourseHour.Surplus.ToString();

                        _dbContext.TemplateMsg.Add(new TemplateMsg()
                        {
                            Guid = Guid.NewGuid(),
                            TemplateId = ConfigurationManager.AppSettings.TemplateId.Attend,
                            ToUser = item.Trainees.RelationCust.WxOpenid,
                            SendNum = 0,
                            SendStatus = TemplateMsgSendStatus.未发送,
                            Data = JsonConvert.SerializeObject(new TemplateMsgAttendModel(fullName, className, dateTime, deduct, surplus)),
                            MsgId = string.Empty,
                            CreatedOn = now,
                            Timing = now,
                        });
                    }
                }
                if (_dbContext.SaveChanges() > 0)
                {
                    response.SetSuccess("签到成功");
                }
                else
                {
                    response.SetFailed("签到失败");
                }
            }
            else
            {
                response.SetError("请选择学员后提交");
            }

            //}
            return Ok(response);
        }

        [HttpGet("{guid}")]
        [OperationLog("学员考勤控制器", "扣除课时", "未到学员扣除课时数")]
        [CustomAuthorize("deduct_coursehour")]
        public IActionResult DeductCourseHour(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            var entity = _dbContext.TraineesAttence.FirstOrDefault(x => x.Guid == guid);
            if (entity != null)
            {
                if (entity.IsDeduct == CommonEnum.YesOrNo.No)
                {
                    if (entity.CourseHour.CourseSubject.ChargeType != ChargeType.按学期)
                    {
                        entity.ModifiedOn = DateTime.Now;
                        entity.ModifiedByUserGuid = AuthContextService.CurrentUser.Guid;
                        entity.ModifiedByUserName = AuthContextService.CurrentUser.DisplayName;
                        entity.IsDeduct = CommonEnum.YesOrNo.Yes;
                        entity.CourseHour.Surplus -= 1;
                        _dbContext.SaveChanges();
                        response.SetSuccess("扣除成功");
                    }
                    else
                    {
                        response.SetFailed("该课程是按学期收费");
                    }
                }
                else
                {
                    response.SetFailed("扣除失败,请勿重复扣除课时数");
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
