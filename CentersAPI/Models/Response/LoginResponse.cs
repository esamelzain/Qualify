using CentersAPI.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class LoginResponse : BaseResponse
    {
        public int Id { get; set; }
        public bool LoggedIn { get; set; }
        public bool Confirmed { get; set; }
        public LoginRequest User { get; set; }
    }
}