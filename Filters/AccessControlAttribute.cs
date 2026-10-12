using InterviewScheduler.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InterviewScheduler.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessControlAttribute : AuthorizeAttribute
    {
        private new readonly string[] Roles = null;
        public AccessControlAttribute()
        {
        }

        public AccessControlAttribute(params string[] Roles)
        {
            this.Roles = Roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            try
            {
                return HasAccessRights(httpContext);
            }
            catch (Exception)
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            if (filterContext.Result.GetType().FullName == "System.Web.Mvc.HttpUnauthorizedResult")
            {
                filterContext.Result = new RedirectResult("~/Error/401");
            }
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == null)
            {
                if (!filterContext.HttpContext.Request.IsAuthenticated)
                {
                    filterContext.Result = new RedirectResult("~/Landing");
                }
            }
            else if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (!filterContext.HttpContext.Request.IsAuthenticated)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {


                        filterContext.Result = new JsonResult
                        {
                            Data = new
                            {
                                Status = 401,
                                Message = "Authentication Expired."
                            },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        FormsAuthentication.RedirectToLoginPage();//Redirection login page
                    }
                }


            }
            else
            {
                filterContext.Result = new RedirectResult("~/Error/401");
            }
        }
        private bool HasAccessRights(HttpContextBase httpContext)
        {
            var _user = ((MyPrincipalModel)System.Web.HttpContext.Current.User).User;
            var currentContext = httpContext.Request.CurrentExecutionFilePath;
            if (Roles.Length > 0)
            {
                foreach (var role in Roles)
                {
                    if (_user.Groups.Contains(role))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}