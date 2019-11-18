using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CentersAPI.Models.EFModels;


namespace CentersAPI.Models.Response
{
    public class Categories : BaseResponse
    {
        public List<UserCategories> categories { get; set; }
    }
    public class UserCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Image { get; set; }
        public int Courses { get; set; }

    }
}