using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class ErrorModel
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageAr { get; set; }
    }
}