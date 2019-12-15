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
    public class CategoriesController : ApiController
    {
        private Entities db = new Entities();
        [HttpPost]
        public Categories GetCenterCategories(int centerId)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                if (centerId != 0)
                {
                    var centerCats = db.TrainningCenterCategories.Where(ccat => ccat.CentersId == centerId).ToList();
                    List<UserCategories> categories = new List<UserCategories>();
                    Console.WriteLine(centerCats.Count);
                    foreach (var category in centerCats)
                    {
                        var smallCat = db.Categories.SingleOrDefault(cat => cat.Id == category.CategoryId);
                        var courseCount = db.Courses.Where(cou => cou.CategoryId == category.CategoryId).Count();
                        Console.WriteLine(smallCat.Id);
                        categories.Add(new UserCategories
                        {
                            Id = smallCat.Id,
                            Name = smallCat.Name,
                            Logo = Convert.ToBase64String(smallCat.Logo),
                            Courses = courseCount
                        });
                    }
                    if (categories.Count != 0)
                        return new Categories()
                        {
                            categories = categories,
                            Message = Utilities.GetErrorMessages("200")
                        };
                }
                var temps = db.Categories.ToList();
                List<UserCategories> temps2 = new List<UserCategories>();
                foreach (var temp in temps)
                {
                    var courseCount = db.Courses.Where(cou => cou.CategoryId == temp.Id).Count();
                    UserCategories temp2 = new UserCategories
                    {
                        Id = temp.Id,
                        Name = temp.Name,
                        Logo = Convert.ToBase64String(temp.Logo),
                        Courses = courseCount

                    };
                    temps2.Add(temp2);
                }
                return new Categories()
                {
                    categories = temps2,
                    Message = Utilities.GetErrorMessages("200")
                };
            }
            catch (Exception ex)
            {
                return new Categories()
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public Categories GetUserCategories(int? userId, int count,int skip)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                if (userId != null && userId != 0)
                {
                    var user = db.EndUsers.Include("UserCategories").SingleOrDefault(cent => cent.Id == userId);
                    List<UserCategories> Categories = new List<UserCategories>();
                    if (user != null)
                    {
                        foreach (var cat in user.UserCategories)
                        {
                            var courseCount = db.Courses.Where(cou => cou.CategoryId == cat.CategoryId && cou.isStarted == true).Count();
                            Category category = db.Categories.SingleOrDefault(cate => cate.Id == cat.CategoryId);
                            UserCategories temp = new UserCategories
                            {
                                Id = category.Id,
                                Name = category.Name,
                                Logo = Convert.ToBase64String(category.Logo),
                                Courses = courseCount
                            };
                            Categories.Add(temp);
                        }
                        return new Categories()
                        {
                            categories = Categories.Count < count ? Categories : Categories.Skip(skip).Take(count).ToList(),
                            Message = Utilities.GetErrorMessages("200")
                        };
                    }
                    else
                    {
                        return new Categories()
                        {
                            Message = Utilities.GetErrorMessages("402")
                        };
                    }
                }
                else
                {
                    var temps = db.Categories.ToList();
                    List<UserCategories> temps2 = new List<UserCategories>();
                    foreach (var temp in temps)
                    {
                        var courseCount = db.Courses.Where(cou => cou.CategoryId == temp.Id).Count();

                        UserCategories temp2 = new UserCategories
                        {
                            Id = temp.Id,
                            Name = temp.Name,
                            Logo = Convert.ToBase64String(temp.Logo),
                            Courses = courseCount
                        };
                        temps2.Add(temp2);
                    }
                    return new Categories()
                    {
                        categories = temps2.Count < count ? temps2 : temps2.Take(count).ToList(),
                        Message = Utilities.GetErrorMessages("200")
                    };
                }
            }
            catch (Exception)
            {
                return new Categories()
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public BaseResponse AddCategoriesToUser(int userId, List<int> ids)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                List<UserCategory> catlist = new List<UserCategory>();
                var indb = db.UserCategories.Where(u => u.UserId == userId).ToList();
                if (indb.Count() > 0)
                {
                    db.UserCategories.RemoveRange(indb);
                    db.SaveChanges();
                }
                foreach (var id in ids)
                {
                    var uc = db.UserCategories.SingleOrDefault(ucc => ucc.CategoryId == id & ucc.UserId == userId);
                    if (uc == null)
                    {
                        UserCategory ucat = new UserCategory
                        {
                            CategoryId = id,
                            UserId = userId
                        };
                        catlist.Add(ucat);
                    }
                }
                db.UserCategories.AddRange(catlist);
                db.SaveChanges();
                return new BaseResponse()
                {
                    Message = Utilities.GetErrorMessages("200")
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
    }
}
