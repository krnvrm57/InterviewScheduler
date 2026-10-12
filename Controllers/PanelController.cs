using InterviewScheduler.ADServices;
using InterviewScheduler.Common;
using InterviewScheduler.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace InterviewScheduler.Controllers
{
    public class PanelController : Controller
    {
        CrimsonDBHandle db = new CrimsonDBHandle();
        private Models.User _user = new Models.User();

        // GET: Panel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreatePanel()
        {
            PanelModel model = new PanelModel();
            model.TechnologyModel = db.GetTechnology();
            return PartialView(model);
        }

        public ActionResult ViewPanel(Guid Id)
        {
            PanelModel model = new PanelModel();
            model = db.GetPanelDetailsById(Id).FirstOrDefault();
            model.TechnologyModel = db.GetTechnology();
            model.AllowEdit = false;
            return PartialView(model);
        }

        public ActionResult EditPanel(Guid Id)
        {
            PanelModel model = new PanelModel();
            model = db.GetPanelDetailsById(Id).FirstOrDefault();
            model.TechnologyModel = db.GetTechnology();
            model.AllowEdit = true;
            return PartialView("ViewPanel", model);
        }

        public ActionResult ManagePanel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetAllPanel()
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

            List<PanelModel> lstPanel = new List<PanelModel>();
            try
            {
                int TotalCount = 0;
                lstPanel = db.GetAllPanels();
                return this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = TotalCount,
                    recordsFiltered = TotalCount,
                    data = lstPanel
                });
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "Task", "GetAllTasks", ex.StackTrace, ex.Message);
                return this.Content(JsonConvert.SerializeObject(lstPanel), "application/json");
            }
        }
        [HttpPost]
        public ActionResult CreatePanel(Guid panelId, string PanelName, Guid Technology, bool Status, string Employeesname)
        {
            AD _ad = new AD();
            List<EmployeeModel> emps = new List<EmployeeModel>();
            Guid Createdby = Guid.Empty;
            try
            {
                _user = ((MyPrincipalModel)System.Web.HttpContext.Current.User).User;
                Createdby = _user.Id;
                string[] Employees = Employeesname.Split(',');
                if (Employees != null && Employees.Length > 0)
                {
                    emps = _ad.GetAll();
                    if (emps != null && emps.Count > 0)
                    {
                        DataTable dtEmployees = new DataTable();
                        dtEmployees.Columns.Add("FullName", typeof(System.String));
                        dtEmployees.Columns.Add("EmailId", typeof(System.String));
                        dtEmployees.Columns.Add("ProfileImage", typeof(System.String));
                        dtEmployees.Columns.Add("Role", typeof(System.String));
                        dtEmployees.Columns.Add("Title", typeof(System.String));
                        dtEmployees.Columns.Add("PrimaryContactNumber", typeof(System.String));
                        dtEmployees.Columns.Add("DOJ", typeof(System.DateTime));
                        foreach (var item in Employees)
                        {
                            var selectedEmployeeDetails = emps.Where(x => x.EmailId.ToLower() == item.ToLower()).FirstOrDefault();
                            dtEmployees.Rows.Add(selectedEmployeeDetails.FullName, selectedEmployeeDetails.EmailId, selectedEmployeeDetails.ThumbnailPhoto, selectedEmployeeDetails.Role, selectedEmployeeDetails.Title, selectedEmployeeDetails.PrimaryContactNumber.Replace("+91", "").Trim(), selectedEmployeeDetails.DOJ);
                        }

                        if (db.AddorUpdatePanel(panelId, PanelName, Technology, Status, Createdby, dtEmployees))
                        {
                            return Json(new { Status = AjaxStatus.Success, Message = string.Empty }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            if (panelId == Guid.Empty)
                            {
                                return Json(new { Status = AjaxStatus.Failure, Message = Message.Panel_Creataion_Failure }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { Status = AjaxStatus.Failure, Message = Message.Panel_Update_Failure }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        Logger.LogEntry(Logger.ErrorType.Error, "Panel", "CreatePanel", "", Message.AD_Service_Empty_Response);
                        return Json(new { Status = AjaxStatus.Failure, Message = Message.Exception }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Logger.LogEntry(Logger.ErrorType.Error, "Panel", "CreatePanel", "", Message.Panel_Group_Employee_Required);
                    return Json(new { Status = AjaxStatus.Failure, Message = Message.Panel_Group_Employee_Required }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "Panel", "CreatePanel", "", ex.Message);
                return Json(new { Status = AjaxStatus.Failure, Message = Message.Task_Creataion_Failure }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}