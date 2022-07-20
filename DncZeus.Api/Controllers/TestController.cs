using DncZeus.Api.Entities;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.CustomException;
using DncZeus.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DncZeus.Api.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    //[ApiController]
    [Route("[controller]/[action]")]
    public class TestController : Controller
    {
        private readonly DncZeusDbContext _dbContext;
        private readonly ILogger _logger;

        public TestController(DncZeusDbContext dbContext, ILogger<TestController> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 测试日志
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        [HttpGet]
        [OperationLog("测试控制器","测试日志","这是描述")]
        public IActionResult Logger()
        {
            _logger.LogDebug(message: "LogDebug()...");
            _logger.LogInformation(message: "LogInformation()...");
            _logger.LogWarning(message: "LogWarning()...");
            _logger.LogError(message: "LogError()...");
            ResponseResultModel response = ResponseModelFactory.CreateResultInstance;
            response.SetSuccess(message: "test logger success");
            return Ok(value: response);
        }

        [HttpGet]
        public IActionResult FaceRecognition(string img)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(img))
            {
                response.SetFailed("参数错误");
                return Ok(response);
            }
            string base64 = img.Substring(img.IndexOf(',') + 1);
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
                    try
                    {
                        var faceGuidList = FaceServer.FaceRecognitionForImage(base64).Select(x => x.Key).ToList();
                        var teacherOrTraineesList = _dbContext.FaceFeature.Where(x => faceGuidList.Contains(x.Guid))
                                                                    .Include(x => x.Trainees)
                                                                    .Include(x => x.Teacher)
                                                                    .Select(x => new {
                                                                        Trainees = x.Trainees,
                                                                        Teacher = x.Teacher
                                                                    }).ToList();
                        var teacherList = teacherOrTraineesList.Where(x => x.Teacher != null).Select(x => x.Teacher).ToList();
                        var traineesList = teacherOrTraineesList.Where(x => x.Trainees != null).Select(x => x.Trainees).ToList();
                        string res = string.Empty;
                        foreach (var item in teacherList)
                        {
                            res += "教师:" + item.FullName + ",";
                        }
                        foreach (var item in traineesList)
                        {
                            res += "学员:" + item.FullName + ",";
                        }
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

        [HttpGet]
        public IActionResult FaceEnter(string img)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                string base64 = img.Substring(img.IndexOf(',') + 1);
                if (FaceServer.FaceCheck(base64) > 0)
                {
                    var addFacefeature = new FaceFeature()
                    {
                        Guid = Guid.NewGuid()
                    };
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
                return Ok(response);
            }
        }
    }
}
