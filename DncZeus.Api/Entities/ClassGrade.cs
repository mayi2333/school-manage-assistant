using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    /// <summary>
    /// 班级表实体类
    /// </summary>
    public class ClassGrade
    {
        public ClassGrade()
        {
            CourseHours = new HashSet<CourseHour>();
            ClassGradeCourseSchedule = new HashSet<ClassGradeCourseScheduleMapping>();
            AuditionCourses = new HashSet<AuditionCourse>();
        }
        /// <summary>
        /// 班级GUID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string ClassName { get; set; }
        /// <summary>
        /// 班级总人数
        /// </summary>
        public int TotalPeople { get; set; }
        /// <summary>
        /// 是特约班，特约版不能直接分配课表，只能对班级里面学生单独分配课表
        /// </summary>
        public YesOrNo IsSpecial { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 是否已删
        /// </summary>
        public IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 科目编码
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string CourseCode { get; set; }
        /// <summary>
        /// 关联的课程
        /// </summary>
        public virtual CourseSubject CourseSubject { get; set; }
        /// <summary>
        /// 关联的课时
        /// </summary>
        public virtual ICollection<CourseHour> CourseHours { get; set; }
        /// <summary>
        /// 班级课表映射
        /// </summary>
        public virtual ICollection<ClassGradeCourseScheduleMapping> ClassGradeCourseSchedule { get; set; }
        /// <summary>
        /// 学员预约试听课
        /// </summary>
        public virtual ICollection<AuditionCourse> AuditionCourses { get; set; }
    }
}
