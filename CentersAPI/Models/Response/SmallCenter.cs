using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class SmallCenter
    {
        public int Id { get; set; }
        public string CenterName { get; set; }
        public string Location { get; set; }
        public int CenterRate { get; set; }
        public string Description { get; set; }
        public List<string> Phones { get; set; }
        public List<string> Images { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
    }
}