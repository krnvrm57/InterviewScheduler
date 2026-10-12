using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewScheduler.Common
{
    public class Constant
    {


    }
    public static class TokenStatus
    {
        public static string Valid = "Valid";
        public static string Expired = "Expired";
        public static string Invalid = "Invalid";
    }
    public static class AjaxStatus
    {
        public const string Success = "Success";
        public const string Failure = "Failure";
    }

    public static class AuthStatus
    {
        public const string Success = "SUCCESS";
        public const string Failure = "FAILURE";
    }
    public static class SessionName
    {
        public const string EmployeeDocuments = "EmployeeDocuments_";
        public const string Employee = "Employee_";
    }
    public static class Roles
    {
        public const string OnboardingEmployee = "NewEmployee";
        public const string HR = "HR";
    }
    public static class Message
    {
        public const string Task_Creataion_Failure = "Error occured while creating the task !";
        public const string Task_Draft_Failure = "Error occured while saving the Draft !";
        public const string Task_Creataion_Validation_Failure = "All the fields are required !";
        public const string FirstName_Required = "First Name is required !";
        public const string LastName_Required = "Last Name is required !";
        public const string EmailId_Required = "EmailId is required !";
        public const string DOJ_Required = "DOJ is required !";
        public const string Manager_Required = "Manager Name is required !";
        public const string Gender_Required = "Gender is required !";
        public const string EMPTYP_Required = "Employment type is required !";
        public const string TRACK_Change = "One of the Video from previous step is not played, auto switching video to play for that track. ";
        public const string Video_StatusUpdate_Error = "Error while updating video status.";
        public const string Emp_Code_Update_success = "Employee code updated successfully.";
        public const string Emp_Code_Update_Failure = "Error while updating employee code.";
        public const string Exception = "Error occured while processing your request. Please try again and  in case issue persist contact administrator.";
        public const string LoginError = "Incorrect username or password, Please enter correct username or password.";
        public const string Add_Project_Error = "Error occured while adding project details";
        public const string Password_Update_Success = "Password successfully Updated !";
        public const string Password_Update_Failure = "Error occured while updating password";
        public const string Update_Employee_Comments_Failure = "Error occured while updating Details.";
        public const string Panel_Creataion_Failure = "Error occured while creating panel.";   
        public const string Panel_Group_Employee_Required = "Please select at least one person for group.";
        public const string Panel_Update_Failure = "Error occured while updating panel details.";
        public const string AD_Service_Empty_Response = "Empty data response from Active Directory.";
    }

}