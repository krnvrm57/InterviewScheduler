using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewScheduler.Models
{
    public class PanelModel
    {
        public Guid Id { get; set; }
        public string PanelName { get; set; }
        public string TechnologyName { get; set; }
        public bool IsActive { get; set; }
        public string PanelEmployeeCount { get; set; }
        public List<EmployeeModel> PannelEmployees { get; set; } = new List<EmployeeModel>();
        public string PannelEmployeeIds { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string PanelDescription { get; internal set; }
        public string PanelIcon { get; internal set; }
        public List<TechnologyModel> TechnologyModel { get; set; }
        public Guid TechnologyId { get; set; }
        public bool AllowEdit { get; set; }

    }
}