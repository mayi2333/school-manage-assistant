using DncZeus.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.Controllers.Api.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class PictureController : ControllerBase
    {
        [HttpPost]
        public IActionResult UploadBase64(string fileBase64, string fileExt)
        {
            var response = ResponseModelFactory.CreateInstance;

            string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "PNG", "JPG", "JPEG", "BMP", "GIF" };
            if (!pictureFormatArray.Contains(fileExt))
            {
                response.SetFailed("请上传图片格式");
                return Ok(response);
            }
            byte[] imageBytes = Convert.FromBase64String(fileBase64);
            //var fileExtension = Path.GetExtension(fileName);

            string dest = $"upload/files/pic/";
            string name = Guid.NewGuid().ToString(); //随机生成新的文件名

            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            string jpgFilePath = $"{dest}/{name}.{fileExt}";

            try
            {
                response.SetSuccess();
                MemoryStream ms = new MemoryStream(imageBytes);
                System.Drawing.Image imgr = System.Drawing.Image.FromStream(ms);
                imgr.Dispose();
                ms.Dispose();
                System.IO.File.WriteAllBytes(jpgFilePath, imageBytes);
                response.SetData($"/src/pic/{name}.{fileExt}");
            }
            catch (Exception ex)
            {
                response.SetError($"newFileName:{name},Error:{ex.Message}");
            }
            return Ok(response);
        }
    }
}
