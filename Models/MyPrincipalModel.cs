using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace InterviewScheduler.Models
{
    public class User
    {
        [Display(Name = "Employee Code")]
        [Required(ErrorMessage = "Required!", AllowEmptyStrings = false)]
        public Guid Id { get; set; }
        [Display(Name = "Employee Name")]
        [Required(ErrorMessage = "Required!", AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Required!", AllowEmptyStrings = false)]
        public string EmailAddress { get; set; }
        public List<string> Groups { get; set; }
        public string loginSession { get; set; }
        public string ThumbnailPhoto { get; set; }
        public string Manager { get; set; }
        public string Designation { get; set; }
        public string NameInit { get; set; }
        public string EmployeeCode { get; set; }
        public string ApplicationAccess { get; set; }
        public string CurrentApplication { get; set; }

    }
    public class MyPrincipalModel : IPrincipal
    {
        public MyPrincipalModel(IIdentity identity)
        {
            Identity = identity;
        }
        public IIdentity Identity
        {
            get;
            private set;
        }
        public User User { get; set; }
        public bool IsInRole(string role)
        {
            if (User.Groups != null && User.Groups.Contains(role))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}