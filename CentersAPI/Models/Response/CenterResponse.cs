using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class CenterResponse:BaseResponse
    {
        public SmallCenter center { get; set; }
    }
}