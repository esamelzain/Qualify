using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class CoursesResponse : BaseResponse
    {
        public List<SmallCourse> CourseList { get; set; }
    }
}