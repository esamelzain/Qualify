using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class UserCourses:BaseResponse
    {
        public List<SmallCourse> PendingCourses { get; set; }
        public List<SmallCourse> PaidCourses { get; set; }
    }
}