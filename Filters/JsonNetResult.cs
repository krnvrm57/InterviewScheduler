using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;

namespace InterviewScheduler.Filters
{
    internal class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            MaxJsonLength = Int32.MaxValue;
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (((InterviewScheduler.Filters.JsonResponse)Data).status.ToString() == "401")
            {
                response.Redirect("~/Landing");
            }
            string serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented);

            response.Write(serializedObject);
        }
    }
}