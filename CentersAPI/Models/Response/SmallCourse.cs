using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class SmallCourse
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string CenterName { get; set; }
        public int Hours { get; set; }
        public int CenterId { get; set; }
        public int Loves { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string Instructor { get; set; }
        public decimal Price { get; set; }
        public string CourseLogo { get; set; }
        public double Rate { get; set; }
        public string Image { get; set; }
    }
}