using CentersAPI.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Requests
{
    public class ProfileRequest
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string CareerLevel { get; set; }
        public string Password { get; set; }
        public LoginRequest loginRequest { get; set; }
    }
}