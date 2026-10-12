using System;

namespace InterviewScheduler.Models
{
    public class AllocationPanelModel
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public string GroupIcon { get; set; }
        public string GroupDescription { get; set; }
        public int UsersInGroup { get; set; }
    }
}