using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    public class Teacher
    {
        public Teacher()
        {
            CourseSchedules = new HashSet<CourseSchedule>();
            TeacherAttences = new HashSet<TeacherAttence>();
        }
        /// <summary>
        /// 老师GUID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 老师姓名
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FullName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Telephone { get; set; }
        /// <summary>
        /// 家庭住址
        /// </summary>
        [Column(TypeName = "nvarchar(250)")]
        public string Address { get; set; }
        /// <summary>
        /// 身份卡绑定信息
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string IdCardBindInfo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 人脸特征Guid
        /// </summary>
        public Guid? FaceFeatureGuid { get; set; }
        /// <summary>
        /// 人脸特征实体
        /// </summary>
        public virtual FaceFeature FaceFeature { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 给老师分配的课程时间表
        /// </summary>
        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; }
        /// <summary>
        /// 老师的出勤记录
        /// </summary>
        public virtual ICollection<TeacherAttence> TeacherAttences { get; set; }

        /// <summary>
        /// 获取当前老师已分配课程的名称
        /// </summary>
        /// <returns></returns>
        public string GetCourseName()
        {
            string CourseSubjectName = string.Empty;
            foreach (var item in CourseSchedules)
            {
                CourseSubjectName += CourseSubjectName.Contains(item.CourseName) ? "" : (item.CourseName + "|");
            }
            return CourseSubjectName.Trim('|');
        }
    }
}
