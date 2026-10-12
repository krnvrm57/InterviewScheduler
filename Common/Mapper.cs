using InterviewScheduler.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewScheduler.Common
{
    public class Mapper
    {
        public static WorkFlowModel MapWorkflowStatesToWorkflowStateModel(string _workflow)
        {
            WorkFlowModel wkfModel = new WorkFlowModel();
            var data = JObject.Parse(_workflow);
            var WorkFlowData = data["Workflow"].Select(x => x);
            List<Workflow> Wkf = new List<Workflow>();
            string StateId = "";
            bool IsComplete = false;
            List<Path> _lstpath = new List<Path>();
            foreach (var item in WorkFlowData)
            {
                StateId = item["StateID"].ToString();
                if (item["Path"] != null)
                {
                    foreach (var _pathitem in item["Path"])
                    {
                        _lstpath.Add(new Path { PathId = _pathitem.ToString() });
                    }
                }
                IsComplete = item["IsComplete"] == null ? false : Convert.ToBoolean(item["IsComplete"].ToString());
                Wkf.Add(new Workflow { StateID = Convert.ToInt32(StateId), Iscomplete = IsComplete, Path = _lstpath });
                _lstpath = new List<Path>();
            }
            wkfModel.Workflow = Wkf;
            wkfModel.WorkFlowName = data["WorkflowName"].ToString();
            wkfModel.Version = data["Version"].ToString();
            wkfModel.StartStateID = Convert.ToInt32(data["StartStateID"].ToString());
            return wkfModel;
        }
    }
}