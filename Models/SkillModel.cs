using System;

namespace InterviewScheduler.Models
{
    public class SkillModel
    {
        public int Id { get; set; }
        public string SKill { get; set; }
        public string SkillIcon { get; set; }
        public string SkillDescription { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}