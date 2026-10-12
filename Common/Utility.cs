using InterviewScheduler.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace InterviewScheduler.Common
{
    public static class Utility
    {
        public static string EncodeToBase64(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode " + ex.Message);
            }
        }
        public static string DecodeFromBase64(string data)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(data);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        public static byte[] ConvertToBinaryFile(HttpPostedFileBase file)
        {
            int fileSizeInBytes = file.ContentLength;
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] data = target.ToArray();
            return data;
        }

        public static List<string> ConvertJsonToListRoles(string jsonRoles)
        {
            List<string> stringList = new List<string>();
            if (!string.IsNullOrEmpty(jsonRoles))
            {
                JObject jobject = JObject.Parse(jsonRoles);
                if (jobject["Roles"] != null)
                {
                    foreach (JToken jtoken in jobject["Roles"])
                    {
                        string str = (string)jtoken;
                        stringList.Add(str);
                    }
                }
            }
            else
            {
                stringList = null;
            }

            return stringList;
        }
        public static string EscapeInput(string Value)
        {
            return System.Security.SecurityElement.Escape(Value);
        }
        public static List<EmployeeModel> MapEmployeeForPanel(string data)
        {
            List<EmployeeModel> lst = new List<EmployeeModel>();

            XDocument doc = XDocument.Parse(data);

            var list = doc.Root.Elements("EmployeeInfo").ToList();

            foreach (var item in list)
            {
                lst.Add(new EmployeeModel { FullName = item.Attribute("FullName").Value, EmailId = item.Attribute("EmailId").Value, PrimaryContactNumber = item.Attribute("PrimaryContactNumber").Value, ThumbnailPhoto = item.Attribute("ThumbnailPhoto").Value });
            }

            return lst;
        }
        public static List<WorkFlowStateModel> GetActionFromWorkFlow(List<Workflow> Workflow, string WorkFlow_State)
        {
            CrimsonDBHandle db = new CrimsonDBHandle();
            var workflowStates = db.GetWorkflowStateAndRolesById(-1);

            var ACtionData = Workflow.Where(x => x.StateID.ToString() == WorkFlow_State).Select(x => x.Path);

            return (from w in workflowStates
                    join ac in ACtionData.FirstOrDefault().AsQueryable() on w.StateId.ToString() equals ac.PathId
                    select new WorkFlowStateModel
                    {
                        StateId = w.StateId,
                        StartDescription_EN = w.StartDescription_EN,
                        EndDescription_EN = w.EndDescription_EN,
                        Roles = w.Roles,
                        Icons = w.Icons
                    }).ToList();

        }

        internal static List<SkillModel> MapSelectedSkills(string selectedSkills)
        {
            CrimsonDBHandle db = new CrimsonDBHandle();
            List<SkillModel> skills = new List<SkillModel>();
            List<SkillModel> lstSkills = db.GetSkills(null);
            List<string> lstselectedSkills = selectedSkills.Split(',').ToList();
            foreach (var item in lstselectedSkills)
            {
               var _item= lstSkills.Where(x => x.Id.ToString() == item).FirstOrDefault();
                if (_item != null && item.Length!=0)
                {
                    skills.Add(_item);
                }
            }

            return skills;
        }
    }
}