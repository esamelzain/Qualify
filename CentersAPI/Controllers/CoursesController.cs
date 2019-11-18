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
    public class CoursesController : ApiController
    {
        private Entities db = new Entities();
        [HttpPost]
        public CourseResponse GetCourseById(string courseId, int userId)
        {
            try
            {
                if (courseId != string.Empty)
                {
                    var id = int.Parse(courseId);
                    var UserId = userId;
                    var course = db.Courses.SingleOrDefault(c => c.Id == id && c.isStarted == true);
                    if (UserId != 0)
                    {
                        var application = db.Applications.SingleOrDefault(app => app.CourseId == id && app.Userid == UserId);
                        var loves = db.CoursLoves.Where(co => co.CourseId == id).ToList();
                        bool isLoved = loves.Any(lo => lo.EndUserId == UserId);
                        if (course != null)
                        {
                            return new CourseResponse
                            {
                                Name = course.Name,
                                Hours = course.Hours,
                                BeginDate = course.BeginDate.ToShortDateString(),
                                BeginTime = course.BeginTime.ToString(),
                                EndTime = course.EndTime.ToString(),
                                EndDate = course.EndTime.ToShortDateString(),
                                Instructor = course.Instructor,
                                Description = course.Description,
                                PreRequest = course.PreRequest,
                                Audience = course.Audience,
                                Price = course.Price,
                                CourseCategory = course.Category.Name,
                                CourseType = course.CourseType.Name,
                                CourseLogo = Convert.ToBase64String(course.Logo),
                                Message = Utilities.GetErrorMessages("200"),
                                IsUserSubscribed = application == null ? false : true,
                                IsPaid = application == null ? false : application.isPaid,
                                IsFinished = course.isFinished,
                                Loves = loves.Count(),
                                IsLoved = isLoved,
                                Outline=course.Outline
                            };
                        }
                        else
                        {
                            return new CourseResponse
                            {
                                Message = Utilities.GetErrorMessages("402")
                            };
                        }
                    }
                    else
                    {
                        var loves = db.CoursLoves.Where(co => co.CourseId == id).ToList();
                        return new CourseResponse
                        {
                            Name = course.Name,
                            Hours = course.Hours,
                            BeginDate = course.BeginDate.ToShortDateString(),
                            BeginTime = course.BeginTime.ToString(),
                            EndTime = course.EndTime.ToString(),
                            EndDate = course.EndTime.ToShortDateString(),
                            Instructor = course.Instructor,
                            Description = course.Description,
                            PreRequest = course.PreRequest,
                            Audience = course.Audience,
                            Price = course.Price,
                            CourseCategory = course.Category.Name,
                            CourseType = course.CourseType.Name,
                            CourseLogo = Convert.ToBase64String(course.Logo),
                            Message = Utilities.GetErrorMessages("200"),
                            IsUserSubscribed = false,
                            IsPaid = false,
                            Loves = loves.Count(),
                            IsFinished = course.isFinished
                        };
                    }
                }
                else
                {
                    return new CourseResponse
                    {
                        Message = Utilities.GetErrorMessages("402")
                    };
                }
            }
            catch (Exception ex)
            {
                return new CourseResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }

        }
        [HttpPost]
        public CoursesResponse GetCoursesByCenter(string centerId, int count , int skip)
        {
            try
            {
                var id = int.Parse(centerId);
                var courses = db.Courses.Where(c => c.CenterId == id && c.isStarted == true).OrderByDescending(c => c.isFinished).ToList();
                if (courses.Count != 0)
                {
                    List<SmallCourse> list = new List<SmallCourse>();
                    foreach (var item in courses)
                    {
                        var cname = db.Centers.SingleOrDefault(c => c.Id == item.CenterId).Name;
                        var loves = db.CoursLoves.Where(co => co.CourseId == item.Id).ToList().Count;
                        SmallCourse smallCourse = new SmallCourse
                        {
                            id = item.Id,
                            Name = item.Name,
                            Hours = item.Hours,
                            BeginDate = item.BeginDate.ToShortDateString(),
                            EndDate = item.EndDate.ToShortDateString(),
                            CourseLogo = Convert.ToBase64String(item.Logo),
                            Rate = new Utilities().GetCenterRate(item.CenterId),
                            CenterName = cname,
                            Price = item.Price,
                            Instructor = item.Instructor,
                            Loves = loves,
                            CenterId = id
                        };
                        list.Add(smallCourse);
                    }
                    return new CoursesResponse
                    {
                        CourseList = list.Skip(skip).Take(count).ToList(),
                        Message = Utilities.GetErrorMessages("200")
                    };
                }
                else
                {
                    return new CoursesResponse
                    {
                        Message = Utilities.GetErrorMessages("504")
                    };
                }
            }
            catch (Exception ex)
            {
                return new CoursesResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public CoursesResponse GetCoursesByCategorey(string categoreyId, int count,int skip)
        {
            try
            {
                var id = int.Parse(categoreyId);
                var courses = db.Courses.Where(c => c.CategoryId == id && c.isStarted == true).OrderByDescending(c => c.isFinished).ToList();
                if (courses.Count != 0)
                {
                    List<SmallCourse> list = new List<SmallCourse>();
                    foreach (var item in courses)
                    {
                        var cname = db.Centers.SingleOrDefault(c => c.Id == item.CenterId).Name;
                        var loves = db.CoursLoves.Where(co => co.CourseId == item.Id).ToList().Count;
                        SmallCourse smallCourse = new SmallCourse
                        {
                            id = item.Id,
                            Name = item.Name,
                            Hours = item.Hours,
                            BeginDate = item.BeginDate.ToShortDateString(),
                            EndDate = item.EndDate.ToShortDateString(),
                            CourseLogo = Convert.ToBase64String(item.Logo),
                            Rate = new Utilities().GetCenterRate(item.Id),
                            CenterName = cname,
                            Price = item.Price,
                            Instructor = item.Instructor,
                            Loves = loves,
                            CenterId = item.CenterId
                        };
                        list.Add(smallCourse);
                    }
                    return new CoursesResponse
                    {
                        CourseList = list.Skip(skip).Take(count).ToList(),
                        Message = Utilities.GetErrorMessages("200")
                    };
                }
                else
                {
                    return new CoursesResponse
                    {
                        Message = Utilities.GetErrorMessages("402")
                    };
                }
            }
            catch (Exception ex)
            {
                return new CoursesResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public CoursesResponse GetCurrentCourses(int userId, int count,int skip)
        {
            try
            {
                if (userId != 0)
                {
                    int user = userId;
                    var userCategories = db.UserCategories.Where(uc => uc.UserId == user).ToList();
                    List<SmallCourse> returnedCourses = new List<SmallCourse>();
                    List<Cours> CatsCourses = new List<Cours>();
                    List<Cours> courses = new List<Cours>();
                    foreach (var userCategory in userCategories)
                    {
                        var course = db.Courses.Where(c => c.CategoryId == userCategory.CategoryId && c.isStarted == true && c.isFinished == false).ToList();
                        if (course.Count != 0)
                            courses.AddRange(course);
                    }
                    if (courses.Count() > 0)
                    {
                        CatsCourses.AddRange(courses);
                    }
                    foreach (var item in CatsCourses)
                    {
                        var cname = db.Centers.SingleOrDefault(c => c.Id == item.CenterId).Name;
                        SmallCourse smallCourse = new SmallCourse
                        {
                            id = item.Id,
                            Name = item.Name,
                            Hours = item.Hours,
                            BeginDate = item.BeginDate.ToShortDateString(),
                            EndDate = item.EndDate.ToShortDateString(),
                            CourseLogo = Convert.ToBase64String(item.Logo),
                            Rate = new Utilities().GetCenterRate(item.CenterId),
                            CenterName = cname,
                            Price = item.Price,
                            Instructor = item.Instructor,
                            CenterId = item.CenterId
                        };
                        returnedCourses.Add(smallCourse);
                    }
                    if (returnedCourses.Count == 0)
                        return new CoursesResponse
                        {
                            Message = Utilities.GetErrorMessages("402")
                        };
                    else
                        return new CoursesResponse
                        {
                            CourseList = returnedCourses.Count < count ? returnedCourses : returnedCourses.Skip(skip).Take(count).ToList(),
                            Message = Utilities.GetErrorMessages("200")
                        };
                }
                else
                {
                    var categories = db.Categories.ToList();
                    List<SmallCourse> courses = new List<SmallCourse>();
                    foreach (var category in categories)
                    {
                        var course = category.Courses.Where(c => c.isStarted == true && c.isFinished == false).FirstOrDefault();
                        var cname = "";
                        if (course != null)
                            cname = db.Centers.SingleOrDefault(c => c.Id == course.CenterId).Name;
                        if (course != null)
                        {
                            SmallCourse smallCourse = new SmallCourse
                            {
                                id = course.Id,
                                Name = course.Name,
                                Instructor = course.Instructor,
                                Hours = course.Hours,
                                Price = course.Price,
                                BeginDate = course.BeginDate.ToShortDateString(),
                                EndDate = course.EndDate.ToShortDateString(),
                                CourseLogo = Convert.ToBase64String(course.Logo),
                                CenterName = cname,
                                Rate = new Utilities().GetCenterRate(course.CenterId)
                            };
                            courses.Add(smallCourse);
                        }
                    }
                    if (courses.Count == 0)
                        return new CoursesResponse
                        {
                            Message = Utilities.GetErrorMessages("402")
                        };
                    else
                        return new CoursesResponse
                        {
                            CourseList = courses.Count < count ? courses : courses.Skip(skip).Take(count).ToList(),
                            Message = Utilities.GetErrorMessages("200")
                        };
                }
            }
            catch (Exception ex)
            {
                return new CoursesResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public UserCourses GetUserCourses(int userId,int count , int skip)
        {
            try
            {
                var user = db.EndUsers.Include("Applications").SingleOrDefault(use => use.Id == userId);
                if (user != null)
                {
                    if (user.Applications.Count() > 0)
                    {
                        List<Cours> pendingCourses = new List<Cours>();
                        List<Cours> paidCourses = new List<Cours>();
                        foreach (var Application in user.Applications)
                        {
                            Cours course = db.Courses.SingleOrDefault(Cours => Cours.Id == Application.CourseId);
                            if (course != null)
                            {
                                if (Application.isPaid)
                                    paidCourses.Add(course);
                                else
                                    pendingCourses.Add(course);
                            }
                        }
                        if (paidCourses.Count() > 0 || pendingCourses.Count() > 0)
                        {
                            List<SmallCourse> pendingSmallCourses = new List<SmallCourse>();
                            List<SmallCourse> paidSmallCourses = new List<SmallCourse>();
                            if (paidCourses.Count() > 0)
                            {
                                foreach (var paid in paidCourses)
                                {
                                    var cname = db.Centers.SingleOrDefault(cid => cid.Id == paid.CenterId).Name;
                                    SmallCourse smallCourse = new SmallCourse
                                    {
                                        id = paid.Id,
                                        Name = paid.Name,
                                        Instructor = paid.Instructor,
                                        Hours = paid.Hours,
                                        Price = paid.Price,
                                        BeginDate = paid.BeginDate.ToShortDateString(),
                                        EndDate = paid.EndDate.ToShortDateString(),
                                        CourseLogo = Convert.ToBase64String(paid.Logo),
                                        CenterName = cname,
                                        Rate = new Utilities().GetCenterRate(paid.CenterId),
                                        CenterId = paid.CenterId

                                    };
                                    paidSmallCourses.Add(smallCourse);
                                }
                            }
                            if (pendingCourses.Count() > 0)
                            {
                                foreach (var pending in pendingCourses)
                                {
                                    var cname = db.Centers.SingleOrDefault(cid => cid.Id == pending.CenterId).Name;
                                    SmallCourse smallCourse = new SmallCourse
                                    {
                                        id = pending.Id,
                                        Name = pending.Name,
                                        Instructor = pending.Instructor,
                                        Hours = pending.Hours,
                                        Price = pending.Price,
                                        BeginDate = pending.BeginDate.ToShortDateString(),
                                        EndDate = pending.EndDate.ToShortDateString(),
                                        CourseLogo = Convert.ToBase64String(pending.Logo),
                                        CenterName = cname,Rate= new Utilities().GetCenterRate(pending.CenterId),
                                        CenterId = pending.CenterId
                                    };
                                    pendingSmallCourses.Add(smallCourse);
                                }
                            }
                            return new UserCourses
                            {
                                PaidCourses = paidSmallCourses.Skip(skip).Take(count).ToList(),
                                PendingCourses = pendingSmallCourses.Skip(skip).Take(count).ToList(),
                                Message = Utilities.GetErrorMessages("200")
                            };
                        }
                        else
                        {
                            return new UserCourses
                            {
                                Message = Utilities.GetErrorMessages("402")
                            };
                        }

                    }
                    else
                    {
                        return new UserCourses
                        {
                            Message = Utilities.GetErrorMessages("402")
                        };
                    }
                }
                else
                {
                    return new UserCourses
                    {
                        Message = Utilities.GetErrorMessages("402")
                    };
                }
            }
            catch (Exception ex)
            {
                return new UserCourses
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public BaseResponse CourseLove(int userId, int courseId)
        {
            try
            {
                var loved = db.CoursLoves.SingleOrDefault(lov => lov.CourseId == courseId && lov.EndUserId == userId);
                if (loved == null)
                {
                    CoursLove love = new CoursLove
                    {
                        EndUserId = userId,
                        CourseId = courseId
                    };
                    db.CoursLoves.Add(love);
                    db.SaveChanges();
                }
                else
                {
                    db.CoursLoves.Remove(loved);
                    db.SaveChanges();
                }
                return new UserCourses
                {
                    Message = Utilities.GetErrorMessages("200")
                };
            }
            catch (Exception ex)
            {
                return new UserCourses
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public CoursesResponse GetCoursesByCenterCategories(string centerId,string categoryId, int count,int skip)
        {
            try
            {
                var id = int.Parse(centerId);
                var catid = int.Parse(categoryId);
                var courses = db.Courses.Where(c => c.CenterId == id && c.CategoryId== catid && c.isStarted == true).OrderByDescending(c => c.isFinished).ToList();
                if (courses.Count != 0)
                {
                    List<SmallCourse> list = new List<SmallCourse>();
                    foreach (var item in courses)
                    {
                        var cname = db.Centers.SingleOrDefault(c => c.Id == item.CenterId).Name;
                        var loves = db.CoursLoves.Where(co => co.CourseId == item.Id).ToList().Count;
                        SmallCourse smallCourse = new SmallCourse
                        {
                            id = item.Id,
                            Name = item.Name,
                            Hours = item.Hours,
                            BeginDate = item.BeginDate.ToShortDateString(),
                            EndDate = item.EndDate.ToShortDateString(),
                            CourseLogo = Convert.ToBase64String(item.Logo),
                            Rate = new Utilities().GetCenterRate(item.CenterId),
                            CenterName = cname,
                            Price = item.Price,
                            Instructor = item.Instructor,
                            Loves = loves,
                            CenterId = item.CenterId
                        };
                        list.Add(smallCourse);
                    }
                    return new CoursesResponse
                    {
                        CourseList = list.Skip(skip).Take(count).ToList(),
                        Message = Utilities.GetErrorMessages("200")
                    };
                }
                else
                {
                    return new CoursesResponse
                    {
                        Message = Utilities.GetErrorMessages("504")
                    };
                }
            }
            catch (Exception ex)
            {
                return new CoursesResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
    }
}
