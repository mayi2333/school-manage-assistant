using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DncZeus.Api.Entities.Enums.CommonEnum;

namespace DncZeus.Api.Entities
{
    public class TraineesAttence
    {
        /// <summary>
        /// 学员考勤Guid
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
        /// 是否已扣除课时
        /// </summary>
        public YesOrNo IsDeduct { get; set; }
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
        /// 学员课时Guid
        /// </summary>
        [Required]
        public Guid CourseHourGuid { get; set; }
        /// <summary>
        /// 学员课时实体
        /// </summary>
        public virtual CourseHour CourseHour { get; set; }
        /// <summary>
        /// 学员Guid
        /// </summary>
        [Required]
        public Guid TraineesGuid { get; set; }
        /// <summary>
        /// 学员实体
        /// </summary>
        public virtual Trainees Trainees { get; set; }
        /// <summary>
        /// 课程表Guid
        /// </summary>
        public Guid CourseScheduleGuid { get; set; }
        /// <summary>
        /// 课表实体
        /// </summary>
        public virtual CourseSchedule CourseSchedule { get; set; }
        /// <summary>
        /// 教师考勤记录Guid
        /// </summary>
        [Required]
        public Guid TeacherAttenceGuid { get; set; }
        /// <summary>
        /// 教师考勤实体
        /// </summary>
        public virtual TeacherAttence TeacherAttence { get; set; }
    }
}
