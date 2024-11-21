using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEPL.QDMS.WindowsService.DAL;

namespace TEPL.QDMS.WindowsService.Business
{
    public class WSAdminBLL
    {
        WSAdminDAL objAdminDAL = new WSAdminDAL();
        public DataTable GetApprovalPendingDocuments()
        {
            return objAdminDAL.GetApprovalPendingDocuments();
        }
        public DataTable GetPublishedDocumentsForDigest(DateTime startDate, DateTime endDate)
        {
            return objAdminDAL.GetPublishedDocumentsForDigest(startDate,endDate);
        }
        public DataTable GetRevalidationDocuments(DateTime cutOffDate)
        {
            return objAdminDAL.GetRevalidationDocuments(cutOffDate);
        }
        public DataTable GetApprovalPendingDocumentsHOD()
        {
            return objAdminDAL.GetApprovalPendingDocumentsHOD();
        }
        public DataTable SetIsArchivedForDocuments(string DocumentNo)
        {
            return objAdminDAL.SetIsArchivedForDocuments(DocumentNo);
        }
    }
}
