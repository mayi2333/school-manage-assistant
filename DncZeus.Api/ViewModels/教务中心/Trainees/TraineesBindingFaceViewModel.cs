using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.ViewModels.教务中心.Trainees
{
    public class TraineesBindingFaceViewModel
    {
        /// <summary>
        /// 教师Guid
        /// </summary>
        public Guid guid { get; set; }
        /// <summary>
        /// base64格式人脸照片
        /// </summary>
        public string img { get; set; }
    }
}
