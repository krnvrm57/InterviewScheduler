using InterviewScheduler.Common;
using InterviewScheduler.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace InterviewScheduler.Controllers
{
    public class LandingController : Controller
    {
        CrimsonDBHandle db = new CrimsonDBHandle();
        // GET: Landing
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel Loginuser, string ReturnUrl = "")
        {
            Logger.LogEntry(Logger.ErrorType.Message, "", "", "", "Requested to login for user " + Loginuser?.UserName);
            ADServices.AD _Ad = new ADServices.AD();
            Models.User _user = new Models.User();
            List<string> result = new List<string>();
            try
            {
                try
                {
                    Loginuser.UserName = Loginuser.UserName.Split('@')[0].ToString();
                }
                catch (Exception)
                {
                }
                string DisplayName = "";
                string AuthCookieName = ConfigurationSettings.AppSettings["AuthCookieName"].ToString();
                var identity = WindowsIdentity.GetCurrent();

                MyPrincipalModel mp = new MyPrincipalModel(identity);
                User usr = new Models.User();

                Logger.LogEntry(Logger.ErrorType.Message, "", "", "", "Verifying Credentials for user " + Loginuser.UserName);

                var response = _Ad.IsValidateCredentials(Loginuser.UserName, Utility.EncodeToBase64(Loginuser.Password));
                var json = JsonConvert.SerializeObject(response);
                Logger.LogEntry(Logger.ErrorType.Message, "", "", "", "credentials service response " + response);
                if (ConfigurationManager.AppSettings["ENV"].ToString() == "DEV")
                {
                    Logger.LogEntry(Logger.ErrorType.Message, "", "", "", "using test account");
                    //EmployeeModel model = db.GetEmployeeInfo(ConfigurationManager.AppSettings["TESTEMAILID"].ToString());
                    EmployeeModel model = db.GetEmployeeInfoByUSerName(Loginuser.UserName);

                    DisplayName = model.FullName;
                    if (model != null && model.EmailId != null && model.Password == Loginuser.Password)
                    {
                        usr.EmailAddress = model.EmailId;
                        usr.Id = model.Id;
                        usr.Name = model.FullName;
                        usr.Manager = "";
                        usr.Groups = model.Role;
                        usr.loginSession = Guid.NewGuid().ToString().ToUpper();
                        usr.Designation = model.Designation;
                        string initials = "";
                        foreach (var item in model.FullName.Split(' '))
                        {
                            initials = initials + item.Substring(0, 1).ToUpper();
                        }
                        usr.NameInit = initials;
                        mp.User = usr;
                        var serializer = new JavaScriptSerializer();
                        string userData = serializer.Serialize(usr);
                        var authTicket = new FormsAuthenticationTicket(
                           1,
                           DisplayName,
                           DateTime.Now,
                             DateTime.Now.AddMinutes(Convert.ToInt32(FormsAuthentication.Timeout.Minutes)),
                           // DateTime.Now.AddMinutes(1),
                           true,
                           userData
                       );
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(AuthCookieName, encryptedTicket);
                        System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                        FormsAuthentication.SetAuthCookie(identity.User.Value, true);
                        usr.ThumbnailPhoto = model.ThumbnailPhoto;
                        Session.Add("AuthCookieName" + usr.Id, usr);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            Logger.LogEntry(Logger.ErrorType.Message, "", "", "", "redirected to Dashboard");
                            return Redirect("/task");
                        }

                    }
                    else
                    {
                        Loginuser.ErrorMessage = Message.LoginError;
                        Logger.LogEntry(Logger.ErrorType.Error, "Landing", "Submit", "Login failed for username " + Loginuser.UserName, Loginuser.ErrorMessage);
                        return View("~/Views/Landing/Index.cshtml", Loginuser);
                    }
                }
                else
                {
                    if (response.Status.ToLower() == AuthStatus.Success.ToLower())
                    {
                        db.AddUpdateEmployeeInfo(response);
                        EmployeeModel model = db.GetEmployeeInfo(response.EmailId);
                        DisplayName = model.FullName;
                        if (model != null && model.EmailId != null)
                        {
                            usr.EmailAddress = model.EmailId;
                            usr.Id = model.Id;
                            usr.Name = model.FullName;
                            usr.Manager = "";
                            usr.Groups = model.Role;
                            usr.ThumbnailPhoto = model.ThumbnailPhoto;
                            usr.loginSession = response.AccessToken.ToUpper();
                            usr.Designation = model.Designation;
                            string initials = "";
                            foreach (var item in model.FullName.Split(' '))
                            {
                                initials = initials + item.Substring(0, 1).ToUpper();
                            }
                            usr.NameInit = initials;
                            mp.User = usr;
                            var serializer = new JavaScriptSerializer();
                            string userData = serializer.Serialize(usr);
                            var authTicket = new FormsAuthenticationTicket(
                               1,
                               DisplayName,
                               DateTime.Now,
                              DateTime.Now.AddMinutes(Convert.ToInt32(FormsAuthentication.Timeout.Minutes)),
                               //DateTime.Now.AddMinutes(1),
                               true,
                               userData
                           );
                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            var authCookie = new HttpCookie(AuthCookieName, encryptedTicket);
                            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                            FormsAuthentication.SetAuthCookie(identity.User.Value, false);
                            Logger.LogEntry(Logger.ErrorType.Message, "", "", "", "User Loggedin from \r \n IP Address: " + HttpContext.Request.UserHostAddress.ToString() + " \r \n HostName :" + HttpContext.Server.MachineName + "\r \n");

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                Logger.LogEntry(Logger.ErrorType.Message, "", "", "", "redirected to Dashboard");
                                return Redirect("/task");
                            }

                        }
                        else
                        {
                            Loginuser.ErrorMessage = Message.LoginError;
                            Logger.LogEntry(Logger.ErrorType.Error, "Landing", "Submit", "Login failed for username " + Loginuser.UserName, Loginuser.ErrorMessage);
                            return View("~/Views/Landing/Index.cshtml", Loginuser);
                        }
                    }
                    else
                    {
                        Loginuser.ErrorMessage = Message.LoginError;
                        Logger.LogEntry(Logger.ErrorType.Error, "Landing", "Submit", "Login failed for username " + Loginuser.UserName, Loginuser.ErrorMessage);
                        return View("~/Views/Landing/Index.cshtml", Loginuser);
                    }
                }
            }
            catch (Exception ex)
            {
                Loginuser.ErrorMessage = Message.Exception;
                Logger.LogEntry(Logger.ErrorType.Error, "Landing", "Submit", ex.StackTrace, ex.Message.ToString());
                return View("~/Views/Landing/Index.cshtml", Loginuser);
            }

        }
        public ActionResult SignOut()
        {
            LoginModel Loginuser = new LoginModel();

            if (((MyPrincipalModel)System.Web.HttpContext.Current.User) != null && ((MyPrincipalModel)System.Web.HttpContext.Current.User).User != null)
            {
                var _usr = ((MyPrincipalModel)System.Web.HttpContext.Current.User).User;
                Logger.LogEntry(Logger.ErrorType.Message, "", "", "", "User Signout for " + _usr.Name);
            }
            else
            {
                Logger.LogEntry(Logger.ErrorType.Message, "", "", "", "User Signout");
            }
            FormsAuthentication.SignOut();
            Session.Abandon();
            // clear authentication cookie
            string AuthCookieName = ConfigurationSettings.AppSettings["AuthCookieName"].ToString();
            HttpCookie cookie1 = new HttpCookie(AuthCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);
            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            HttpContext.User = null;
            HttpContext.Request.Cookies.Clear();
            Logger.LogEntry(Logger.ErrorType.Message, "", "", "", "User Loggedin from \r \n IP Address: " + HttpContext.Request.UserHostAddress.ToString() + " \r \n HostName :" + HttpContext.Server.MachineName + "\r \n");

            return View("~/Views/Landing/Index.cshtml", Loginuser);
        }
    }
}