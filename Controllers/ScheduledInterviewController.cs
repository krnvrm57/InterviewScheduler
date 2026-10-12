using InterviewScheduler.Common;
using InterviewScheduler.Filters;
using InterviewScheduler.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewScheduler.Controllers
{
    public class ScheduledInterviewController : Controller
    {
        CrimsonDBHandle db = new CrimsonDBHandle();
        private Models.User _user = new Models.User();

        [AccessControl("Employee", "SDCHR")]

        // GET: ScheduleInterview
        public ActionResult Index()
        {
            return View();
        }
        [AccessControl("Employee", "SDCHR")]
        [HttpPost]
        public ActionResult GetAllInterviews()
        {
            _user = ((MyPrincipalModel)System.Web.HttpContext.Current.User).User;
             
            string draw = Request.Form["draw"][0].ToString();
            int startRec = Convert.ToInt32(Request.Form["start"][0]);
            string mode = string.IsNullOrEmpty(Request.Form["mode"]) ? "" : Request.Form["mode"].ToString();
            string option = string.IsNullOrEmpty(Request.Form["option"]) ? "" : Request.Form["option"].ToString();
            string limit = string.IsNullOrEmpty(Request.Form["length"]) ? "" : Request.Form["length"].ToString();
            string start = string.IsNullOrEmpty(Request.Form["start"]) ? "" : Request.Form["start"].ToString();
            string UserId = string.IsNullOrEmpty(Request.Form["user"]) ? "" : Request.Form["user"].ToString();
            string searchKey = string.IsNullOrEmpty(Request.Form["search[value]"]) ? "" : Request.Form["search[value]"].ToString();
            string searchKey_Status = string.IsNullOrEmpty(Request.Form["search_Status"]) ? "0" : Request.Form["search_Status"].ToString();
            string orderByColumn = string.IsNullOrEmpty(Request.Form["order[0][column]"]) ? "" : Request.Form["order[0][column]"].ToString();
            string orderByDir = string.IsNullOrEmpty(Request.Form["order[0][dir]"]) ? "" : Request.Form["order[0][dir]"].ToString();
            int pageSize = Convert.ToInt32(Request.Form["length"][0]);
            List<TaskModel> lstOnboardingTasks = new List<TaskModel>();
            try
            {
                int TotalCount = 0;
                lstOnboardingTasks = db.GetScheduleInterviewForEmployee(_user.Id);
                return this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = TotalCount,
                    recordsFiltered = TotalCount,
                    data = lstOnboardingTasks
                });
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "ScheduleInterview", "GetAllInterviews", ex.StackTrace, ex.Message);
                return this.Content(JsonConvert.SerializeObject(lstOnboardingTasks), "application/json");
            }
        }
    }
}