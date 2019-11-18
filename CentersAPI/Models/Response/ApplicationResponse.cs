using CentersAPI.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class ApplicationResponse : BaseResponse
    {
        public bool Application { get; set; }
    }
}