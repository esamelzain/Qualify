using CentersAPI.Helpers;
using CentersAPI.Models.EFModels;
using CentersAPI.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CentersAPI.Controllers
{
    public class ApplicationsController : ApiController
    {
        private Entities db = new Entities();
        [HttpPost]
        public ApplicationResponse AddApplication(string userId, string courseId)
        {
            try
            {
                int uId = int.Parse(userId);
                int cId = int.Parse(courseId);
                var endUser = db.EndUsers.SingleOrDefault(user => user.Id == uId);
                var course = db.Courses.SingleOrDefault(c => c.Id == cId && c.EndDate > DateTime.Now);
                if (endUser != null)
                {
                    if (course != null)
                    {
                        if (db.Applications.Any(app => app.Userid == uId && app.CourseId == cId))
                        {
                            return new ApplicationResponse
                            {
                                Application = true,
                                Message = Utilities.GetErrorMessages("441")
                            };
                        }
                        Application application = new Application
                        {
                            CourseId = cId,
                            Userid = uId,
                            IsIndividual = true,
                            ApplicantCount = 1,
                            Cours = course,
                            EndUser = endUser,
                            isPaid = false
                        };
                        db.Applications.Add(application);
                        db.SaveChanges();
                        return new ApplicationResponse
                        {
                            Application = true,
                            Message = Utilities.GetErrorMessages("200")
                        };
                    }
                    else
                    {
                        return new ApplicationResponse
                        {
                            Application = false,
                            Message = Utilities.GetErrorMessages("402")
                        };
                    }
                }
                else
                {
                    return new ApplicationResponse
                    {
                        Application = false,
                        Message = Utilities.GetErrorMessages("405")
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApplicationResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public ApplicationResponse CancelApplication(string userId, string courseId)
        {
            try
            {
                int uId = int.Parse(userId);
                int cId = int.Parse(courseId);
                var application = db.Applications.SingleOrDefault(app => app.Userid == uId
                                                                    && app.CourseId == cId);
                if (application != null)
                {
                    db.Applications.Remove(application);
                    db.SaveChanges();
                    return new ApplicationResponse
                    {
                        Application = true,
                        Message = Utilities.GetErrorMessages("200")
                    };
                }
                else
                {
                    return new ApplicationResponse
                    {
                        Application = false,
                        Message = Utilities.GetErrorMessages("504")
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApplicationResponse
                {
                    Application = false,
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        ////Just For Test Essam & Ahmed
       /* public string PushNoti(int userId)
        {
            List<string> content = new List<string>();
            var user = db.EndUsers.Find(userId);
            var noti = db.Notifications.Where(c => c.id > user.NotificationId).ToList();

            foreach (var item in noti)
            {
                content.Add(item.Content);
            }

            if (noti.Count == 0)
                user.NotificationId = 0;
            else
                user.NotificationId = noti.Max(c => c.id);

            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return JsonConvert.SerializeObject(content);
        }*/

    }
}
