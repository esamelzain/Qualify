using CentersAPI.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class RegisterResponse : BaseResponse
    {
        public bool Registered { get; set; }
        public int Id { get; set; }
        public RegisterRequest User { get; set; }
    }
}