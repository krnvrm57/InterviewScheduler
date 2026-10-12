using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewScheduler.Models
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string ThumbnailPhoto { get; set; }
        public string ManagerName { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public string StreetAddress { get; set; }
        public string Status { get; set; }
        public string ErrorDetails { get; set; }
        public List<string> Role { get; set; }
        public string AccessToken { get; set; }
        public string Designation { get; internal set; }
        public bool IsDeleted { get; internal set; }
        public string Password { get; internal set; }
        public string PrimaryContactNumber { get; internal set; }
        public string SecondaryContactNumber { get; internal set; }
        public string EmployeeCode { get; internal set; }
        public List<string> AllRoles { get; internal set; }
        public DateTime? DOJ { get; internal set; }
    }
}