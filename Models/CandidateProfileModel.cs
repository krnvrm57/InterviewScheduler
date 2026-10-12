using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewScheduler.Models
{
    public class CandidateProfileModel
    {
        public Guid Id { get; set; }
        public string CandidateName { get; set; }
        public string CandidateGender { get; set; } 
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public Guid DocumentId { get; set; }
        public string DocTitle { get; set; }
        public byte[] Document { get; set; }
    }
}
