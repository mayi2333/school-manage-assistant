using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    public class AuditionCourse
    {
        /// <summary>
        /// 试课Guid
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 当前试课状态
        /// </summary>
        public AuditionCourseState State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
        /// <summary>
        /// 关联学员Guid
        /// </summary>
        [Required]
        public Guid TraineesGuid { get; set; }
        /// <summary>
        /// 关联学员
        /// </summary>
        public virtual Trainees Trainees { get; set; }
        /// <summary>
        /// 科目编码 每个科目只可以试听一次
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string CourseCode { get; set; }
        /// <summary>
        /// 预约的课程
        /// </summary>
        public virtual CourseSubject CourseSubject { get; set; }
        /// <summary>
        /// 分配的班级Guid
        /// </summary>
        public Guid? ClassGradeGuid { get; set; }
        /// <summary>
        /// 分配的班级
        /// </summary>
        public virtual ClassGrade ClassGrade { get; set; }
        /// <summary>
        /// 关联的教师考勤记录Guid 试听完成才关联
        /// </summary>
        public Guid? TeacherAttenceGuid { get; set; }
        /// <summary>
        /// 关联的教师考勤实体
        /// </summary>
        public virtual TeacherAttence TeacherAttence { get; set; }
    }
}
