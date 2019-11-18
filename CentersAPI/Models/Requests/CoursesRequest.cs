using CentersAPI.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Requests
{
    public class CoursesRequest
    {
        public LoginRequest User { get; set; }
    }
}