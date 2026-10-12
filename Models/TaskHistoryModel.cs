using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewScheduler.Models
{
    public class TaskHistoryModel
    {
        public string ActionName { get; set; }
        public string ActionBy { get; set; }
        public string ActionDate { get; set; }
        public string comments { get; set; }
    }
}