using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using DncZeus.Api.RequestPayload.教务中心.Teacher;
using DncZeus.Api.ViewModels.教务中心.Teacher;
using DncZeus.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Authorization;

namespace DncZeus.Api.Controllers.Api.V1.教务中心
{
    /// <summary>
    /// 教师管理
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public TeacherController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 教师列表加载
        /// </summary>
        /// <param name="payload">教师列表数据加载请求载体</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize("teacher_view")]
        public IActionResult List(TeacherRequestPayload payload)
        {
            using (_dbContext)
            {
                var now = DateTime.Now;
                var query = _dbContext.Teacher.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.FullName.Contains(payload.Kw.Trim())
                                        || x.Telephone.Contains(payload.Kw.Trim())
                                        || x.Address.Contains(payload.Kw.Trim())
                                        || x.Memo.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDeleted == payload.IsDeleted);
                }
                if (payload.IsBindCourseSchedule > CommonEnum.YesOrNo.All)
                {
                    query = payload.IsBindCourseSchedule == CommonEnum.YesOrNo.Yes ?
                        query.Where(x => x.CourseSchedules.Any(a => a.EndDate >= now
                        && a.IsEnabled == CommonEnum.IsEnabled.Yes
                        && a.IsDeleted == CommonEnum.IsDeleted.No))
                        : query.Where(x => !(x.CourseSchedules.Any(a => a.EndDate >= now
                        && a.IsEnabled == CommonEnum.IsEnabled.Yes
                        && a.IsDeleted == CommonEnum.IsDeleted.No)));
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize)
                    .Include(x => x.CourseSchedules)
                    .Include(x => x.TeacherAttences)
                    .Select(x => new
                    {
                        Teacher = x,
                        CourseSchedules = x.CourseSchedules.Where(x => x.EndDate >= now && x.IsEnabled == CommonEnum.IsEnabled.Yes),
                        TeacherAttences = x.TeacherAttences.Where(x => (x.IsAttend == CommonEnum.YesOrNo.Yes || x.IsSubstitute == CommonEnum.YesOrNo.Yes)
                        && x.CreatedOn > DateTime.Now.Date.AddDays(1 - DateTime.Now.Day))
                    })
                    .ToList()
                    .Select(x => x.Teacher)
                    .ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<Teacher, TeacherJsonModel>);

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }
        /// <summary>
        /// 根据关键字查询教师列表
        /// </summary>
        /// <param name="kw">关键字</param>
        /// <param name="cp">当前页码</param>
        /// <param name="ps">分页大小</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult FindByKeyword(string kw, int cp = 1, int ps = 20)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            if (string.IsNullOrEmpty(kw))
            {
                response.SetFailed("没有查询到数据");
                return Ok(response);
            }
            if (cp < 1)
            {
                cp = 1;
            }
            using (_dbContext)
            {
                var query = _dbContext.Teacher.Where(x => (x.FullName.Contains(kw) || x.Telephone.Contains(kw) || x.Guid.ToString() == kw)
                    && x.IsDeleted == CommonEnum.IsDeleted.No)
                    .OrderBy("CreatedOn", true)
                    .Paged(cp, ps);
                var list = query.ToList();
                var data = list.Select(x => new { x.Guid, x.FullName, x.Telephone });
                response.SetData(data);
                return Ok(response);
            }
        }

        /// <summary>
        /// 创建教师
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("teacher_create")]
        public IActionResult Create(TeacherCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.FullName.Trim().Length <= 0)
            {
                response.SetFailed("请输入老师姓名");
                return Ok(response);
            }
            if (model.Telephone.Trim().Length <= 0 || !Utils.RegexHelper.Check(model.Telephone, Utils.RegexHelper.Type.电话))
            {
                response.SetFailed("请输入正确的联系电话");
                return Ok(response);
            }
            using (_dbContext)
            {
                if (_dbContext.Teacher.Count(x => x.FullName == model.FullName && x.Telephone == model.Telephone && x.IsDeleted == CommonEnum.IsDeleted.No) > 0)
                {
                    response.SetFailed("该老师已存在，创建失败");
                    return Ok(response);
                }
                var entity = _mapper.Map<TeacherCreateOrEditViewModel, Teacher>(model);
                entity.CreatedOn = DateTime.Now;
                entity.Guid = Guid.NewGuid();
                entity.IsDeleted = CommonEnum.IsDeleted.No;
                entity.IdCardBindInfo = string.Empty;
                _dbContext.Teacher.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑教师
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Teacher.FirstOrDefault(x => x.Guid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<Teacher, TeacherCreateOrEditViewModel>(entity));
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的教师信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("teacher_edit")]
        public IActionResult Edit(TeacherCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            if (model.FullName.Trim().Length <= 0)
            {
                response.SetFailed("请输入老师姓名");
                return Ok(response);
            }
            if (model.Telephone.Trim().Length <= 0 || !Utils.RegexHelper.Check(model.Telephone, Utils.RegexHelper.Type.电话))
            {
                response.SetFailed("请输入正确的联系电话");
                return Ok(response);
            }
            using (_dbContext)
            {
                if (_dbContext.Teacher.Count(x => x.Guid != model.Guid && x.FullName == model.FullName && x.Telephone == model.Telephone && x.IsDeleted == CommonEnum.IsDeleted.No) > 0)
                {
                    response.SetFailed("该老师已存在，修改失败");
                    return Ok(response);
                }
                var entity = _dbContext.Teacher.FirstOrDefault(x => x.Guid == model.Guid);
                if (entity == null)
                {
                    response.SetFailed("参数错误，修改失败");
                    return Ok(response);
                }
                entity.FullName = model.FullName;
                entity.Telephone = model.Telephone;
                entity.Address = model.Address;
                entity.Memo = model.Memo;
                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }

        /// <summary>
        /// 教师绑定ID卡
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("teacher_bind_card")]
        public IActionResult TeacherBindingCard(Guid guid, string card)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if (_dbContext.Teacher.Any(x => x.Guid != guid && x.IdCardBindInfo == card.Trim() && x.IsDeleted == CommonEnum.IsDeleted.No) 
                    || _dbContext.Trainees.Any(x => x.IdCardBindInfo == card.Trim() && x.IsDeleted == CommonEnum.IsDeleted.No))
                {
                    response.SetFailed("此卡已被绑定，绑定失败");
                    return Ok(response);
                }
                var entity = _dbContext.Teacher.FirstOrDefault(x => x.Guid == guid);
                if (entity == null)
                {
                    response.SetFailed("参数错误，绑定失败");
                    return Ok(response);
                }
                if (entity.IdCardBindInfo == string.Empty)
                {
                    entity.IdCardBindInfo = card.Trim();
                    _dbContext.SaveChanges();
                    response.SetSuccess("绑定成功");
                }
                else
                {
                    response.SetFailed("请勿重复绑定，绑定失败");
                }
                return Ok(response);
            }
        }
        /// <summary>
        /// 教师ID卡信息解绑
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("teacher_bind_card")]
        public IActionResult TeacherUnBindingCard(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Teacher.FirstOrDefault(x => x.Guid == guid);
                if (entity == null)
                {
                    response.SetFailed("参数错误，解绑失败");
                    return Ok(response);
                }
                if (entity.IdCardBindInfo != string.Empty)
                {
                    entity.IdCardBindInfo = string.Empty;
                    _dbContext.SaveChanges();
                    response.SetSuccess("解绑成功");
                }
                else
                {
                    response.SetFailed("还未绑定过ID卡，解绑失败");
                }
                return Ok(response);
            }
        }
        /// <summary>
        /// 教师绑定人脸信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("teacher_bind_face")]
        public IActionResult TeacherBindingFace(TeacherBindingFaceViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Teacher.FirstOrDefault(x => x.Guid == model.guid);
                if (entity != null)
                {
                    if (string.IsNullOrEmpty(model.img))
                    {
                        var faceFeatureGuid = entity.FaceFeatureGuid;
                        if (faceFeatureGuid != null && faceFeatureGuid != Guid.Empty)
                        {
                            entity.FaceFeatureGuid = null;
                            var delFacefeature = new FaceFeature()
                            {
                                Guid = faceFeatureGuid.Value,
                            };
                            _dbContext.FaceFeature.Attach(delFacefeature);
                            _dbContext.FaceFeature.Remove(delFacefeature);
                            _dbContext.SaveChanges();
                        }
                        response.SetSuccess("已清空人脸信息");
                    }
                    else
                    {
                        string base64 = model.img.Substring(model.img.IndexOf(',') + 1);
                        if (FaceServer.FaceCheck(base64) == 1)
                        {
                            var faceFeatureGuid = entity.FaceFeatureGuid;
                            var addFacefeature = new FaceFeature()
                            {
                                Guid = Guid.NewGuid()
                            };
                            entity.FaceFeatureGuid = addFacefeature.Guid;
                            if (faceFeatureGuid != null && faceFeatureGuid != Guid.Empty)
                            {
                                var delFacefeature = new FaceFeature()
                                {
                                    Guid = faceFeatureGuid.Value,
                                };
                                _dbContext.FaceFeature.Attach(delFacefeature);
                                _dbContext.FaceFeature.Remove(delFacefeature);
                            }
                            addFacefeature.FaceEncodes = FaceServer.FaceEntry(base64, addFacefeature.Guid);
                            if (!string.IsNullOrEmpty(addFacefeature.FaceEncodes) && addFacefeature.FaceEncodes != "Error" && addFacefeature.FaceEncodes != "NO Face")
                            {
                                addFacefeature.CreatedOn = DateTime.Now;
                                _dbContext.FaceFeature.Add(addFacefeature);
                                _dbContext.SaveChanges();
                                response.SetSuccess("人脸信息更新成功");
                            }
                            else
                            {
                                response.SetFailed("人脸信息更新失败");
                            }
                        }
                        else
                        {
                            response.SetFailed("未识别到人脸，或者识别到多张人脸");
                        }
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
}
