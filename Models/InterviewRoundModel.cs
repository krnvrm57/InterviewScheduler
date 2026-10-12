using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewScheduler.Models
{
    public class InterviewRoundModel
    {
        public Guid Id { get; set; }
        public string InterviewRoundName { get; set; }
        public string Order { get; internal set; }
        public bool IsActive { get; internal set; }
    }
}