using InterviewScheduler.Common;
using InterviewScheduler.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Web.Mvc;

namespace InterviewScheduler.ADServices
{
    public class AD
    {
        string LDAPUrl = ConfigurationSettings.AppSettings["LDAPUrl"].ToString();

        public List<EmployeeModel> GetAll()
        {
            List<EmployeeModel> employeeModel = new List<EmployeeModel>();
            try
            {
                string username = ConfigurationSettings.AppSettings["LDAPUserName"].ToString();
                string key = ConfigurationSettings.AppSettings["LDAPEncriptionKey"].ToString();
                using (var directoryEntry = new DirectoryEntry(LDAPUrl + "/OU=CrimsonLogic Users,DC=snshub,DC=org", username, key))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(directoryEntry))
                    {
                        var queryFormat = "((|(C={0})))";
                        searcher.Filter = string.Format(queryFormat, "IN");
                        foreach (SearchResult result in searcher.FindAll())
                        {
                            if (result.Properties["mail"].Count > 0)
                            {
                                DateTime? _doj = null;
                                if (result.Properties["whencreated"].Count > 0)
                                {
                                    Convert.ToDateTime(result.Properties["whencreated"][0]);
                                }
                                employeeModel.Add(new EmployeeModel
                                {
                                    UserName = result.Properties["samaccountname"].Count > 0 ? result.Properties["samaccountname"][0].ToString() : string.Empty,
                                    FullName = result.Properties["name"].Count > 0 ? result.Properties["name"][0].ToString() : string.Empty,
                                    ManagerName = result.Properties["manager"].Count > 0 ? result.Properties["manager"][0].ToString() : string.Empty,
                                    PrimaryContactNumber = result.Properties["telephonenumber"].Count > 0 ? result.Properties["telephonenumber"][0].ToString() : string.Empty,
                                    Title = result.Properties["title"].Count > 0 ? result.Properties["title"][0].ToString() : string.Empty,
                                    Department = result.Properties["department"].Count > 0 ? result.Properties["department"][0].ToString() : string.Empty,
                                    EmailId = result.Properties["mail"].Count > 0 ? result.Properties["mail"][0].ToString() : string.Empty,
                                    Location = result.Properties["l"].Count > 0 ? result.Properties["l"][0].ToString() : string.Empty,
                                    Country = result.Properties["C"].Count > 0 ? result.Properties["C"][0].ToString() : string.Empty,
                                    Role = new List<string> { "Employee" },
                                   // Company = result.Properties["company"].Count > 0 ? result.Properties["company"][0].ToString() : string.Empty,
                                    ThumbnailPhoto = result.Properties["thumbnailphoto"].Count > 0 ? Convert.ToBase64String((byte[])result.Properties["thumbnailphoto"][0]) : null,
                                    DOJ=_doj,
                                    StreetAddress = result.Properties["streetaddress"].Count > 0 ? result.Properties["streetaddress"][0].ToString() : string.Empty,
                                    Status = AjaxStatus.Success,
                                    ErrorDetails = string.Empty
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return employeeModel;
            }
            return employeeModel;
        }

        public List<EmployeeModel> GetByName(string name)
        {
            List<EmployeeModel> employeeModel = new List<EmployeeModel>();
            try
            {
                string username = ConfigurationSettings.AppSettings["LDAPUserName"].ToString();
                string key = ConfigurationSettings.AppSettings["LDAPEncriptionKey"].ToString();
                using (var directoryEntry = new DirectoryEntry(LDAPUrl + "/OU=CrimsonLogic Users,DC=snshub,DC=org", username, key))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(directoryEntry))
                    {
                        var queryFormat = "((&(name=*{0}*)(C=*{1}*)))";
                        searcher.Filter = string.Format(queryFormat,name,"IN");

                        foreach (SearchResult result in searcher.FindAll())
                        {
                            if (result.Properties["mail"].Count > 0)
                            {

                                DateTime? _doj = null;
                                if (result.Properties["whencreated"].Count > 0)
                                { 
                                    Convert.ToDateTime(result.Properties["whencreated"][0]);
                                }

                                employeeModel.Add(new EmployeeModel
                                {
                                    UserName = result.Properties["samaccountname"].Count > 0 ? result.Properties["samaccountname"][0].ToString() : string.Empty,
                                    FullName = result.Properties["name"].Count > 0 ? result.Properties["name"][0].ToString() : string.Empty,
                                    ManagerName = result.Properties["manager"].Count > 0 ? result.Properties["manager"][0].ToString() : string.Empty, 
                                    PrimaryContactNumber = result.Properties["telephonenumber"].Count > 0 ? result.Properties["telephonenumber"][0].ToString() : string.Empty,
                                    Title = result.Properties["title"].Count > 0 ? result.Properties["title"][0].ToString() : string.Empty,
                                    //Department = result.Properties["department"].Count > 0 ? result.Properties["department"][0].ToString() : string.Empty,
                                    EmailId = result.Properties["mail"].Count > 0 ? result.Properties["mail"][0].ToString() : string.Empty,
                                    //Location = result.Properties["l"].Count > 0 ? result.Properties["l"][0].ToString() : string.Empty,
                                    Country = result.Properties["C"].Count > 0 ? result.Properties["C"][0].ToString() : string.Empty,
                                    Role = new List<string> { "Employee" },
                                    // Company = result.Properties["company"].Count > 0 ? result.Properties["company"][0].ToString() : string.Empty,
                                    ThumbnailPhoto = result.Properties["thumbnailphoto"].Count > 0 ? Convert.ToBase64String((byte[])result.Properties["thumbnailphoto"][0]) : null,
                                    DOJ = _doj,
                                    //StreetAddress = result.Properties["streetaddress"].Count > 0 ? result.Properties["streetaddress"][0].ToString() : string.Empty,
                                    Status = AjaxStatus.Success,
                                    ErrorDetails = string.Empty
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return employeeModel;
            }
            return employeeModel;
        }


        [HttpPost]
        public EmployeeModel IsValidateCredentials(string username, string encryptedSecretKey)
        {
            string _decryptedSecretKey = Utility.DecodeFromBase64(encryptedSecretKey);
            EmployeeModel empModel = new EmployeeModel();
            try
            {
                using (var directoryEntry = new DirectoryEntry(LDAPUrl, username, _decryptedSecretKey))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(directoryEntry))
                    {
                        var queryFormat = "((|(SAMAccountName={0})))";
                        searcher.Filter = string.Format(queryFormat, username);
                        foreach (SearchResult item in searcher.FindAll())
                        {
                            DateTime? _doj = null;
                            if (item.Properties["whencreated"].Count > 0)
                            {
                                Convert.ToDateTime(item.Properties["whencreated"][0]);
                            }
                            empModel.UserName = item.Properties["samaccountname"].Count > 0 ? item.Properties["samaccountname"][0].ToString() : string.Empty;
                            empModel.FullName = item.Properties["name"].Count > 0 ? item.Properties["name"][0].ToString() : string.Empty;
                            empModel.ManagerName = item.Properties["manager"].Count > 0 ? item.Properties["manager"][0].ToString() : string.Empty;
                            empModel.Title = item.Properties["title"].Count > 0 ? item.Properties["title"][0].ToString() : string.Empty;
                            empModel.Department = item.Properties["department"].Count > 0 ? item.Properties["department"][0].ToString() : string.Empty;
                            empModel.EmailId = item.Properties["mail"].Count > 0 ? item.Properties["mail"][0].ToString() : string.Empty;
                            empModel.Location = item.Properties["l"].Count > 0 ? item.Properties["l"][0].ToString() : string.Empty;
                            empModel.Country = item.Properties["C"].Count > 0 ? item.Properties["C"][0].ToString() : string.Empty;
                            empModel.Role = new List<string> { "Employee" };
                            empModel.AccessToken = Utility.EncodeToBase64(Guid.NewGuid().ToString());
                            empModel.Company = item.Properties["company"].Count > 0 ? item.Properties["company"][0].ToString() : string.Empty;
                            empModel.StreetAddress = item.Properties["streetaddress"].Count > 0 ? item.Properties["streetaddress"][0].ToString() : string.Empty;
                            empModel.ThumbnailPhoto = item.Properties["thumbnailphoto"].Count > 0 ? Convert.ToBase64String((byte[])item.Properties["thumbnailphoto"][0]) : null;
                            empModel.DOJ = _doj;
                            empModel.Status = AjaxStatus.Success;
                            empModel.ErrorDetails = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                empModel.Status = AjaxStatus.Failure;
                empModel.ErrorDetails = ex.Message;
                return empModel;
            }
            return empModel;
        }

    }
}