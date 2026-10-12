using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewScheduler.Models
{
    public class WorkFlowModel
    {
        public string WorkflowDefination { get; set; }
        public string WorkFlowName { get; set; }
        public string Version { get; set; }
        public int StartStateID { get; set; }
        public List<Workflow> Workflow { get; set; }

    }
    public class Workflow
    {
        public int StateID { get; set; }
        public List<Path> Path { get; set; }
        public bool Iscomplete { get; set; }
    }
    public class Path
    {
        public string PathId { get; set; }
    }
}