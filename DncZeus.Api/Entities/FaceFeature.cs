using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DncZeus.Api.Entities
{
    public class FaceFeature
    {
        /// <summary>
        /// 人脸信息Guid
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 人脸特征数组转换的字符串
        /// </summary>
        [Column(TypeName = "varchar(3000)")]
        public string FaceEncodes { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 录入该人脸特征的教师
        /// </summary>
        public virtual Teacher Teacher { get; set; }
        /// <summary>
        /// 录入该人脸特征的学员
        /// </summary>
        public virtual Trainees Trainees { get; set; }
    }
}
