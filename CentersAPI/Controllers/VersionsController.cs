using CentersAPI.Helpers;
using CentersAPI.Models.EFModels;
using CentersAPI.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CentersAPI.Controllers
{
    [BasicAuthentication]
    public class VersionsController : ApiController
    {
        private Entities db = new Entities();
        [HttpGet]
        public VersionResponse CurrentVersion()
        {
            try
            {
                var date = db.ApplicationVersoins.Max(v => v.Date);
                var version = db.ApplicationVersoins.SingleOrDefault(v => v.Date == date);
                if (version != null)
                    return new VersionResponse
                    {
                        Version = version,
                        Message = Utilities.GetErrorMessages("200")
                    };
                else
                    return new VersionResponse
                    {
                        Message = Utilities.GetErrorMessages("504")
                    };
            }
            catch (Exception ex)
            {
                return new VersionResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpGet]
        public IsUpdatedResponse IsUpdated(string VersionNumber)
        {
            try
            {
                int x = int.Parse(VersionNumber);
                var userVersion = db.ApplicationVersoins.SingleOrDefault(v => v.VersionNumber == x);
                var date = db.ApplicationVersoins.Max(v => v.Date);
                var Currentversion = db.ApplicationVersoins.SingleOrDefault(v => v.Date == date);
                if (userVersion != null && userVersion.VersionNumber == Currentversion.VersionNumber)
                    return new IsUpdatedResponse
                    {
                        Message = Utilities.GetErrorMessages("200"),
                        IsUpdated = true
                    };
                else
                    return new IsUpdatedResponse
                    {
                        Message = Utilities.GetErrorMessages("200"),
                        IsUpdated = false
                    };
            }
            catch (Exception ex)
            {
                return new IsUpdatedResponse
                {
                    Message = Utilities.GetErrorMessages("500"),
                    IsUpdated = true
                };
            }
        }
    }
}
