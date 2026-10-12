using InterviewScheduler.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewScheduler.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string InterviewType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Panel { get; set; }
        public int Status { get; set; }
        public string StatusTitle { get; set; }
        public string AllowMultipleSchedule { get; set; }
        public Guid Technology { get; set; }
        public Guid Position { get; set; }
        public Guid InterviewRound { get; set; }
        public string InterviewRoundName { get; set; }
        public string TechnologyTitle { get; set; }
        public string PositionTitle { get; set; }
        public DateTime InterviewSlot1Date { get; set; }
        public DateTime InterviewSlot2Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public string TaskDescription { get; set; }
        public string AssignedToPanel { get; set; }
        public Guid CandidateId { get; set; }
        public int InterviewerRequired { get; set; }
        public string InterviewSlot1StartTime { get; set; }
        public string InterviewSlot1EndTime { get; set; }
        public string InterviewSlot2StartTime { get; set; }
        public string InterviewSlot2EndTime { get; set; }
        public string WorkflowDefinition { get; set; }
        public string WorkflowVersion { get; set; }
        public string WorkflowName { get; set; }
        public int CurrentStateID { get; set; }
        public string Comments { get; set; }
        public string SkillsRequired { get; set; }
        public string TaskNumber { get; set; }
        public TaskWorkFlowModel TaskWorkFlowModel { get; set; }
        public CandidateProfileModel CandidateProfileModel { get; set; }
        public TaskActionsModel TaskActionsModel { get; set; }
        public ActionHistoryModel ActionHistoryModel { get; set; }
        public List<InterviewRoundModel> InterviewRoundModel { get; set; }
        public List<WorkFlowStateModel> Action { get; set; }
        public List<SkillModel> Skills { get; set; }
        public List<PositionModel> PositionModel { get; set; }
        public List<TechnologyModel> TechnologyModel { get; set; }
        public List<AllocationPanelModel> PanelForAllocation { get; set; }
        public List<SuggestedInterviewSlotsModel> SuggestedInterviewSlotsModel { get; set; }
        public bool AllowEdit { get; set; } = true;
        public bool AllowAction { get; set; } = true;
        public int WorkflowStatus { get; set; }
        public List<TaskHistoryModel> TaskHistoryModel { get; set; }
        public string LoginUserName { get; internal set; }
        public string DraftID { get; internal set; }
    }
}
