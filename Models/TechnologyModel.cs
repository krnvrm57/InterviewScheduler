using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewScheduler.Models
{
    public class TechnologyModel
    {
        public Guid Id { get; set; }
        public string Technology { get; set; }
        public string TechnologyIcon { get; set; }
        public string TechnologyDescription { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
