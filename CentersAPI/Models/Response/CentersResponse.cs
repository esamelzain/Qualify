using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class CentersResponse:BaseResponse
    {
        public List<SmallCenter> centers { get; set; }
    }
}