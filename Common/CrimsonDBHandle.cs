using InterviewScheduler.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewScheduler.Common
{
    public class CrimsonDBHandle
    {
        private SqlConnection con;

        #region Connection
        private void GetConnectionString()
        {
            try
            {
                string constring = ConfigurationManager.ConnectionStrings["ConnStrOnboardingDB"].ToString();
                con = new SqlConnection(constring);
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "GetConnectionString", ex.Message, ex.StackTrace);
                throw;
            }
        }

        #endregion


        #region MasterData
        public List<TechnologyModel> GetTechnology()
        {
            List<TechnologyModel> lstmodel = new List<TechnologyModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetTechnology", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstmodel.Add(new TechnologyModel
                        {
                            Id = Guid.Parse(dr["Id"].ToString()),
                            Technology = dr["Technology"].ToString(),
                            TechnologyIcon = dr["TechnologyIcon"].ToString(),
                            TechnologyDescription = dr["TechnologyDescription"].ToString(),
                            isActive = Convert.ToBoolean(dr["isActive"].ToString()),
                            CreatedBy = dr["CreatedBy"].ToString(),
                            CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]),
                            LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                            LastUpdatedDate = dr["LastUpdatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["LastUpdatedDate"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "Technology", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;
        }
        public EmployeeModel GetEmployeeInfo(string userName)
        {
            EmployeeModel model = new EmployeeModel();

            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("sp_GetEmployeeInfo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmailID", Utility.EscapeInput(userName));
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Open();
                    sd.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        model.FullName = dt.Rows[0]["FullName"].ToString();
                        model.Id = Guid.Parse(dt.Rows[0]["Id"].ToString());
                        model.IsDeleted = Convert.ToBoolean(dt.Rows[0]["IsDeleted"].ToString());
                        model.Password = dt.Rows[0]["Password"].ToString();
                        model.ThumbnailPhoto = dt.Rows[0]["ThumbnailPhoto"].ToString();
                        model.PrimaryContactNumber = dt.Rows[0]["PrimaryContactNumber"].ToString();
                        model.SecondaryContactNumber = dt.Rows[0]["SecondaryContactNumber"].ToString();
                        model.EmployeeCode = dt.Rows[0]["EmployeeCode"].ToString();
                        model.EmailId = dt.Rows[0]["EmailId"].ToString();
                        model.Designation = dt.Rows[0]["Designation"].ToString();
                        model.Role = Utility.ConvertJsonToListRoles(dt.Rows[0]["Role"].ToString());
                        model.AllRoles = dt.Rows[0]["AllRoles"].ToString().Split(',').ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "GetDetails", ex.Message, ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return model;
        }

        internal Guid CreateDraft(object draftID, string draftJsonData, string title, Guid id)
        {
            throw new NotImplementedException();
        }

        public bool AddorUpdatePanel(Guid panelId, string panelName, Guid technology, bool status, Guid createdby, DataTable dtEmployees)
        {
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_AddOrUpdatePanel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@panelId", panelId);
                    cmd.Parameters.AddWithValue("@panelName", panelName);
                    cmd.Parameters.AddWithValue("@technology", technology);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@createdby", createdby);
                    cmd.Parameters.AddWithValue("@dtEmployees", dtEmployees);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();

                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "AddDetails", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public EmployeeModel GetEmployeeInfoByUSerName(string userName)
        {
            EmployeeModel model = new EmployeeModel();

            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("sp_GetEmployeeInfoByUSerName", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmailID", userName);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Open();
                    sd.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        model.FullName = dt.Rows[0]["FullName"].ToString();
                        model.Id = Guid.Parse(dt.Rows[0]["Id"].ToString());
                        model.IsDeleted = Convert.ToBoolean(dt.Rows[0]["IsDeleted"].ToString());
                        model.Password = dt.Rows[0]["Password"].ToString();
                        model.ThumbnailPhoto = dt.Rows[0]["ThumbnailPhoto"].ToString();
                        model.PrimaryContactNumber = dt.Rows[0]["PrimaryContactNumber"].ToString();
                        model.SecondaryContactNumber = dt.Rows[0]["SecondaryContactNumber"].ToString();
                        model.EmployeeCode = dt.Rows[0]["EmployeeCode"].ToString();
                        model.EmailId = dt.Rows[0]["EmailId"].ToString();
                        model.Designation = dt.Rows[0]["Designation"].ToString();
                        model.Role = Utility.ConvertJsonToListRoles(dt.Rows[0]["Role"].ToString());
                        model.AllRoles = dt.Rows[0]["AllRoles"].ToString().Split(',').ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "GetDetails", ex.Message, ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return model;
        }
        public bool AddUpdateEmployeeInfo(EmployeeModel response)
        {
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_AddOrUpdateEmployeeDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FullName", response.FullName);
                    cmd.Parameters.AddWithValue("@EmailID", response.EmailId);
                    cmd.Parameters.AddWithValue("@Designation", response.Title);
                    cmd.Parameters.AddWithValue("@ThumbnailPhoto", response.ThumbnailPhoto);
                    cmd.Parameters.AddWithValue("@Role", response.Role[0]);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();

                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "AddDetails", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public List<TaskHistoryModel> GetTaskHistory(Guid ReferenceKey, Guid EmployeeId)
        {
            List<TaskHistoryModel> lstmodel = new List<TaskHistoryModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetTaskHistory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReferenceKey", ReferenceKey);
                    cmd.Parameters.AddWithValue("@EmployeeId", EmployeeId);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstmodel.Add(new TaskHistoryModel
                        {
                            ActionName = dr["ActionName"].ToString(),
                            ActionBy = dr["ActionBy"].ToString(),
                            ActionDate = dr["ActionDate"].ToString(),
                            comments = dr["comments"].ToString(),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "Technology", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;
        }

        internal List<PanelModel> GetAllPanels()
        {

            List<PanelModel> lstmodel = new List<PanelModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetPanelDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstmodel.Add(new PanelModel
                        {
                            Id = Guid.Parse(dr["Id"].ToString()),
                            PanelName = dr["GroupName"].ToString(),
                            PanelDescription = dr["GroupDescription"].ToString(),
                            PanelIcon = dr["GroupIcon"].ToString(),
                            PanelEmployeeCount = dr["EmployeesInPannel"].ToString(),
                            TechnologyId = Guid.Parse(dr["TechnologyId"].ToString()),
                            TechnologyName = dr["Technology"].ToString(),
                            IsActive = Convert.ToBoolean(dr["isActive"].ToString()),
                            CreatedBy = dr["CreatedBy"].ToString(),
                            CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "Technology", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;

        }
        internal List<PanelModel> GetPanelDetailsById(Guid Id)
        {

            List<PanelModel> lstmodel = new List<PanelModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetPanelDetailsById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstmodel.Add(new PanelModel
                        {
                            Id = Guid.Parse(dr["Id"].ToString()),
                            PanelName = dr["GroupName"].ToString(),
                            PanelDescription = dr["GroupDescription"].ToString(),
                            PanelIcon = dr["GroupIcon"].ToString(),
                            PanelEmployeeCount = dr["EmployeesInPannel"].ToString(),
                            TechnologyId = Guid.Parse(dr["TechnologyId"].ToString()),
                            TechnologyName = dr["Technology"].ToString(),
                            IsActive = Convert.ToBoolean(dr["isActive"].ToString()),
                            CreatedBy = dr["CreatedBy"].ToString(),
                            CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]),
                            PannelEmployees = Utility.MapEmployeeForPanel(dr["EmployeeDetails"].ToString())
                        }); ;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "Technology", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;

        }
        public List<WorkFlowStateModel> GetWorkflowStateAndRolesById(int StateId)
        {
            List<WorkFlowStateModel> lstDetails = new List<WorkFlowStateModel>();

            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetWorkflowStateAndRolesById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StateId", StateId);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Open();
                    sd.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                        lstDetails.Add(
                                new WorkFlowStateModel
                                {
                                    Roles = Utility.ConvertJsonToListRoles(Convert.ToString(dr["Roles"])),
                                    StartDescription_EN = Convert.ToString(dr["StartDescription_EN"]),
                                    EndDescription_EN = Convert.ToString(dr["EndDescription_EN"]),
                                    StateId = Convert.ToInt32(dr["StateId"]),
                                    Icons = Convert.ToString(dr["Icons"]),
                                });
                }
            }
            catch (Exception ex)
            {
                //  Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "GetDetails", ex.Message, ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstDetails;
        }
        public List<SuggestedInterviewSlotsModel> GetSuggestedInterviewSlotsForTask(Guid TaskId, Guid InterviewRound)
        {
            List<SuggestedInterviewSlotsModel> lstmodel = new List<SuggestedInterviewSlotsModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetSuggestedInterviewSlotsForTask", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TaskId", TaskId);
                    cmd.Parameters.AddWithValue("@InterviewRound", InterviewRound);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        DateTime? InterviewDate = null;
                        if (dr["InterviewSlotDate"].ToString() != "")
                        {
                            InterviewDate = Convert.ToDateTime(dr["InterviewSlotDate"]);
                        }



                        lstmodel.Add(new SuggestedInterviewSlotsModel
                        {
                            Id = Guid.Parse(dr["Id"].ToString()),
                            TaskId = Guid.Parse(dr["TaskId"].ToString()),
                            InterviewSlotDate = InterviewDate,
                            InterviewSlotStartTime = dr["InterviewSlotStartTime"].ToString(),
                            InterviewSlotEndTime = dr["InterviewSlotEndTime"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            EmailId = dr["EmailId"].ToString(),
                            AllocatedToEmployee = Guid.Parse(dr["AllocatedToEmployee"].ToString()),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "Technology", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;
        }
        public TaskModel GetInterviewShedulerTaskDetailsById(Guid Id)
        {
            List<TaskModel> model = new List<TaskModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetInterviewShedulerTaskDetailsById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        model.Add(new TaskModel
                        {
                            Id = Guid.Parse(dr["Id"].ToString()),
                            Title = dr["Title"].ToString(),
                            TaskNumber = dr["TaskNumber"].ToString(),
                            TaskDescription = dr["TaskDescription"].ToString(),
                            AssignedToPanel = dr["AssignedToPanel"].ToString(),
                            CandidateId = Guid.Parse(dr["CandidateId"].ToString()),
                            Position = Guid.Parse(dr["Position"].ToString()),
                            Technology = Guid.Parse(dr["Technology"].ToString()),
                            AllowMultipleSchedule = dr["AllowMultipleSchedule"].ToString(),
                            InterviewSlot1Date = dr["InterviewSlot1Date"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["InterviewSlot1Date"]),
                            InterviewSlot1StartTime = dr["InterviewSlot1StartTime"].ToString(),
                            InterviewSlot1EndTime = dr["InterviewSlot1EndTime"].ToString(),
                            InterviewSlot2Date = dr["InterviewSlot2Date"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["InterviewSlot2Date"]),
                            InterviewSlot2StartTime = dr["InterviewSlot2StartTime"].ToString(),
                            InterviewSlot2EndTime = dr["InterviewSlot2EndTime"].ToString(),
                            InterviewerRequired = Convert.ToInt32(dr["InterviewerRequired"].ToString()),
                            Status = Convert.ToInt32(dr["Status"].ToString()),
                            WorkflowStatus = Convert.ToInt32(dr["WorkflowStatus"].ToString()),
                            PositionTitle = dr["PositionTitle"].ToString(),
                            TechnologyTitle = dr["TechnologyTitle"].ToString(),
                            StatusTitle = dr["StatusTitle"].ToString(),
                            IsActive = Convert.ToBoolean(dr["IsActive"].ToString()),
                            CreatedBy = Guid.Parse(dr["CreatedBy"].ToString()),
                            CreatedByName = dr["CreatedByName"].ToString(),
                            CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]),
                            LastUpdatedBy = Guid.Parse(dr["LastUpdatedBy"].ToString()),
                            LastUpdatedDate = dr["LastUpdatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["LastUpdatedDate"]),
                            WorkflowDefinition = Convert.ToString(dr["WorkflowDefinition"]),
                            WorkflowVersion = Convert.ToString(dr["WorkflowVersion"]),
                            WorkflowName = Convert.ToString(dr["WorkflowName"]),
                            InterviewRound = Guid.Parse(dr["InterviewRound"].ToString()),
                            InterviewRoundName = Convert.ToString(dr["InterviewRoundName"]),
                            CurrentStateID = Convert.ToInt32(Convert.ToString(dr["WorkflowStatus"])),
                            SkillsRequired = Convert.ToString(dr["SkillsRequired"]),
                            Action = Utility.GetActionFromWorkFlow(Mapper.MapWorkflowStatesToWorkflowStateModel(Convert.ToString(dr["WorkflowDefinition"])).Workflow, Convert.ToString(dr["WorkflowStatus"])),
                            Skills = Utility.MapSelectedSkills(dr["SkillsRequired"].ToString())

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "GetInterviewShedulerTaskDetailsById", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return model.FirstOrDefault();
        }
        internal bool UpdateTaskResponsee(Guid Id, int TaskAction, DateTime? SuggestedSlotDate1, string SuggestedStartTime1, string SuggestedEndTime1, DateTime? SuggestedSlotDate2, string SuggestedStartTime2, string SuggestedEndTime2, Guid PannelId, Guid createdBy, string Comments, Guid InterviewRound, int InterviewerRequired)
        {
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_UpdateTaskResponse", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@TaskAction", TaskAction);
                    cmd.Parameters.AddWithValue("@SuggestedSlotDate1", SuggestedSlotDate1);
                    cmd.Parameters.AddWithValue("@SuggestedStartTime1", SuggestedStartTime1);
                    cmd.Parameters.AddWithValue("@SuggestedEndTime1", SuggestedEndTime1);
                    cmd.Parameters.AddWithValue("@SuggestedSlotDate2", SuggestedSlotDate2);
                    cmd.Parameters.AddWithValue("@SuggestedStartTime2", SuggestedStartTime2);
                    cmd.Parameters.AddWithValue("@SuggestedEndTime2", SuggestedEndTime2);
                    cmd.Parameters.AddWithValue("@InterviewRound", InterviewRound);
                    cmd.Parameters.AddWithValue("@InterviewerRequired", InterviewerRequired);
                    cmd.Parameters.AddWithValue("@Comments", Comments);
                    cmd.Parameters.AddWithValue("@PannelId", PannelId);
                    cmd.Parameters.AddWithValue("@createdBy", createdBy);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "UpdateTaskResponsee", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public CandidateProfileModel GetCandidateProfileForSheduledInterview(Guid Id)
        {
            List<CandidateProfileModel> model = new List<CandidateProfileModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetCandidateProfileForSheduledInterview", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        model.Add(new CandidateProfileModel
                        {
                            Id = Guid.Parse(dr["CandidateId"].ToString()),
                            CandidateName = dr["CandidateName"].ToString(),
                            CandidateGender = dr["CandidateGender"].ToString(),
                            PhoneNumber = dr["PhoneNumber"].ToString(),
                            EmailId = dr["EmailId"].ToString(),
                            DocumentId = Guid.Parse(dr["DocumentId"].ToString()),
                            DocTitle = dr["Title"].ToString(),
                            Document = (byte[])(dr["Document"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "GetInterviewShedulerTaskDetailsById", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return model.FirstOrDefault();
        }



        public DocumentModel GetDocumentById(Guid Id)
        {
            List<DocumentModel> model = new List<DocumentModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetDocumentById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        model.Add(new DocumentModel
                        {
                            Id = Guid.Parse(dr["DocumentId"].ToString()),
                            Title = dr["Title"].ToString(),
                            DocumentType = dr["DocumentType"].ToString(),
                            DocumentTypeId = Guid.Parse(dr["DocumentTypeId"].ToString()),
                            IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString()),
                            Document = (byte[])(dr["Document"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "GetInterviewShedulerTaskDetailsById", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return model.FirstOrDefault();
        }

        public List<InterviewRoundModel> GetInterviewRound()
        {
            List<InterviewRoundModel> model = new List<InterviewRoundModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetInterviewRound", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        model.Add(new InterviewRoundModel
                        {
                            Id = Guid.Parse(dr["Id"].ToString()),
                            InterviewRoundName = dr["InterviewRoundName"].ToString(),
                            Order = dr["Order"].ToString(),
                            IsActive = Convert.ToBoolean(dr["IsActive"].ToString()),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "GetInterviewRound", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return model;
        }
        public TaskModel GetInterviwerDetailsById(Guid Id)
        {
            List<TaskModel> model = new List<TaskModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetInterviwerDetailsById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        model.Add(new TaskModel
                        {
                            Id = Guid.Parse(dr["Id"].ToString()),
                            Title = dr["Title"].ToString(),
                            TaskDescription = dr["TaskDescription"].ToString(),
                            AssignedToPanel = dr["AssignedToPanel"].ToString(),
                            CandidateId = Guid.Parse(dr["CandidateId"].ToString()),
                            Position = Guid.Parse(dr["Position"].ToString()),
                            Technology = Guid.Parse(dr["Technology"].ToString()),
                            AllowMultipleSchedule = dr["AllowMultipleSchedule"].ToString(),
                            InterviewSlot1Date = dr["InterviewSlot1Date"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["InterviewSlot1Date"]),
                            InterviewSlot1StartTime = dr["InterviewSlot1StartTime"].ToString(),
                            InterviewSlot1EndTime = dr["InterviewSlot1EndTime"].ToString(),
                            InterviewSlot2Date = dr["InterviewSlot2Date"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["InterviewSlot2Date"]),
                            InterviewSlot2StartTime = dr["InterviewSlot2StartTime"].ToString(),
                            InterviewSlot2EndTime = dr["InterviewSlot2EndTime"].ToString(),
                            InterviewerRequired = Convert.ToInt32(dr["InterviewerRequired"].ToString()),
                            Status = Convert.ToInt32(dr["Status"].ToString()),
                            PositionTitle = dr["PositionTitle"].ToString(),
                            TechnologyTitle = dr["TechnologyTitle"].ToString(),
                            InterviewRound = Guid.Parse(dr["InterviewRound"].ToString()),
                            InterviewRoundName = dr["InterviewRoundName"].ToString(),
                            StatusTitle = dr["TaskStatus"].ToString(),
                            IsActive = Convert.ToBoolean(dr["IsActive"].ToString()),
                            CreatedBy = Guid.Parse(dr["CreatedBy"].ToString()),
                            CreatedByName = dr["CreatedByName"].ToString(),
                            CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]),
                            LastUpdatedBy = Guid.Parse(dr["LastUpdatedBy"].ToString()),
                            LastUpdatedDate = dr["LastUpdatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["LastUpdatedDate"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "GetInterviewShedulerTaskDetailsById", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return model.FirstOrDefault();
        }
        public Guid CreateDraft(Guid DraftID, string DraftJsonData, string Title, Guid CreatedBy)
        {
            Guid DraftId = Guid.Empty;
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("AddDraftData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DraftID", DraftID);
                    cmd.Parameters.AddWithValue("@DraftTitle", Title);
                    cmd.Parameters.AddWithValue("@DraftData", DraftJsonData);
                    cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DraftId = Guid.Parse(dr["DraftId"].ToString());
                    }
                }
                return DraftId;
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "AddDraftData", ex.Message, ex.StackTrace);
                throw ex;

            }
        }
        public bool CreateTask(CreateTaskModel model)
        {
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("sp_CreateTask", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Title", Utility.EscapeInput(model.Title));
                    cmd.Parameters.AddWithValue("@TaskDescription", Utility.EscapeInput(model.TaskDescription));
                    cmd.Parameters.AddWithValue("@Technology", model.Technology);
                    cmd.Parameters.AddWithValue("@Position", model.Position);
                    cmd.Parameters.AddWithValue("@SkillsRequired", model.SkillsRequired);
                    cmd.Parameters.AddWithValue("@AllowMultipleSchedule", Utility.EscapeInput(model.AllowMultipleSchedule == "undefined" ? null : model.AllowMultipleSchedule));
                    cmd.Parameters.AddWithValue("@InterviewSlot1Date", model.InterviewSlot1Date?.ToString());
                    cmd.Parameters.AddWithValue("@InterviewSlot1StartTime", Utility.EscapeInput(model.InterviewSlot1StartTime));
                    cmd.Parameters.AddWithValue("@InterviewSlot1EndTime", Utility.EscapeInput(model.InterviewSlot1EndTime));
                    cmd.Parameters.AddWithValue("@InterviewSlot2Date", model.InterviewSlot2Date?.ToString());
                    cmd.Parameters.AddWithValue("@InterviewSlot2StartTime", Utility.EscapeInput(model.InterviewSlot2StartTime));
                    cmd.Parameters.AddWithValue("@InterviewSlot2EndTime", Utility.EscapeInput(model.InterviewSlot2EndTime));
                    cmd.Parameters.AddWithValue("@CandidateName", Utility.EscapeInput(model.CandidateName));
                    cmd.Parameters.AddWithValue("@CandidateGender", Utility.EscapeInput(model.CandidateGender));
                    cmd.Parameters.AddWithValue("@AssignedToPanel", Utility.EscapeInput(model.AssignedToPanel));
                    cmd.Parameters.AddWithValue("@InterviewerRequired", Utility.EscapeInput(model.InterviewerRequired));
                    cmd.Parameters.AddWithValue("@EmailId", Utility.EscapeInput(model.EmailId));
                    cmd.Parameters.AddWithValue("@PhoneNumber", Utility.EscapeInput(model.PhoneNumber));
                    cmd.Parameters.AddWithValue("@DocumentTitle", model.DocumentTitle);
                    cmd.Parameters.AddWithValue("@Document", model.Document);
                    cmd.Parameters.AddWithValue("@DocumentTypeId", model.DocumentTypeId);
                    cmd.Parameters.AddWithValue("@DocumentType", model.DocumentType);
                    cmd.Parameters.AddWithValue("@InterviewRound", model.InterviewRound);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "AddDetails", ex.Message, ex.StackTrace);
                throw ex;

            }
        }
        public List<SkillModel> GetSkills(string RequestString)
        {
            List<SkillModel> lstmodel = new List<SkillModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetSkills", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestString", RequestString);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstmodel.Add(new SkillModel
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            SKill = dr["SKill"].ToString(),
                            SkillIcon = dr["SkillIcon"].ToString(),
                            SkillDescription = dr["SkillDescription"].ToString(),
                            isActive = Convert.ToBoolean(dr["isActive"].ToString()),
                            CreatedBy = dr["CreatedBy"].ToString(),
                            CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]),
                            LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                            LastUpdatedDate = dr["LastUpdatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["LastUpdatedDate"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "Position", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;
        }

        public List<PositionModel> GetPosition()
        {
            List<PositionModel> lstmodel = new List<PositionModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetPosition", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstmodel.Add(new PositionModel
                        {
                            Id = Guid.Parse(dr["Id"].ToString()),
                            Position = dr["Position"].ToString(),
                            PositionIcon = dr["PositionIcon"].ToString(),
                            PositionDescription = dr["PositionDescription"].ToString(),
                            isActive = Convert.ToBoolean(dr["isActive"].ToString()),
                            CreatedBy = dr["CreatedBy"].ToString(),
                            CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]),
                            LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                            LastUpdatedDate = dr["LastUpdatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["LastUpdatedDate"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "Position", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;
        }
        public List<AllocationPanelModel> GetPanelForAllocation()
        {
            List<AllocationPanelModel> lstmodel = new List<AllocationPanelModel>();
            try
            {
                GetConnectionString();
                using (SqlCommand cmd = new SqlCommand("SP_GetPanelForAllocation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    con.Open();
                    sd.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstmodel.Add(new AllocationPanelModel
                        {
                            Id = Guid.Parse(dr["Id"].ToString()),
                            GroupName = dr["GroupName"].ToString(),
                            GroupIcon = dr["GroupIcon"].ToString(),
                            GroupDescription = dr["GroupDescription"].ToString(),
                            UsersInGroup = Convert.ToInt32(dr["UsersInGroup"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "Position", ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;
        }

        public List<TaskModel> GetScheduleInterviewForEmployee(Guid Id)
        {
            List<TaskModel> lstmodel = new List<TaskModel>();
            try
            {
                GetConnectionString();
                SqlCommand cmd = new SqlCommand("SP_GetScheduleInterviewForEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                con.Open();
                sd.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lstmodel.Add(new TaskModel
                    {
                        Id = Guid.Parse(dr["Id"].ToString()),
                        Title = dr["Title"].ToString(),
                        TaskNumber = dr["TaskNumber"].ToString(),
                        TaskDescription = dr["TaskDescription"].ToString(),
                        AssignedToPanel = dr["AssignedToPanel"].ToString(),
                        CandidateId = Guid.Parse(dr["CandidateId"].ToString()),
                        Position = Guid.Parse(dr["Position"].ToString()),
                        Technology = Guid.Parse(dr["Technology"].ToString()),
                        InterviewerRequired = Convert.ToInt32(dr["InterviewerRequired"].ToString()),
                        InterviewSlot1Date = dr["InterviewSlot1Date"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["InterviewSlot1Date"]),
                        InterviewSlot1StartTime = dr["InterviewSlot1StartTime"].ToString(),
                        InterviewSlot1EndTime = dr["InterviewSlot1EndTime"].ToString(),
                        Status = Convert.ToInt32(dr["Status"].ToString()),
                        PositionTitle = dr["PositionTitle"].ToString(),
                        TechnologyTitle = dr["TechnologyTitle"].ToString(),
                        StatusTitle = dr["StatusTitle"].ToString(),
                        InterviewRound = Guid.Parse(dr["InterviewRound"].ToString()),
                        InterviewRoundName = Convert.ToString(dr["InterviewRoundName"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"].ToString()),
                        CreatedBy = Guid.Parse(dr["CreatedBy"].ToString()),
                        CreatedByName = dr["CreatedByName"].ToString(),
                        CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]),
                        LastUpdatedBy = Guid.Parse(dr["LastUpdatedBy"].ToString()),
                        LastUpdatedDate = dr["LastUpdatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["LastUpdatedDate"]),
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "InterviewShedulerTask", ex.Message, ex.StackTrace);
                //throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;
        }

        public List<TaskModel> GetInterviewShedulerTask(Guid Id)
        {
            List<TaskModel> lstmodel = new List<TaskModel>();
            try
            {
                GetConnectionString();
                SqlCommand cmd = new SqlCommand("SP_GetInterviewShedulerTask", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                con.Open();
                sd.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lstmodel.Add(new TaskModel
                    {
                        Id = Guid.Parse(dr["Id"].ToString()),
                        TaskNumber = dr["TaskNumber"].ToString(),
                        Title = dr["Title"].ToString(),
                        TaskDescription = dr["TaskDescription"].ToString(),
                        AssignedToPanel = dr["AssignedToPanel"].ToString(),
                        CandidateId = Guid.Parse(dr["CandidateId"].ToString()),
                        Position = Guid.Parse(dr["Position"].ToString()),
                        Technology = Guid.Parse(dr["Technology"].ToString()),
                        InterviewerRequired = Convert.ToInt32(dr["InterviewerRequired"].ToString()),
                        Status = Convert.ToInt32(dr["Status"].ToString()),
                        PositionTitle = dr["PositionTitle"].ToString(),
                        TechnologyTitle = dr["TechnologyTitle"].ToString(),
                        StatusTitle = dr["StatusTitle"].ToString(),
                        SkillsRequired = dr["SkillsRequired"].ToString(),
                        InterviewRound = Guid.Parse(dr["InterviewRound"].ToString()),
                        InterviewRoundName = Convert.ToString(dr["InterviewRoundName"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"].ToString()),
                        CreatedBy = Guid.Parse(dr["CreatedBy"].ToString()),
                        CreatedByName = dr["CreatedByName"].ToString(),
                        CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]),
                        LastUpdatedBy = Guid.Parse(dr["LastUpdatedBy"].ToString()),
                        LastUpdatedDate = dr["LastUpdatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["LastUpdatedDate"]),
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "InterviewShedulerTask", ex.Message, ex.StackTrace);
                //throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;
        }

        public List<TaskModel> GetInterviewShedulerForPanelTask(Guid EmployeeId)
        {
            List<TaskModel> lstmodel = new List<TaskModel>();
            try
            {
                GetConnectionString();
                SqlCommand cmd = new SqlCommand("SP_GetInterviewShedulerForPanelTask", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", EmployeeId);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                con.Open();
                sd.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lstmodel.Add(new TaskModel
                    {
                        Id = Guid.Parse(dr["Id"].ToString()),
                        Title = dr["Title"].ToString(),
                        TaskNumber = dr["TaskNumber"].ToString(),
                        TaskDescription = dr["TaskDescription"].ToString(),
                        AssignedToPanel = dr["AssignedToPanel"].ToString(),
                        CandidateId = Guid.Parse(dr["CandidateId"].ToString()),
                        Position = Guid.Parse(dr["Position"].ToString()),
                        Technology = Guid.Parse(dr["Technology"].ToString()),
                        InterviewerRequired = Convert.ToInt32(dr["InterviewerRequired"].ToString()),
                        Status = Convert.ToInt32(dr["Status"].ToString()),
                        PositionTitle = dr["PositionTitle"].ToString(),
                        TechnologyTitle = dr["TechnologyTitle"].ToString(),
                        StatusTitle = dr["StatusTitle"].ToString(),
                        InterviewRound = Guid.Parse(dr["InterviewRound"].ToString()),
                        InterviewRoundName = Convert.ToString(dr["InterviewRoundName"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"].ToString()),
                        CreatedBy = Guid.Parse(dr["CreatedBy"].ToString()),
                        CreatedByName = dr["CreatedByName"].ToString(),
                        CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]),
                        LastUpdatedBy = Guid.Parse(dr["LastUpdatedBy"].ToString()),
                        LastUpdatedDate = dr["LastUpdatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["LastUpdatedDate"]),
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "InterviewShedulerTask", ex.Message, ex.StackTrace);
                //throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return lstmodel;
        }

        #endregion


        //public List<EmployeeDocumentsModel> GetEmployeeDocuments(Guid EmployeeId)
        //{

        //    List<EmployeeDocumentsModel> lstmodel = new List<EmployeeDocumentsModel>();
        //    try
        //    {
        //        GetConnectionString();
        //        using (SqlCommand cmd = new SqlCommand("SP_GetEmployeeDocumentsByEmployeeId", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@EmployeeId", EmployeeId);
        //            SqlDataAdapter sd = new SqlDataAdapter(cmd);
        //            DataSet ds = new DataSet();
        //            con.Open();
        //            sd.Fill(ds);

        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                lstmodel.Add(new EmployeeDocumentsModel
        //                {
        //                    Id = Guid.Parse(dr["Id"].ToString()),
        //                    EmployeeId = Guid.Parse(dr["EmployeeId"].ToString()),
        //                    Title = dr["Title"].ToString(),
        //                    DocumentTypeName = dr["DocumentTypeName"].ToString(),
        //                    DocumentTypeId = Guid.Parse(dr["DocumentTypeId"].ToString()),
        //                    Document = (byte[])(dr["Document"]),
        //                    DocumentType = dr["DocumentType"].ToString(),
        //                    IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString()),
        //                    CreatedDate = dr["CreatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]),
        //                    CreatedBy = Guid.Parse(dr["CreatedBy"].ToString()),
        //                    LastUpdatedDate = dr["LastUpdatedDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(dr["LastUpdatedDate"]),
        //                    LastUpdatedBy = Guid.Parse(dr["LastUpdatedBy"].ToString()),
        //                    canbeUpdated = true,
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandler", "GetEmployeeDocuments", ex.Message, ex.StackTrace);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Close();
        //        }
        //    }
        //    return lstmodel;
        //}

        //public bool AddEmployeeDocuments(EmployeeDocumentsModel model)
        //{
        //    try
        //    {
        //        GetConnectionString();
        //        using (SqlCommand cmd = new SqlCommand("SP_UploadDocument", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
        //            cmd.Parameters.AddWithValue("@Title", model.Title);
        //            cmd.Parameters.AddWithValue("@DocumentTypeId", model.DocumentTypeId);
        //            cmd.Parameters.AddWithValue("@Document", model.Document);
        //            cmd.Parameters.AddWithValue("@DocumentType", model.DocumentType);
        //            cmd.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);
        //            cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);

        //            con.Open();
        //            int i = cmd.ExecuteNonQuery();
        //            con.Close();
        //            if (i >= 1)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "AddEmployeeDocuments", ex.Message, ex.StackTrace);
        //        throw ex;
        //    }
        //}

        //public bool UpdateVideoPlayFlag(Guid Id, Guid UserId, bool IsCompleted = false)
        //{
        //    try
        //    {
        //        GetConnectionString();
        //        using (SqlCommand cmd = new SqlCommand("SP_UpdateVideoStatus", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@Id", Id);
        //            cmd.Parameters.AddWithValue("@IsCompleted", IsCompleted);
        //            cmd.Parameters.AddWithValue("@UserId", UserId);
        //            con.Open();
        //            int i = cmd.ExecuteNonQuery();
        //            con.Close();
        //            if (i >= 1)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogEntry(Logger.ErrorType.Error, "CrimsonDBHandle", "AddEmployeeDocuments", ex.Message, ex.StackTrace);
        //        throw ex;
        //    }
        //}
    }
}