using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TEPL.QDMS.WindowsService.Business;
using TEPL.QMS.Common;
using TEPL.QMS.Common.Constants;

namespace TEPLQDMSWS
{
    public class UserNotifications
    {
        public void SendDigestmailForPublichedDocuments()
        {
            WSAdminBLL objAdmin = new WSAdminBLL();
            DateTime startDate = DateTime.Now.AddDays(-7);
            DateTime endDate = DateTime.Now;
            DataTable dt = objAdmin.GetPublishedDocumentsForDigest(startDate, endDate);
            string[] selectedColumns = new[] { "DocumentType", "DepartmentName", "SectionName", "DocumentNo", "DocumentDescription", "RevisionReason", "Version", "PublishedOn" };
            DataTable dt1 = new DataView(dt).ToTable(false, selectedColumns);
            dt1.TableName = "PublishedDocuments";
            string strExcelPath = QMSConstants.TempFolder + "PublishedDocuments.xlsx";
            ExcelOperations.ExportDataSet(dt1, strExcelPath);
            string strMailSubject = $"Documents published from {startDate.ToString("dd-MMM-yyyy")} to {endDate.ToString("dd-MMM-yyyy")}";
            StringBuilder strMailBody = new StringBuilder();
            strMailBody.Append($"Attached documents are published from {startDate.ToString("dd-MMM-yyyy")} to {endDate.ToString("dd-MMM-yyyy")}");
            strMailBody.Append("<br/><br/>");
            strMailBody.Append("Link to see published documents: <a style='text-decoration:underline' target='_blank' href='" +
            ConfigurationManager.AppSettings["websiteURL"].ToString() + "'>" + "Click here" + "</a>");
            string toEmailid = "dms@tataelectronics.co.in";
            //toEmailid = "rajesh.m-ext@tataelectronics.co.in";
            MailSend.PrepareandSendMail("ApprovalMailTemplate", "User", toEmailid, "", "dms.support@tataelectronics.co.in", strMailSubject, strExcelPath, strMailBody.ToString());
        }
        public void DocumentRevalidation()
        {
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            try
            {
                //month = 6;
                //day = 1;
                if (month % 6 == 0)
                {
                    if (day == 1 || day == 5 || day == 7)
                    {
                        string[] commonColumns = { "DepartmentName", "SectionName", "DocumentLevel" };
                        WSAdminBLL objAdmin = new WSAdminBLL();
                        DateTime cutOffDate = new DateTime(DateTime.Now.AddMonths(-17).Year, DateTime.Now.AddMonths(-17).Month, 1).AddDays(-1);
                        DateTime FinalDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);
                        LoggerBlock.WriteLog("Before calling sharepoint");
                        DataTable dtReviewers = GetSharePointListItems();
                        LoggerBlock.WriteLog("after calling sharepoint");
                        DataTable dtDocuments = objAdmin.GetRevalidationDocuments(cutOffDate);
                        dtDocuments.Columns.Add("ApprovalUser");
                        foreach (DataRow dr in dtDocuments.Rows)
                        {
                            LoggerBlock.WriteLog("DepartmentName:" + dr["DepartmentName"].ToString());
                            LoggerBlock.WriteLog("SectionName:" + dr["SectionName"].ToString());
                            LoggerBlock.WriteLog("DocumentLevel:" + dr["DocumentLevel"].ToString());

                            dr["ApprovalUser"] = string.Join(";", dtReviewers.AsEnumerable()
                                .Where(row => row.Field<string>("DepartmentName").ToLower() == dr["DepartmentName"].ToString().ToLower() && row.Field<string>("SectionName").ToLower() == dr["SectionName"].ToString().ToLower() && row.Field<string>("DocumentLevel").ToLower() == dr["DocumentLevel"].ToString().ToLower())
            .Select(row => row.Field<string>("ApprovalUser")));
                            LoggerBlock.WriteLog("ApprovalUser" + dr["ApprovalUser"].ToString());
                        }
                        var grouped = from table in dtDocuments.AsEnumerable()
                                      group table by new { UploadedUserEmail = table["UploadedUserEmail"], UploadedUserName = table["UploadedUserName"] } into groupby
                                      select new
                                      {
                                          Value = groupby.Key,
                                          ColumnValues = groupby,
                                          ApprovalUser = string.Join(";", groupby.AsEnumerable().Select(row => row.Field<string>("ApprovalUser")))
                                      };
                        string strMailSubject = string.Empty;
                        string toEmailid = string.Empty;
                        string ccEmailId = string.Empty;
                        foreach (var key in grouped)
                        {
                            LoggerBlock.WriteLog(key.Value.UploadedUserEmail.ToString());
                            LoggerBlock.WriteLog("---------------------------");
                            toEmailid = key.Value.UploadedUserEmail.ToString();
                            dtDocuments = key.ColumnValues.CopyToDataTable();
                            string[] selectedColumns = new[] { "DocumentType", "DepartmentName", "SectionName", "DocumentNo", "DocumentDescription", "RevisionReason", "Version", "PublishedOn" };
                            DataTable dt1 = new DataView(dtDocuments).ToTable(false, selectedColumns);
                            dt1.TableName = "RevalidationDocuments";
                            string strExcelPath = QMSConstants.TempFolder + key.Value.UploadedUserName + ".xlsx";
                            ExcelOperations.ExportDataSet(dt1, strExcelPath);
                            LoggerBlock.WriteLog("Reviewer mail id" + key.ApprovalUser);

                            ccEmailId = GetEmailIDs("doc.control@tataelectronics.co.in" + ";" + key.ApprovalUser);
                            strMailSubject = $"Documents published before {cutOffDate.ToString("dd-MMM-yyyy")} required to revalidate";
                            StringBuilder strMailBody = new StringBuilder();
                            strMailBody.Append($"Your are the creator of attached documents, that are published before {cutOffDate.ToString("dd-MMM-yyyy")} required to revalidate before {FinalDate.ToString("dd-MMM-yyyy")}");
                            strMailBody.Append("<br/><br/>");
                            strMailBody.Append("Link to see published documents: <a style='text-decoration:underline' target='_blank' href='" +
                            ConfigurationManager.AppSettings["websiteURL"].ToString() + "'>" + "Click here" + "</a>");
                            if (Convert.ToBoolean(CommonMethods.GetConfigValue("IsDocumentRevalidationTest").ToString()))
                            {
                                strMailBody.Append("<br/><br/>CC Mailids : " + ccEmailId);
                                toEmailid = "rajesh.m-ext@tataelectronics.co.in";
                                ccEmailId = "rajesh.m-ext@tataelectronics.co.in;siva.d-ext@tataelectronics.co.in;";
                            }
                            LoggerBlock.WriteLog("To MailID:" + toEmailid);
                            LoggerBlock.WriteLog("CC MailID:" + ccEmailId);
                            MailSend.PrepareandSendMail("ApprovalMailTemplate", key.Value.UploadedUserName.ToString(), toEmailid, ccEmailId, "dms.support@tataelectronics.co.in", strMailSubject, strExcelPath, strMailBody.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
            }
        }

        private DataTable GetSharePointListItems()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SectionName"));
            dt.Columns.Add(new DataColumn("DepartmentName"));
            dt.Columns.Add(new DataColumn("DocumentLevel"));
            dt.Columns.Add(new DataColumn("ApprovalUser"));
            // Get the SharePoint site and list
            LoggerBlock.WriteLog("GetSharePointListItems started");
            try
            {
                using (SPSite site = new SPSite(SharePointConstants.siteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists.TryGetList(SharePointConstants.approvalMatrixListName);

                        // If the list exists, retrieve all items
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            query.ViewXml = "<View><Query><Where><And><And><IsNotNull><FieldRef Name='Section'/></IsNotNull><IsNotNull><FieldRef Name='Department'/></IsNotNull></And><Eq><FieldRef Name='ApprovalStage' /><Value Type='Text'>Document Reviewer</Value></Eq></And></Where></Query></View>";
                            query.ViewFields = "<FieldRef Name='Section' /><FieldRef Name='Department' /><FieldRef Name='Level' /><FieldRef Name='ApprovalUser' />";
                            query.ViewFieldsOnly = true; // Only retrieve the specified fields
                            SPListItemCollection items = list.GetItems(query);

                            foreach (SPListItem item in items)
                            {
                                // Do something with each item
                                DataRow dr = dt.NewRow();
                                if (item["Section"] != null)
                                {
                                    dr["SectionName"] = item["Section"].ToString().Split('#')[1];
                                    LoggerBlock.WriteLog("SectionName:" + dr["SectionName"].ToString());
                                }
                                if (item["Department"] != null)
                                {
                                    dr["DepartmentName"] = item["Department"].ToString().Split('#')[1];
                                    LoggerBlock.WriteLog("DepartmentName:" + dr["DepartmentName"].ToString());
                                }
                                if (item["Level"] != null)
                                {
                                    dr["DocumentLevel"] = item["Level"].ToString();
                                    LoggerBlock.WriteLog("DocumentLevel:" + dr["DocumentLevel"].ToString());
                                }
                                if (item["ApprovalUser"] != null)
                                {
                                    dr["ApprovalUser"] = item["ApprovalUser"].ToString();
                                    LoggerBlock.WriteLog("ApprovalUser:" + dr["ApprovalUser"].ToString());
                                }

                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
                LoggerBlock.WriteLog("SharePoint count" + dt.Rows.Count);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteLog("Error while getting approver matrix details");
                LoggerBlock.WriteTraceLog(ex);
            }
            LoggerBlock.WriteLog("GetSharePointListItems completed");
            return dt;
        }

        public string GetEmailIDs(string strEmails)
        {
            strEmails = strEmails.Replace("#", "");
            strEmails = strEmails.Replace(" ", "");
            string[] Approvers = strEmails.Split(';');
            strEmails = string.Empty;
            foreach (string Approver in Approvers)
            {
                try
                {
                    bool isLogin = Regex.IsMatch(Approver, QMSConstants.LoginRegEx, RegexOptions.IgnoreCase);
                    if (isLogin)
                    {
                        if (!strEmails.Contains(Approver))
                            strEmails = strEmails + ";" + Approver;
                        else
                            LoggerBlock.WriteLog("Repeated approver email id :" + Approver);
                    }
                    else
                        LoggerBlock.WriteLog("Invalid approver email id :" + Approver);
                }
                catch (Exception ex)
                {
                    LoggerBlock.WriteTraceLog(ex);
                }
            }
            return strEmails.Trim(';');
        }
    }
}
