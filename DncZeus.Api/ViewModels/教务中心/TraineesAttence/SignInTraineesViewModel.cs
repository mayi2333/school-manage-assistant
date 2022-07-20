using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.ViewModels.教务中心.TraineesAttence
{
    public class SignInTraineesViewModel
    {
        public SignInTraineesViewModel()
        {
            AttendTrainees = new List<Guid>();
        }
        public Guid CourseScheduleGuid { get; set; }
        public List<Guid> AttendTrainees { get; set; }
    }
}
