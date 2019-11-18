using CentersAPI.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class UserProfile : BaseResponse
    {
        public Profile endUser { get; set; }
    }
    public class Profile
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string CareerLevel { get; set; }
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; }
        public string VerifyCode { get; set; }
    }
}