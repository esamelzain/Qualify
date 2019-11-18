using CentersAPI.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Requests
{
    public class RegisterRequest 
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}