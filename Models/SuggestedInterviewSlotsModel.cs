using System;

namespace InterviewScheduler.Models
{
    public class SuggestedInterviewSlotsModel
    {

        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public DateTime? InterviewSlotDate { get; set; }
        public string InterviewSlotStartTime { get; set; }
        public string InterviewSlotEndTime { get; set; }
        public string FullName { get;  set; }
        public string EmailId { get;  set; }
        public Guid AllocatedToEmployee { get; set; }
    }
}