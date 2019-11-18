using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class VerifyCodeResponse : BaseResponse
    {
        public bool Verified { get; set; }
        public LoginResponse User { get; set; }
    }
}