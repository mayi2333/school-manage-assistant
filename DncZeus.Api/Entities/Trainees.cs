using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    /// <summary>
    /// 学员实体类
    /// </summary>
    public class Trainees
    {
        public Trainees()
        {
            CourseHours = new HashSet<CourseHour>();
            TraineesAttences = new HashSet<TraineesAttence>();
            AuditionCourses = new HashSet<AuditionCourse>();
        }
        /// <summary>
        /// 学员GUID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 学员姓名
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FullName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [Required]
        public int Age { get; set; }
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
        /// 关联客户Guid
        /// </summary>
        public Guid? CustomerGuid { get; set; }
        /// <summary>
        /// 关联客户
        /// </summary>
        public virtual Customer RelationCust { get; set; }
        /// <summary>
        /// 人脸特征Guid
        /// </summary>
        public Guid? FaceFeatureGuid { get; set; }
        /// <summary>
        /// 人脸特征实体
        /// </summary>
        public virtual FaceFeature FaceFeature { get; set; }
        /// <summary>
        /// 是否已删
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 关联的课时
        /// </summary>
        public virtual ICollection<CourseHour> CourseHours { get; set; }
        /// <summary>
        /// 学员考勤记录
        /// </summary>
        public virtual ICollection<TraineesAttence> TraineesAttences { get; set; }
        /// <summary>
        /// 学员预约试听课记录
        /// </summary>
        public virtual ICollection<AuditionCourse> AuditionCourses { get; set; }

        /// <summary>
        /// 获取当前学员已分配班级的名称
        /// </summary>
        /// <returns></returns>
        public string GetClassName()
        {
            string GradeName = string.Empty;
            foreach (var item in CourseHours)
            {
                GradeName += item.ClassGrade?.ClassName + "|";
            }
            return GradeName.Trim('|');
        }
        /// <summary>
        /// 获取当前学员已购买课程的名称
        /// </summary>
        /// <returns></returns>
        public string GetCourseName()
        {
            string CourseSubjectName = string.Empty;
            foreach (var item in CourseHours)
            {
                CourseSubjectName += item.CourseSubject?.CourseName + "|";
            }
            return CourseSubjectName.Trim('|');
        }
    }
}
