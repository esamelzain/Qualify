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
    public class SearchEngineController : ApiController
    {
        private Entities db = new Entities();
        [HttpPost]
        public SearchResponse DoSearch(int userId, string key)
        {
            try
            {
                UserSearchHistory userSearchHistory = new UserSearchHistory();
                userSearchHistory.UserId = userId;
                userSearchHistory.Keyword = key;
                var BaseCourses = db.Courses.Include("Category").Include("Center").Where(c => (c.isStarted == true) && (c.Name.Contains(key) || c.Center.Name.Contains(key) || c.Category.Name.Contains(key))).ToList();
                var BaseCenters = db.Centers.Include("Category").Include("Courses").Where(c => c.IsConfirmed).ToList();
                var BaseCategories = db.Categories.Include("Courses").Include("Centers").ToList();
                List<Course> BigCourses = new List<Course>();
                List<Center> BigCenters = new List<Center>();
                List<SmallCenter> SmallCenter = new List<SmallCenter>();
                List<SmallCourse> SmallCourse = new List<SmallCourse>();
                //Remove Duplicate
                BigCourses = BaseCourses;
                foreach (var basecenter in BaseCenters)
                {
                    if (basecenter.Name.Contains(key))
                    {
                        BigCenters.Add(basecenter);
                    }
                    var cats = db.TrainningCenterCategories.Include("Category").Where(c => c.CentersId == basecenter.Id).ToList();
                    foreach (var item in cats)
                    {
                        if (item.Category.Name.Contains(key))
                        {
                            BigCenters.Add(basecenter);
                        }
                    }
                }
                foreach (var bigCenter in BigCenters)
                {
                    SmallCenter SM = new SmallCenter
                    {
                        CenterName = bigCenter.Name,
                        CenterRate = (int)new Utilities().GetCenterRate(bigCenter.Id),
                        Id = bigCenter.Id,
                        Email = bigCenter.Email,
                        Location = bigCenter.LocationLat + ";" + bigCenter.LocationLong,
                        Logo = Convert.ToBase64String(bigCenter.Logo),
                        Phones = new List<string> { bigCenter.Phone1, bigCenter.Phone2, bigCenter.Phone3 }
                    };
                    SmallCenter.Add(SM);
                }
                foreach (var bigCourse in BigCourses)
                {
                    var cname = db.Centers.SingleOrDefault(c => c.Id == bigCourse.CenterId).Name;
                    var loves = db.CoursLoves.Where(co => co.CourseId == bigCourse.Id).ToList().Count;
                    SmallCourse SC = new SmallCourse
                    {
                        id = bigCourse.Id,
                        Name = bigCourse.Name,
                        Hours = bigCourse.Hours,
                        BeginDate = bigCourse.BeginDate.ToShortDateString(),
                        EndDate = bigCourse.EndDate.ToShortDateString(),
                        CourseLogo = Convert.ToBase64String(bigCourse.Logo),
                        Rate = new Utilities().GetCenterRate(bigCourse.CenterId),
                        CenterName = cname,
                        Price = bigCourse.Price,
                        Instructor = bigCourse.Instructor,
                        Loves = loves,
                        CenterId = bigCourse.CenterId
                    };
                    SmallCourse.Add(SC);
                    var BC = db.Centers.SingleOrDefault(cen => cen.Id == SC.CenterId);
                    SmallCenter SM = new SmallCenter
                    {
                        CenterName = BC.Name,
                        CenterRate = (int)new Utilities().GetCenterRate(BC.Id),
                        Id = BC.Id,
                        Email = BC.Email,
                        Location = BC.LocationLat + ";" + BC.LocationLong,
                        Logo = Convert.ToBase64String(BC.Logo),
                        Phones = new List<string> { BC.Phone1, BC.Phone2, BC.Phone3 }
                    };
                    foreach (var Small in SmallCenter)
                    {
                        if (SM.Id != Small.Id)
                        {
                            SmallCenter.Add(SM);
                        }
                    }
                }
                SmallCenter = SmallCenter.Distinct().ToList();
                var result = new SearchResponse
                {
                    smallCenters = SmallCenter,
                    smallCourses = SmallCourse,
                    Message = Utilities.GetErrorMessages("200")
                };
                userSearchHistory.Result = System.Web.Helpers.Json.Encode(result);
                db.UserSearchHistories.Add(userSearchHistory);
                return result;
            }
            catch (Exception ex)
            {
                return new SearchResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public List<string> AutoComplete(int userId, string key)
        {
            try
            {
                List<string> AutoComplete = new List<string>();
                var centers = db.Centers.Where(c => c.Name.Contains(key));
                var courses = db.Courses.Where(c => c.Name.Contains(key));
                var categories = db.Categories.Where(c => c.Name.Contains(key));
                foreach (var center in centers)
                {
                    AutoComplete.Add(center.Name);
                }
                foreach (var course in courses)
                {
                    AutoComplete.Add(course.Name);
                }
                foreach (var category in categories)
                {
                    AutoComplete.Add(category.Name);
                }
                List<UserFavourite> userFavourites = new List<UserFavourite>();
                foreach (var item in AutoComplete)
                {
                    userFavourites.Add(new UserFavourite
                    {
                        EndUserId = userId,
                        FavouriteId = key,
                        FavouriteType = item
                    });
                }
                db.UserFavourites.AddRange(userFavourites);
                db.SaveChanges();
                return AutoComplete;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
        [HttpPost]
        public List<string> RecomendedSearches(int userId)
        {
            try
            {
                var categories = db.UserCategories.Where(uc=>uc.UserId== userId).ToList();
                List<string> CourseNames = new List<string>();
                foreach (var category in categories)
                {
                    string cName = new Utilities().TopCourseName(category.CategoryId);
                    if(cName!="")
                        CourseNames.Add(cName);
                }
                return CourseNames;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}
