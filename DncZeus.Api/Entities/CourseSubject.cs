using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    /// <summary>
    /// 课程科目表实体类
    /// </summary>
    public class CourseSubject
    {
        public CourseSubject()
        {
            ClassGrades = new HashSet<ClassGrade>();
            CourseHours = new HashSet<CourseHour>();
            AuditionCourses = new HashSet<AuditionCourse>();
        }
        /// <summary>
        /// 科目编码
        /// </summary>
        [Required]
        [Key]
        [Column(TypeName = "nvarchar(50)")]
        public string Code { get; set; }
        /// <summary>
        /// 科目名称
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string CourseName { get; set; }
        /// <summary>
        /// 收费方式
        /// </summary>
        public ChargeType ChargeType { get; set; }
        /// <summary>
        /// 课程价格(单位为分)，按课时为课时价格，按学期为学期价
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
        /// <summary>
        /// 是否已删
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 课程关联的班级
        /// </summary>
        public virtual ICollection<ClassGrade> ClassGrades { get; set; }
        /// <summary>
        /// 课程关联的课时
        /// </summary>
        public virtual ICollection<CourseHour> CourseHours { get; set; }
        /// <summary>
        /// 课程关联的课表
        /// </summary>
        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; }
        /// <summary>
        /// 学员预约试听课记录
        /// </summary>
        public virtual ICollection<AuditionCourse> AuditionCourses { get; set; }
    }

    /// <summary>
    /// 收费类型
    /// </summary>
    public enum ChargeType
    {
        未指定=-1,
        按课时=0,
        按学期=1
    }
}
