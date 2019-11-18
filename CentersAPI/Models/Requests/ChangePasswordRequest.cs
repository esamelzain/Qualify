using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Requests
{
    public class ChangePasswordRequest
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public string UserName { get; set; }
    }
}