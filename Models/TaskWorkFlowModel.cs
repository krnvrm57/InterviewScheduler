using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewScheduler.Models
{
    public class TaskWorkFlowModel
    {
        public Guid Id { get; set; }
        public string WorkflowName { get; set; }
        public string Version { get; set; }
        public string Actions { get; set; }
        public string Slots { get; set; }
        public string SlotsPending { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
