using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Requests
{
    public class VerifyCodeRequest
    {
        public string UserName { get; set; }
        public string Code { get; set; }
    }
}