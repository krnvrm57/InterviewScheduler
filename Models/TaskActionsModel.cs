using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewScheduler.Models
{
    public class TaskActionsModel
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public DateTime AvailableDateOption1 { get; set; }
        public DateTime AvailableDateOptiion2 { get; set; }
        public string Comments { get; set; }
        public List<SuggestedInterviewSlotsModel> SuggestedInterviewSlotsModel { get; set; }
    }
}
