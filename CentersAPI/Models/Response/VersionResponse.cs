using CentersAPI.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class VersionResponse : BaseResponse
    {
        public ErrorModel Message { get; set; }
        public ApplicationVersoin Version { get; set; }
    }
}