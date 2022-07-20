using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    public class TeacherAttence
    {
        public TeacherAttence()
        {
            TraineesAttences = new HashSet<TraineesAttence>();
            AuditionCourses = new HashSet<AuditionCourse>();
        }
        /// <summary>
        /// 教师考勤Guid
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 是否出勤
        /// </summary>
        public YesOrNo IsAttend { get; set; }
        /// <summary>
        /// 是否代课
        /// </summary>
        public YesOrNo IsSubstitute { get; set; }
        /// <summary>
        /// 创建者ID
        /// </summary>
        public Guid CreatedByUserGuid { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime? AttenceTime { get; set; }
        /// <summary>
        /// 创建者姓名
        /// </summary>
        public string CreatedByUserName { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
        /// <summary>
        /// 最近修改者ID
        /// </summary>
        public Guid? ModifiedByUserGuid { get; set; }
        /// <summary>
        /// 最近修改者姓名
        /// </summary>
        public string ModifiedByUserName { get; set; }
        /// <summary>
        /// 签到方式
        /// </summary>
        [DefaultValue(AttenceType.空缺)]
        public AttenceType AttenceType { get; set; }
        /// <summary>
        /// 教师Guid
        /// </summary>
        [Required]
        public Guid TeacherGuid { get; set; }
        /// <summary>
        /// 教师实体
        /// </summary>
        public virtual Teacher Teacher { get; set; }
        /// <summary>
        /// 课程表Guid
        /// </summary>
        [Required]
        public Guid CourseScheduleGuid { get; set; }
        /// <summary>
        /// 课表实体
        /// </summary>
        public virtual CourseSchedule CourseSchedule { get; set; }
        /// <summary>
        /// 当此记录为代课记录时 填写缺勤记录Guid
        /// </summary>
        public Guid? ParentGuid { get; set; }
        /// <summary>
        /// 当此记录是代课时，缺勤记录的实体
        /// </summary>
        public virtual TeacherAttence ParentGuidNavigation { get;set;}
        /// <summary>
        /// 当此记录缺勤时，代课记录的实体
        /// </summary>
        public virtual TeacherAttence InverseParentGuidNavigation { get; set; }
        /// <summary>
        /// 学员考勤记录
        /// </summary>
        public virtual ICollection<TraineesAttence> TraineesAttences { get; set; }
        /// <summary>
        /// 学员预约试听课记录
        /// </summary>
        public virtual ICollection<AuditionCourse> AuditionCourses { get; set; }
    }
}
