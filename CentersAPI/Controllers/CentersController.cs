using CentersAPI.Helpers;
using CentersAPI.Models.EFModels;
using CentersAPI.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;

namespace CentersAPI.Controllers
{
    [BasicAuthentication]
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
                    SmallCenter smallCenter = new SmallCenter
                    {
                        CenterName = center.Name,
                        CenterRate = (int)new Utilities().GetCenterRate(center.Id),
                        Id = center.Id,
                        Email = center.Email,
                        Location = center.LocationLat + ";" + center.LocationLong,
                        Logo = Convert.ToBase64String(center.Logo),
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
        public CenterResponse GetCenterProfile(int userId, int centerId)
        {
            try
            {
                var center = db.Centers.SingleOrDefault(cen => cen.Id == centerId);
                if (center != null)
                {
                    bool userRated = db.CenterRates.Any(r => r.UserId == userId && r.CenterId == centerId);
                    SmallCenter smallCenter = new SmallCenter
                    {
                        CenterName = center.Name,
                        CenterRate = userRated ? 0 : (int)new Utilities().GetCenterRate(center.Id),
                        Id = center.Id,
                        Email = center.Email,
                        Location = center.LocationLat + "*" + center.LocationLong,
                        Logo = Convert.ToBase64String(center.Logo),
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
        public List<string> GetCenterImages(int centerId)
        {
            try
            {
                var ceIm = db.CenterImages.Where(ci => ci.CenterId == centerId).ToList();
                List<string> images = new List<string>();
                if (ceIm.Count > 0)
                {
                    foreach (var ci in ceIm)
                    {
                        images.Add(Convert.ToBase64String(ci.Image));
                    }
                }
                return images;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}
