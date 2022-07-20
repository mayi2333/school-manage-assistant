using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.ViewModels.教务中心.CourseSchedule
{
    public class SaveAssignCourseScheduleViewModel
    {
        public SaveAssignCourseScheduleViewModel()
        {
            AssignedClassGrades = new List<Guid>();
            AssignedCourseHours = new List<Guid>();
        }
        /// <summary>
        /// 用户GUID
        /// </summary>
        public Guid CourseScheduleGuid { get; set; }
        /// <summary>
        /// 已分配的班级Guid集合
        /// </summary>
        public List<Guid> AssignedClassGrades { get; set; }
        /// <summary>
        /// 已分配的学员课时Guid集合
        /// </summary>
        public List<Guid> AssignedCourseHours { get; set; }
    }
}
