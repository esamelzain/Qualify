using CentersAPI.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class NotificationResponse : BaseResponse
    {
        public List<Notification> Notifications { get; set; }
    }
}