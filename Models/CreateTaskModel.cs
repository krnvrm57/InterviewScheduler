using System;
using System.Web;

namespace InterviewScheduler.Models
{
    public class CreateTaskModel
    {
        public string Title { get; set; }
        public Guid Technology { get; set; }
        public Guid Position { get; set; }
        public string TaskDescription { get; set; }
        public string AllowMultipleSchedule { get; set; }
        public DateTime? InterviewSlot1Date { get; set; }
        public string InterviewSlot1StartTime { get; set; }
        public string InterviewSlot1EndTime { get; set; }
        public DateTime? InterviewSlot2Date { get; set; }
        public string InterviewSlot2StartTime { get; set; }
        public string InterviewSlot2EndTime { get; set; }
        public string CandidateName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string CandidateGender { get; set; }
        public string AssignedToPanel { get; set; }
        public string SkillsRequired { get; set; }
        public string InterviewerRequired { get; set; }
        public Guid CreatedBy { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }
        public string DocumentTitle { get; set; }
        public byte[] Document { get; set; }
        public Guid DocumentTypeId { get; set; }
        public Guid InterviewRound { get; set; }
        public string DocumentType { get; set; }
        public bool IsDraft { get; set; }
        public Guid DraftID { get; set; } = Guid.Empty;
    }
}