using InterviewScheduler.Common;
using InterviewScheduler.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace InterviewScheduler
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string AuthCookieName = ConfigurationSettings.AppSettings["AuthCookieName"].ToString();

        FormsAuthenticationTicket authTicket;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error(Object sender, EventArgs e)
        {

            var errordata = Server.GetLastError().GetBaseException();
            ExceptionModel _exception = new ExceptionModel();
            Response.Clear();
            Server.ClearError();
            _exception.ErrorCode = _exception.ErrorCode;
            _exception.Message = errordata.Message;
            _exception.Description = errordata.StackTrace.ToString();
            _exception.ErrorId = Guid.NewGuid().ToString().ToUpper();
            HttpContext.Current.Session.Add("_Error", _exception);
            Response.Redirect("/error", false);
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            string executionPath = HttpContext.Current.Request.CurrentExecutionFilePath;
            HttpCookie authCookie = Context.Request.Cookies[AuthCookieName];

            if (authCookie == null || authCookie.Value == "")
            {
                Logger.LogEntry(Logger.ErrorType.Message, "Global", "Application_AuthenticateRequest", "", "authCookie is null");
                FormsAuthentication.SignOut();
                return;
            }

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket.Expired)
            {
                Logger.LogEntry(Logger.ErrorType.Message, "Global", "Application_AuthenticateRequest", "", "authCookie expired for user" + authTicket.Name);
                try
                {
                    Context.User = null;
                    Context.Request.Cookies.Clear();
                    FormsAuthentication.SignOut();
                    Logger.LogEntry(Logger.ErrorType.Message, "Global", "Application_AuthenticateRequest", "", "User auto signout ");
                }
                catch (CryptographicException cex)
                {
                    FormsAuthentication.SignOut();
                    Logger.LogEntry(Logger.ErrorType.Message, "Global", "Application_AuthenticateRequest", "", "Invalid cookie during Cryptographic Operation");
                }
                catch (Exception ex)
                {
                    FormsAuthentication.SignOut();
                    Logger.LogEntry(Logger.ErrorType.Message, "Global", "Application_AuthenticateRequest", "", "Invalid cookie during Cryptographic Operation");
                    return;
                }
            }
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[AuthCookieName];

            if (authCookie == null || authCookie.Value == "")
            {
                return;
            }

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var identity = new GenericIdentity(authTicket.Name, "Forms");
            var principal = new MyPrincipalModel(identity);
            if (Context != null)
            {
                Logger.LogEntry(Logger.ErrorType.Message, "Global", "Application_PostAuthenticateRequest", "", "Serializing user Data for :" + authTicket.Name);
                var serializer = new JavaScriptSerializer();
                principal.User = (User)serializer.Deserialize(authTicket.UserData, typeof(User));
                Context.User = principal;
            }
            else
            {
                Logger.LogEntry(Logger.ErrorType.Error, "Global", "Application_PostAuthenticateRequest", "", "Context user is null" + authTicket != null && authTicket.Name != null ? " for user " + authTicket.Name : "");
            }

        }
    }
}
