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
        public static string spGetWFApprovalMatrix = "spGetWFApprovalMatrix";
        public static string spGetWFApprovers = "spGetWFApprovers";
        public static string spInsertWFApprovalMatrix = "spInsertWFApprovalMatrix";
        public static string spUpdateWFApprovalMatrix = "spUpdateWFApprovalMatrix";
        public static string spGetApprovalPendingDocumentsWITH_HOD = "spGetApprovalPendingDocumentsWITH_HOD";
        public static string spSetArchivedLatePendingDocuments = "spSetArchivedLatePendingDocuments";


    }
}
