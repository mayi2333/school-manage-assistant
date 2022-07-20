using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.Entities.Enums;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.DataAccess;
using DncZeus.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using DncZeus.Api.RequestPayload.教务中心.Trainees;
using DncZeus.Api.ViewModels.教务中心.Trainees;
using DncZeus.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Authorization;

namespace DncZeus.Api.Controllers.Api.V1.教务中心
{
    /// <summary>
    /// 学员管理
    /// </summary>
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TraineesController : ControllerBase
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly IMapper _mapper;

        public TraineesController(DncZeusDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取学员列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize("trainees_view")]
        public IActionResult List(TraineesRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.Trainees.AsQueryable().AsNoTracking();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = payload.Kw.Trim().Contains("岁") ?
                        query.Where(x => x.Age.ToString() == payload.Kw.Trim().TrimEnd('岁'))
                        : query.Where(x => x.FullName.Contains(payload.Kw.Trim())
                                        || x.Telephone.Contains(payload.Kw.Trim())
                                        || x.Address.Contains(payload.Kw.Trim())
                                        || x.Memo.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDeleted == payload.IsDeleted);
                }
                if (payload.IsBindClassGrade > CommonEnum.YesOrNo.All)
                {
                    query = payload.IsBindClassGrade == CommonEnum.YesOrNo.Yes ?
                        query.Where(x => x.CourseHours.Any(a => a.ClassGradeGuid.HasValue && a.ClassGradeGuid != Guid.Empty))
                        : query.Where(x => x.CourseHours.Count == 0 || x.CourseHours.Any(a => !a.ClassGradeGuid.HasValue || a.ClassGradeGuid == Guid.Empty));
                }
                if (payload.IsBindCourseHour > CommonEnum.YesOrNo.All)
                {
                    query = payload.IsBindCourseHour == CommonEnum.YesOrNo.Yes ?
                        query.Where(x => x.CourseHours.Any(p => p.Surplus > 0))
                        : query.Where(x => !x.CourseHours.Any(p => p.Surplus > 0));
                }
                if (payload.ClassGradeGuid != null)
                {
                    query = query.Where(x => x.CourseHours.Any(p => p.ClassGradeGuid == payload.ClassGradeGuid));
                }
                if (payload.CourseCode != null)
                {
                    query = query.Where(x => x.CourseHours.Any(p => p.CourseCode == payload.CourseCode.Trim()));
                }

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize)
                    .Include(x => x.CourseHours)
                    .Include(x => x.CourseHours).ThenInclude(x => x.ClassGrade)
                    .Include(x => x.CourseHours).ThenInclude(x => x.CourseSubject)
                    .Select(x => new
                    {
                        Trainees = x,
                        CourseHours = x.CourseHours.Where(x => x.Surplus > 0 && x.ExpiryDate >= DateTime.Now.Date),
                    })
                    .ToList()
                    .Select(x => x.Trainees)
                    .ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<Trainees, TraineesJsonModel>);
                //foreach (var item in list)
                //{
                //    var jsonModel = data.First(x => x.Guid == item.Guid);
                //    jsonModel.ClassName = item.GetClassName();
                //    jsonModel.CourseName = item.GetCourseName();
                //}
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }
        /// <summary>
        /// 根据关键字查询学生列表
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
                var query = _dbContext.Trainees.Where(x => (x.FullName.Contains(kw) || x.Telephone.Contains(kw) || x.Guid.ToString() == kw)
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
        /// 创建学员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("trainees_create")]
        [OperationLog("学员控制器", "创建学员", "新增学员保存")]
        public IActionResult Create(TraineesCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.FullName.Trim().Length <= 0)
            {
                response.SetFailed("请输入学员姓名");
                return Ok(response);
            }
            if (model.Age < 0)
            {
                response.SetFailed("请输入正确的年龄");
                return Ok(response);
            }
            if (model.Telephone.Trim().Length <= 0 || !Utils.RegexHelper.Check(model.Telephone, Utils.RegexHelper.Type.电话))
            {
                response.SetFailed("请输入正确的联系电话");
                return Ok(response);
            }
            //using (_dbContext)
            //{
                if (_dbContext.Trainees.Count(x => x.FullName == model.FullName && x.Telephone == model.Telephone && x.IsDeleted == CommonEnum.IsDeleted.No) > 0)
                {
                    response.SetFailed("该学员已存在，创建失败");
                    return Ok(response);
                }
                //var entity = new Trainees();
                var entity = _mapper.Map<TraineesCreateOrEditViewModel, Trainees>(model);
                //entity.FullName = model.FullName.Trim();
                //entity.Telephone = model.Telephone.Trim();
                //entity.Address = model.Address.Trim();
                //entity.Age = model.Age;
                //entity.Memo = model.Memo;
                entity.CreatedOn = DateTime.Now;
                entity.Guid = Guid.NewGuid();
                entity.IsDeleted = CommonEnum.IsDeleted.No;
                entity.IdCardBindInfo = string.Empty;
                _dbContext.Trainees.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            //}
        }

        /// <summary>
        /// 编辑学员
        /// </summary>
        /// <param name="guid">用户GUID</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("trainees_view")]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Trainees.FirstOrDefault(x => x.Guid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<Trainees, TraineesCreateOrEditViewModel>(entity));
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的学员信息
        /// </summary>
        /// <param name="model">用户视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("trainees_edit")]
        [OperationLog("学员控制器", "编辑学员", "编辑学员保存")]
        public IActionResult Edit(TraineesCreateOrEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            if (model.FullName.Trim().Length <= 0)
            {
                response.SetFailed("请输入学员姓名");
                return Ok(response);
            }
            if (model.Age < 0)
            {
                response.SetFailed("请输入正确的年龄");
                return Ok(response);
            }
            if (model.Telephone.Trim().Length <= 0 || !Utils.RegexHelper.Check(model.Telephone, Utils.RegexHelper.Type.电话))
            {
                response.SetFailed("请输入正确的联系电话");
                return Ok(response);
            }
            //using (_dbContext)
            //{
                if (_dbContext.Trainees.Count(x => x.Guid != model.Guid && x.FullName == model.FullName && x.Telephone == model.Telephone && x.IsDeleted == CommonEnum.IsDeleted.No) > 0)
                {
                    response.SetFailed("该学员已存在，修改失败");
                    return Ok(response);
                }
                var entity = _dbContext.Trainees.FirstOrDefault(x => x.Guid == model.Guid);
                if (entity == null)
                {
                    response.SetFailed("参数错误，修改失败");
                    return Ok(response);
                }
                entity.FullName = model.FullName;
                entity.Telephone = model.Telephone;
                entity.Address = model.Address;
                entity.Age = model.Age;
                entity.Memo = model.Memo;
                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            //}
        }
        /// <summary>
        /// 学员绑定ID卡
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("trainees_bind_card")]
        public IActionResult TraineesBindingCard(Guid guid, string card)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if (_dbContext.Teacher.Any(x => x.IdCardBindInfo == card.Trim() && x.IsDeleted == CommonEnum.IsDeleted.No)
                    || _dbContext.Trainees.Any(x => x.Guid != guid && x.IdCardBindInfo == card.Trim() && x.IsDeleted == CommonEnum.IsDeleted.No))
                {
                    response.SetFailed("此卡已被绑定，绑定失败");
                    return Ok(response);
                }
                var entity = _dbContext.Trainees.FirstOrDefault(x => x.Guid == guid);
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
        /// 学员ID卡信息解绑
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("trainees_bind_card")]
        public IActionResult TraineesUnBindingCard(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Trainees.FirstOrDefault(x => x.Guid == guid);
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
        /// 学员绑定人脸信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize("trainees_bind_face")]
        public IActionResult TraineesBindingFace(TraineesBindingFaceViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Trainees.FirstOrDefault(x => x.Guid == model.guid);
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
        /// <summary>
        /// 删除学员
        /// </summary>
        /// <param name="ids">用户GUID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("trainees_delete")]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }

        /// <summary>
        /// 恢复学员
        /// </summary>
        /// <param name="ids">用户GUID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        [CustomAuthorize("trainees_recover")]
        public IActionResult Recover(string ids)
        {
            var response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
            return Ok(response);
        }

        #region 私有方法
        /// <summary>
        /// 删除学员
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">用户ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE Trainees SET IsDeleted=@IsDeleted WHERE Guid IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlCommand(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        #endregion
    }
}
