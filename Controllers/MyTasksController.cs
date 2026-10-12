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
    public class MyTasksController : Controller
    {
        CrimsonDBHandle db = new CrimsonDBHandle();
        private Models.User _user = new Models.User();

        [AccessControl("Employee", "SDCHR")]
        // GET: MyTaks
        public ActionResult Index()
        {
            return View();
        }
        [AccessControl("Employee", "SDCHR")]
        public ActionResult ViewTask(Guid Id)
        {
            TaskModel model = new TaskModel();
            try
            {
                model = db.GetInterviewShedulerTaskDetailsById(Id);
                model.CandidateProfileModel = db.GetCandidateProfileForSheduledInterview(Id);
                model.TechnologyModel = db.GetTechnology();
                model.PositionModel = db.GetPosition();
                model.PanelForAllocation = db.GetPanelForAllocation();
                model.InterviewRoundModel = db.GetInterviewRound();
                model.TaskHistoryModel = db.GetTaskHistory(Id, Guid.Empty);
                model.AllowEdit = false;
                model.AllowAction = false;
                model.SuggestedInterviewSlotsModel = db.GetSuggestedInterviewSlotsForTask(Id, model.InterviewRound);

            }
            catch (Exception ex)
            {
            }
            return PartialView(model);
        }
        [AccessControl("Employee", "SDCHR")]
        public ActionResult EditTask(Guid Id)
        {
            TaskModel model = new TaskModel();
            try
            {
                model = db.GetInterviewShedulerTaskDetailsById(Id);
                model.CandidateProfileModel = db.GetCandidateProfileForSheduledInterview(Id);
                model.TechnologyModel = db.GetTechnology();
                model.PositionModel = db.GetPosition();
                model.PanelForAllocation = db.GetPanelForAllocation();
                model.InterviewRoundModel = db.GetInterviewRound();
                model.AllowEdit = true;
                model.TaskActionsModel = new TaskActionsModel();
                model.SuggestedInterviewSlotsModel = db.GetSuggestedInterviewSlotsForTask(Id, model.InterviewRound);

                if (model.AllowEdit)
                {

                }

            }
            catch (Exception ex)
            {
            }
            return PartialView("ViewTask", model);
        }

        [AccessControl("Employee", "SDCHR")]
        [HttpPost]
        public ActionResult GetAllTasks()
        {
            List<TaskModel> lstOnboardingTasks = new List<TaskModel>();
            string draw = Request.Form["draw"][0].ToString();

            try
            {
                _user = ((MyPrincipalModel)System.Web.HttpContext.Current.User).User;

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
                int TotalCount = 0;
                lstOnboardingTasks = db.GetInterviewShedulerForPanelTask(_user.Id);
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
                Logger.LogEntry(Logger.ErrorType.Error, "Task", "GetAllTasks", ex.StackTrace, ex.Message);
                return this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = lstOnboardingTasks
                });
            }
        }
        [AccessControl("Employee", "SDCHR")]
        [HttpPost]
        public ActionResult GetTaskResponse(Guid Id, int TaskAction, DateTime? SuggestedSlotDate1, string SuggestedStartTime1, string SuggestedEndTime1, DateTime? SuggestedSlotDate2, string SuggestedStartTime2, string SuggestedEndTime2, string Comments)
        {
            _user = ((MyPrincipalModel)System.Web.HttpContext.Current.User).User;

            TaskModel model = db.GetInterviewShedulerTaskDetailsById(Id);

            if (db.UpdateTaskResponsee(Id, TaskAction, SuggestedSlotDate1, SuggestedStartTime1, SuggestedEndTime1, SuggestedSlotDate2, SuggestedStartTime2, SuggestedEndTime2, Guid.Parse(model.AssignedToPanel), _user.Id, Comments,Guid.Empty,0))
            {
                return Json(new { status = AjaxStatus.Success, Message = string.Empty }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = AjaxStatus.Failure, Message = string.Empty }, JsonRequestBehavior.AllowGet);
        }
    }
}