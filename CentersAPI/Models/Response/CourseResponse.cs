using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class CourseResponse : BaseResponse
    {
        public string Name { get; set; }
        public int Hours { get; set; }
        public string BeginDate { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public string EndDate { get; set; }
        public string Instructor { get; set; }
        public string Outline { get; set; }
        public string Description { get; set; }
        public string PreRequest { get; set; }
        public string Audience { get; set; }
        public decimal Price { get; set; }
        public string CourseLogo { get; set; }
        public string CourseCategory { get; set; }
        public string CourseType { get; set; }
        public bool IsUserSubscribed { get; set; }
        public bool IsFinished { get; set; }
        public bool IsPaid { get; set; }
        public bool IsLoved { get; set; }
        public int Loves { get; set; }
    }
}