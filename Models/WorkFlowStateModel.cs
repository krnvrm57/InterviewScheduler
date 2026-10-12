using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewScheduler.Models
{
    public class WorkFlowStateModel
    {
        public int StateId { get; set; }
        public string StartDescription_EN { get; set; }
        public string EndDescription_EN { get; set; }
        public List<string> Roles { get; set; }
        public string Icons { get; set; }
    }
}