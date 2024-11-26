using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace TEPL.QDMS.WindowsService.Constants
{
    public static class WSConstants
    {
        public static string DBCon = Convert.ToString(ConfigurationManager.ConnectionStrings["DBCon"]);

        //Stored procedure
        public static string spGetApprovalPendingDocuments = "spGetApprovalPendingDocuments";
        public static string spGetPublishedDocumentsForDigest = "spGetPublishedDocumentsForDigest";
        public static string spGetRevalidationDocuments="spGetRevalidationDocuments";
        public static string spCreateAction = "spCreateAction";
        public static string spExecuteAction = "spExecuteAction";
        public static string spGetWorkflowStage = "spGetWorkflowStage";
        public static string spGetPendingActionItems = "spGetPendingActionItems";
        public static string spGetWorkflowStages = "spGetWorkflowStages";
        public static string spGetExecutionDetails = "spGetExecutionDetails";
        public static string spGetWFStages = "spGetWFStages";
        //public static string spGetWFApprovalMatrix = "spGetWFApprovalMatrix";
        //public static string spGetWFApprovers = "spGetWFApprovers";
        //public static string spInsertWFApprovalMatrix = "spInsertWFApprovalMatrix";
        //public static string spUpdateWFApprovalMatrix = "spUpdateWFApprovalMatrix";
        public static string spGetApprovalPendingDocumentsWITH_HOD = "spGetApprovalPendingDocumentsWITH_HOD";
        public static string spSetArchivedLatePendingDocuments = "spSetArchivedLatePendingDocuments";

        public static string spGetConfigValue = "spGetConfigValue";
        public static string spGetTaskSchedules = "spGetTaskSchedules";
        public static string SMTPPORT = Convert.ToString(ConfigurationManager.AppSettings["SMTPPORT"]);
        public static string SMTPHOST = Convert.ToString(ConfigurationManager.AppSettings["SMTPHOST"]);
        public static string EmailUserName = Convert.ToString(ConfigurationManager.AppSettings["EmailUserName"]);
        public static string EmailPassword = Convert.ToString(ConfigurationManager.AppSettings["EmailPassword"]);
        public static string EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);


    }
}
