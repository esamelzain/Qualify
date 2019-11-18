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
    public class CentersController : ApiController
    {
        private Entities db = new Entities();
        [HttpPost]
        public BaseResponse RateCenter(int userId, int centerId, int userRate)
        {
            try
            {
                if (db.CenterRates.Any(rate => rate.UserId == userId && rate.CenterId == centerId))
                {
                    var rate = db.CenterRates.SingleOrDefault(rat => rat.UserId == userId && rat.CenterId == centerId);
                    db.CenterRates.Remove(rate);
                    db.SaveChanges();
                    return new BaseResponse
                    {
                        Message = Utilities.GetErrorMessages("200"),
                    };
                }
                else
                {
                    db.CenterRates.Add(new CenterRate
                    {
                        CenterId = centerId,
                        UserId = userId,
                        Rate = userRate
                    });
                    db.SaveChanges();
                    return new BaseResponse
                    {
                        Message = Utilities.GetErrorMessages("200"),
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Message = Utilities.GetErrorMessages("500"),
                };
            }
        }
        [HttpPost]
        public CentersResponse GetCenters(int count, int skip)
        {
            try
            {
                var bigCenters = db.Centers.Where(c => c.IsConfirmed).ToList();
                List<SmallCenter> SmallCenters = new List<SmallCenter>();
                foreach (var center in bigCenters)
                {
                    var ceIm = db.CenterImages.Where(ci => ci.CenterId == center.Id).ToList();
                    List<string> images = new List<string>();
                    if (ceIm.Count > 0)
                    {
                        foreach (var ci in ceIm)
                        {
                            images.Add(Convert.ToBase64String(ci.Image));
                        }
                    }
                    SmallCenter smallCenter = new SmallCenter
                    {
                        CenterName = center.Name,
                        CenterRate = (int)new Utilities().GetCenterRate(center.Id),
                        Id = center.Id,
                        Email = center.Email,
                        Location = center.LocationLat + ";" + center.LocationLong,
                        Logo = Convert.ToBase64String(center.Logo),
                        Images = images,
                        Phones = new List<string> { center.Phone1, center.Phone2, center.Phone3 }
                    };
                    SmallCenters.Add(smallCenter);
                }
                SmallCenters = SmallCenters.OrderByDescending(c => c.CenterRate).ToList();
                return new CentersResponse
                {
                    Message = Utilities.GetErrorMessages("200"),
                    centers = SmallCenters.Skip(skip).Take(count).ToList()
                };
            }
            catch (Exception)
            {
                return new CentersResponse
                {
                    Message = Utilities.GetErrorMessages("500"),
                };
            }
        }
        [HttpPost]
        public CenterResponse GetCenterProfile(int centerId)
        {
            try
            {
                var center = db.Centers.SingleOrDefault(cen => cen.Id == centerId);
                if (center != null)
                {
                    var ceIm = db.CenterImages.Where(ci => ci.CenterId == center.Id).ToList();
                    List<string> images = new List<string>();
                    if (ceIm.Count > 0)
                    {
                        foreach (var ci in ceIm)
                        {
                            images.Add(Convert.ToBase64String(ci.Image));
                        }
                    }
                    SmallCenter smallCenter = new SmallCenter
                    {
                        CenterName = center.Name,
                        CenterRate = (int)new Utilities().GetCenterRate(center.Id),
                        Id = center.Id,
                        Email = center.Email,
                        Location = center.LocationLat + "*" + center.LocationLong,
                        Logo = Convert.ToBase64String(center.Logo),
                        Images = images,
                        Phones = new List<string> { center.Phone1, center.Phone2, center.Phone3 }
                    };
                    return new CenterResponse
                    {
                        center = smallCenter,
                        Message = Utilities.GetErrorMessages("200")
                    };
                }
                else
                {
                    return new CenterResponse
                    {
                        Message = Utilities.GetErrorMessages("402"),
                    };
                }
            }
            catch (Exception ex)
            {
                return new CenterResponse
                {
                    Message = Utilities.GetErrorMessages("500"),
                };
            }
        }
        [HttpPost]
        public SearchResponse DoSearch(string key)
        {
            try
            {
                var BaseCourses = db.Courses.Include("Category").Include("Center").Where(c => (c.isStarted == true) && (c.Name.Contains(key) || c.Center.Name.Contains(key) || c.Category.Name.Contains(key))).ToList();
                var BaseCenters = db.Centers.Include("Category").Include("Courses").Where(c => c.IsConfirmed).ToList();
                var BaseCategories = db.Categories.Include("Courses").Include("Centers").ToList();
                List<Cours> BigCourses = new List<Cours>();
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
                    var ceIm = db.CenterImages.Where(ci => ci.CenterId == bigCenter.Id).ToList();
                    List<string> images = new List<string>();
                    if (ceIm.Count > 0)
                    {
                        foreach (var ci in ceIm)
                        {
                            images.Add(Convert.ToBase64String(ci.Image));
                        }
                    }
                    SmallCenter SM = new SmallCenter
                    {
                        CenterName = bigCenter.Name,
                        CenterRate = (int)new Utilities().GetCenterRate(bigCenter.Id),
                        Id = bigCenter.Id,
                        Email = bigCenter.Email,
                        Location = bigCenter.LocationLat + ";" + bigCenter.LocationLong,
                        Logo = Convert.ToBase64String(bigCenter.Logo),
                        Images = images,
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
                }
                SmallCenter = SmallCenter.Distinct().ToList();
                return new SearchResponse
                {
                    smallCenters = SmallCenter,
                    smallCourses = SmallCourse,
                    Message = Utilities.GetErrorMessages("200")
                };
            }
            catch (Exception ex)
            {
                return new SearchResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
    }
}
