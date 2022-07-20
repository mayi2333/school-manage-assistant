using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    /// <summary>
    /// 课时表实体类
    /// </summary>
    public class CourseHour
    {
        public CourseHour()
        {
            TraineesAttences = new HashSet<TraineesAttence>();
            CourseHourCourseSchedule = new HashSet<CourseHourCourseScheduleMapping>();
        }
        /// <summary>
        /// 课时GUID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 剩余课时数
        /// </summary>
        public int Surplus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
        /// <summary>
        /// 截至日期
        /// </summary>
        public DateTime ExpiryDate { get; set; }
        /// <summary>
        /// 修改课时所产生的操作日志，只增不减，打卡消课不做记录
        /// </summary>
        [Column(TypeName = "nvarchar(max)")]
        public string OperationLog { get; set; }
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
        /// 关联班级Guid
        /// </summary>
        public Guid? ClassGradeGuid { get; set; }
        /// <summary>
        /// 关联的班级
        /// </summary>
        public virtual ClassGrade ClassGrade { get; set; }
        /// <summary>
        /// 学员上课出勤记录
        /// </summary>
        public virtual ICollection<TraineesAttence> TraineesAttences { get; set; }
        /// <summary>
        /// 学员课表映射
        /// </summary>
        public virtual ICollection<CourseHourCourseScheduleMapping> CourseHourCourseSchedule { get; set; }
    }
}
