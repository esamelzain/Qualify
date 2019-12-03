//using BPG.Helpers;
//using BPG.Models.Response;
//using DataManagement;
using CentersAPI.Helpers;
using CentersAPI.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CentersAPI.Helpers
{
    /// <summary>
    /// Custom Attribute
    /// Inherits AuthorizationFilterAttribute
    /// </summary>
    public class BasicAuthentication : AuthorizationFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        // Override
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {

                string controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
                if (controllerName != "PublicKey")
                {
                    if (actionContext.Request.Headers.Authorization == null)
                    {
                        actionContext.Response = actionContext.Request
                             .CreateResponse(new BaseResponse
                             {
                                 Message =  Utilities.GetErrorMessages("503")
                             });
                    }
                    else
                    {
                        var authToken = actionContext.Request.Headers
                          .Authorization.Parameter;
                        var decodeauthToken = System.Text.Encoding.UTF8.GetString(
                            Convert.FromBase64String(authToken));
                        var arrUserNameandPassword = decodeauthToken.Split(':');
                        if (new Utilities().IsAuthorizedUser(arrUserNameandPassword[0], arrUserNameandPassword[1]))
                        {
                            Thread.CurrentPrincipal = new GenericPrincipal(
                            new GenericIdentity(arrUserNameandPassword[0]), null);
                        }
                        else
                        {
                            actionContext.Response = actionContext.Request
                            .CreateResponse(new BaseResponse
                            {
                                Message = Utilities.GetErrorMessages("503")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               actionContext.Response = actionContext.Request
                        .CreateResponse(new BaseResponse
                        {
                            Message = Utilities.GetErrorMessages("503")
                        });
            }
        }
    }
}