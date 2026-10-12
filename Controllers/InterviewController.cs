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
    public class InterviewController : Controller
    {
        CrimsonDBHandle db = new CrimsonDBHandle();
        private Models.User _user = new Models.User();

        [AccessControl("SDCHR")]
        public ActionResult Index()
        {
            Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "GetConnectionString", "", "");

            return View();
        }
        [AccessControl("SDCHR")]
        public ActionResult CreateTask()
        {
            _user = ((MyPrincipalModel)System.Web.HttpContext.Current.User).User;

            TaskModel model = new TaskModel();
            model.TechnologyModel = db.GetTechnology();
            model.PositionModel = db.GetPosition();
            model.PanelForAllocation = db.GetPanelForAllocation();
            model.InterviewRoundModel = db.GetInterviewRound();
            model.LoginUserName = _user.Name;
            return PartialView(model);
        }
        [AccessControl("SDCHR")]
        [HttpPost]
        public ActionResult SubmitTask(Guid Id, int TaskAction, string ScheduleOn, string Comments, Guid InterviewRound, Guid AssignedToPanel, int InterviewerRequired)
        {
            _user = ((MyPrincipalModel)System.Web.HttpContext.Current.User).User;
            TaskModel model = db.GetInterviewShedulerTaskDetailsById(Id);
            DateTime? SuggestedSlotDate1 = null;
            string SuggestedStartTime1 = "";
            string SuggestedEndTime1 = "";
            if (ScheduleOn != "")
            {
                var data = ScheduleOn.Split('_');
                SuggestedSlotDate1 = Convert.ToDateTime(data[0]);
                SuggestedStartTime1 = Convert.ToString(data[1]);
                SuggestedEndTime1 = Convert.ToString(data[2]);
            }
            if (TaskAction != 200)
            {
                AssignedToPanel = Guid.Parse(model.AssignedToPanel);
            }

            if (db.UpdateTaskResponsee(Id, TaskAction, SuggestedSlotDate1, SuggestedStartTime1, SuggestedEndTime1, null, null, null, AssignedToPanel, _user.Id, Comments, InterviewRound, InterviewerRequired))
            {
                return Json(new { status = AjaxStatus.Success, Message = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = AjaxStatus.Failure, Message = Message.Task_Creataion_Failure }, JsonRequestBehavior.AllowGet);
            }
        }
        [AccessControl("SDCHR")]
        [HttpPost]
        public ActionResult CreateTask(CreateTaskModel model)
        {
            bool rtnvalue = false;
            _user = ((MyPrincipalModel)System.Web.HttpContext.Current.User).User;

            var ProfileCount = Request.Form.AllKeys.Where(x => x.Contains("CandidateName_")).ToList().Count();

            Dictionary<string, HttpPostedFileBase> Profiledetails = new Dictionary<string, HttpPostedFileBase>();

            for (int i = 0; i < ProfileCount; i++)
            {
                Profiledetails.Add(Request.Form.GetValues("CandidateName_" + i).FirstOrDefault(), Request.Files.Get(i));
            }

            foreach (var item in Profiledetails)
            {
                model.CandidateName = item.Key;
                model.Document = Utility.ConvertToBinaryFile(item.Value);
                model.DocumentTitle = item.Value.FileName;
                model.DocumentType = item.Value.ContentType;
                model.DocumentTypeId = Guid.Parse("08ED475E-435B-40E4-966D-F3C4A793ED58");

                model.CreatedBy = _user.Id;

                if (db.CreateTask(model))
                {
                    rtnvalue = true;
                }
                else
                {
                    rtnvalue = false;
                }
            }

            if (rtnvalue)
            {
                return Json(new { status = AjaxStatus.Success, Message = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = AjaxStatus.Failure, Message = Message.Task_Creataion_Failure }, JsonRequestBehavior.AllowGet);

            }
        }


        [AccessControl("SDCHR")]
        [HttpPost]
        public ActionResult SaveDraft(CreateTaskModel model)
        {
            Guid DraftId = Guid.Empty;
            _user = ((MyPrincipalModel)System.Web.HttpContext.Current.User).User;
            var DraftJsonData = JsonConvert.SerializeObject(model);
            DraftId = db.CreateDraft(model.DraftID, DraftJsonData, model.Title, _user.Id);
            if (DraftId != Guid.Empty)
            {
                return Json(new { DraftId = DraftId, Message = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { DraftId = Guid.Empty, Message = Message.Task_Draft_Failure }, JsonRequestBehavior.AllowGet);
            }
        }

        [AccessControl("SDCHR")]
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
                model.SuggestedInterviewSlotsModel = db.GetSuggestedInterviewSlotsForTask(Id, Guid.Empty);
                model.AllowEdit = false;
                model.AllowAction = false;
            }
            catch (Exception ex)
            {
            }
            return PartialView(model);
        }
        [AccessControl("SDCHR")]
        public ActionResult ActionOnTask(Guid Id)
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
                model.SuggestedInterviewSlotsModel = db.GetSuggestedInterviewSlotsForTask(Id, model.InterviewRound);
                model.AllowEdit = false;

                if (model.WorkflowStatus == 500)
                {
                    model.AllowEdit = false;
                }


            }
            catch (Exception ex)
            {
            }
            return PartialView("ViewTask", model);
        }

        [AccessControl("SDCHR")]
        public ActionResult TransferTask(Guid Id)
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
                model.SuggestedInterviewSlotsModel = db.GetSuggestedInterviewSlotsForTask(Id, model.InterviewRound);
                model.AllowEdit = true;

                if (model.WorkflowStatus == 500)
                {
                    model.AllowEdit = false;
                }
                model.AllowAction = true;
            }
            catch (Exception ex)
            {
            }
            return PartialView(model);
        }
        [AccessControl("SDCHR")]
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
                model.TaskHistoryModel = db.GetTaskHistory(Id, Guid.Empty);
                model.SuggestedInterviewSlotsModel = db.GetSuggestedInterviewSlotsForTask(Id, model.InterviewRound);
                model.AllowEdit = true;

                if (model.WorkflowStatus == 500)
                {
                    model.AllowEdit = false;
                }
                model.AllowAction = true;
            }
            catch (Exception ex)
            {
            }
            return PartialView("ViewTask", model);
        }
        [AccessControl("SDCHR")]
        [HttpPost]
        public ActionResult GetAllTasks()
        {
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
                lstOnboardingTasks = db.GetInterviewShedulerTask(Guid.Empty);
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
                return this.Content(JsonConvert.SerializeObject(lstOnboardingTasks), "application/json");
            }
        }

    }
}