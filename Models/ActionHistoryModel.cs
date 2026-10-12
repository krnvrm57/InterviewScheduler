using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewScheduler.Models
{
    public class ActionHistoryModel
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string ActionBy { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
