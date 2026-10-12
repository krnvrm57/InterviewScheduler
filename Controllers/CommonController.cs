using InterviewScheduler.ADServices;
using InterviewScheduler.Common;
using InterviewScheduler.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewScheduler.Controllers
{
    public class CommonController : Controller
    {
        CrimsonDBHandle db = new CrimsonDBHandle();
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetEmployees(string Query)
        {

            List<EmployeeModel> lstEmployees = new List<EmployeeModel>();
            try
            {
                AD _ad = new AD();
                List<EmployeeModel> emp = _ad.GetByName(Query);
                var data = from lst in emp select new {  EmailId = lst.EmailId, FullName = lst.FullName, ThumbnailPhoto = lst.ThumbnailPhoto };
                return this.Content(JsonConvert.SerializeObject(data), "application/json");
            }
            catch (Exception)
            {
                return this.Content(JsonConvert.SerializeObject(lstEmployees), "application/json");
            }
        }

        public ActionResult GetAllPanel ()
        {

            List<EmployeeModel> lstEmployees = new List<EmployeeModel>();
            try
            {
                AD _ad = new AD();
                List<PanelModel> emp = db.GetAllPanels() ;
                var data = from lst in emp select new { Id = lst.Id, Name = lst.PanelName, Imageurl = lst.PanelName };
                return this.Content(JsonConvert.SerializeObject(data), "application/json");
            }
            catch (Exception)
            {
                return this.Content(JsonConvert.SerializeObject(lstEmployees), "application/json");
            }
        }

        public ActionResult GetSkills(string Query)
       {
            List<SkillModel> lstEmployees = new List<SkillModel>();
            try
            {
                AD _ad = new AD();
                List<SkillModel> SkillsData = db.GetSkills(Query);
                var data = (from lst in SkillsData select new { Id = lst.Id, Name = lst.SKill, Icon = lst.SkillIcon }).ToList();
                return this.Content(JsonConvert.SerializeObject(data), "application/json");
            }
            catch (Exception)
            {
                return this.Content(JsonConvert.SerializeObject(lstEmployees), "application/json");
            }
        }

        [HttpGet]
        public FileResult DownloadDocument(Guid DocumentId)
        {
            var DocInfo = db.GetDocumentById(DocumentId);
            var fileName = DocInfo.Title;
            return File(DocInfo.Document, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}