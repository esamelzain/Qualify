using CentersAPI.Helpers;
using CentersAPI.Models.EFModels;
using CentersAPI.Models.Requests;
using CentersAPI.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CentersAPI.Controllers
{
    [BasicAuthentication]
    public class UserController : ApiController
    {
        private Entities db = new Entities();
        [HttpPost]
        public LoginResponse Login(LoginRequest request)
        {
            try
            {
                var user = db.EndUsers.SingleOrDefault(
                    u => (u.Phone == request.UserName ||
                    u.Email == request.UserName) &&
                    u.Password == request.Password);
                if (user != null)
                {
                    return new LoginResponse
                    {
                        Id = user.Id,
                        Confirmed = user.EmailConfirmed,
                        LoggedIn = true,
                        Message = Utilities.GetErrorMessages("200"),
                        User = request
                    };
                }
                else
                {
                    return new LoginResponse
                    {
                        LoggedIn = false,
                        Message = Utilities.GetErrorMessages("404"),
                        User = request
                    };
                }

            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    LoggedIn = false,
                    Message = Utilities.GetErrorMessages("500"),
                    User = request
                };
            }
        }
        [HttpPost]
        public RegisterResponse Register(RegisterRequest request)
        {
            try
            {
                if (request.Email == string.Empty ||
                    request.Phone == string.Empty ||
                    request.Name == string.Empty ||
                    request.Password == string.Empty)
                {
                    return new RegisterResponse
                    {
                        Registered = false,
                        Message = Utilities.GetErrorMessages("511"),
                        User = request
                    };
                }
                else
                {
                    var user = db.EndUsers.SingleOrDefault(
                    u => (u.Phone == request.Phone ||
                    u.Email == request.Email));
                    if (user != null)
                    {
                        return new RegisterResponse
                        {
                            Registered = false,
                            Message = Utilities.GetErrorMessages("510"),
                            User = request
                        };
                    }
                    else
                    {
                        EndUser newUser = new EndUser();
                        newUser.EmailConfirmed = false;
                        newUser.VerifyCode = new Random().Next(1000, 9999).ToString();
                        newUser.Phone = request.Phone;
                        newUser.Email = request.Email;
                        newUser.Name = request.Name;
                        newUser.Password = request.Password;
                        db.EndUsers.Add(newUser);
                        db.SaveChanges();
                        var EmailSent = Utilities.sendMail(newUser.VerifyCode, newUser.Email);
                        return new RegisterResponse
                        {
                            Id = newUser.Id,
                            Registered = true,
                            User = request,
                            Message = Utilities.GetErrorMessages("200")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new RegisterResponse
                {
                    Registered = false,
                    Message = Utilities.GetErrorMessages("500"),
                    User = request
                };
            }
        }
        [HttpPost]
        public BaseResponse ReSendCode(string email)
        {
            try
            {
                var user = db.EndUsers.SingleOrDefault(u => u.Email == email);
                if (user != null)
                {
                    var newCode = new Random().Next(1000, 9999).ToString();
                    if (Utilities.sendMail(newCode, email))
                    {
                        user.VerifyCode = newCode;
                        db.SaveChanges();
                        return new BaseResponse
                        {
                            Message = Utilities.GetErrorMessages("200")
                        };
                    }
                    else
                    {
                        return new BaseResponse
                        {
                            Message = Utilities.GetErrorMessages("500")
                        };
                    }
                }
                return new BaseResponse
                {
                    Message = Utilities.GetErrorMessages("404")
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public VerifyCodeResponse VerifyCode(VerifyCodeRequest request)
        {
            try
            {
                var user = db.EndUsers.SingleOrDefault(u => u.Phone == request.UserName || u.Email == request.UserName);
                if (user != null)
                {
                    if (user.VerifyCode == request.Code)
                    {
                        user.EmailConfirmed = true;
                        db.SaveChanges();
                        return new VerifyCodeResponse
                        {
                            Message = Utilities.GetErrorMessages("200"),
                            Verified = true,
                            User = new LoginResponse
                            {
                                Confirmed = true,
                                LoggedIn = true,
                                Message = Utilities.GetErrorMessages("200"),
                                User = new LoginRequest
                                {
                                    Password = user.Password,
                                    UserName = request.UserName
                                }

                            }
                        };
                    }
                    else
                    {
                        return new VerifyCodeResponse
                        {
                            Verified = false,
                            Message = Utilities.GetErrorMessages("509"),
                            User = null
                        };
                    }
                }
                else
                {
                    return new VerifyCodeResponse
                    {
                        Verified = false,
                        Message = Utilities.GetErrorMessages("507"),
                        User = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new VerifyCodeResponse
                {
                    Verified = false,
                    Message = Utilities.GetErrorMessages("500"),
                    User = null
                };
            }
        }
        [HttpPost]
        public BaseResponse ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                int id = 0;
                try
                {
                    id = int.Parse(request.UserName);
                }
                catch (Exception)
                {
                }
                var user = db.EndUsers.SingleOrDefault(u => (u.Email == request.UserName || u.Phone == request.UserName || u.Id == id) && u.Password == request.OldPassword);
                if (user != null)
                {
                    user.Password = request.NewPassword;
                    db.SaveChanges();
                    return new BaseResponse
                    {
                        Message = Utilities.GetErrorMessages("210")
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        Message = Utilities.GetErrorMessages("404")
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public BaseResponse EditProfile(ProfileRequest profileRequest, int userId)
        {
            try
            {
                var endUser = db.EndUsers.SingleOrDefault(user => user.Id == userId);
                var dbuser = db.EndUsers.SingleOrDefault(u => u.Id == userId);
                if (dbuser != null)
                {
                    if (dbuser.Email != profileRequest.Email || dbuser.Phone != profileRequest.Phone)
                    {
                        if (db.EndUsers.Any(us => us.Email == profileRequest.Email || us.Phone == profileRequest.Phone))
                        {
                            return new BaseResponse
                            {
                                Message = Utilities.GetErrorMessages("441")
                            };
                        }
                    }
                }
                db.Entry(endUser).CurrentValues.SetValues(profileRequest);
                db.SaveChanges();
                return new BaseResponse
                {
                    Message = Utilities.GetErrorMessages("210")
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        [Cache(TimeDuration = 100)]
        public async Task<UserProfile> GetUserProfile(string userName)
        {
            try
            {
                int uid = 0;
                try
                {
                    uid = int.Parse(userName);
                }
                catch (Exception)
                {
                }
                var user = await db.EndUsers.SingleOrDefaultAsync(use => use.Phone == userName || use.Email == userName || use.Id == uid);
                if (user != null)
                {
                    Profile pro = new Profile
                    {
                        Id = user.Id,
                        CareerLevel = user.CareerLevel,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        Name = user.Name,
                        Password = user.Password,
                        Phone = user.Phone,
                        VerifyCode = user.VerifyCode
                    };
                    return new UserProfile
                    {
                        endUser = pro,
                        Message = Utilities.GetErrorMessages("200")
                    };
                }
                else
                    return new UserProfile
                    {
                        Message = Utilities.GetErrorMessages("402")
                    };
            }
            catch (Exception ex)
            {
                return new UserProfile
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        [Cache(TimeDuration = 100)]
        public async Task<BaseResponse> CreateNewPassword(string userName, string newPassword)
        {
            try
            {
                int uid = 0;
                try
                {
                    uid = int.Parse(userName);
                }
                catch (Exception)
                {
                }
                var user = await db.EndUsers.SingleOrDefaultAsync(use => use.Phone == userName || use.Email == userName || use.Id == uid);
                if (user != null)
                {
                    user.Password = newPassword;
                    await db.SaveChangesAsync();
                    return new BaseResponse
                    {
                        Message = Utilities.GetErrorMessages("200")
                    };
                }
                else
                    return new UserProfile
                    {
                        Message = Utilities.GetErrorMessages("402")
                    };
            }
            catch (Exception)
            {
                return new UserProfile
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
        [HttpPost]
        public NotificationResponse PushNoti(int userId)
        {
            try
            {
                var user = db.EndUsers.Find(userId);
                var notification = db.Notifications.Where(c => c.id > user.NotificationId).ToList();
                if (notification.Count == 0)
                    user.NotificationId = 0;
                else
                    user.NotificationId = notification.Max(c => c.id);
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new NotificationResponse
                {
                    Message = Utilities.GetErrorMessages("200"),
                    Notifications = notification
                };
            }
            catch (Exception ex)
            {
                return new NotificationResponse
                {
                    Message = Utilities.GetErrorMessages("500")
                };
            }
        }
    }
}
