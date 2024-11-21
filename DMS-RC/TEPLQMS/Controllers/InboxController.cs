using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEPL.QMS.BLL.Component;
using TEPL.QMS.Common;
using TEPL.QMS.Common.Constants;
using TEPL.QMS.Common.Model;
using TEPLQMS.Models;
namespace TEPLQMS.Controllers
{
    public class InboxController : Controller
    {
        // GET: Inbox
        [CustomAuthorize(Roles = "USER")]
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult GetUserRequests()
        {
            try
            {
                DocumentUpload obj = new DocumentUpload();
                var objDocs = obj.GetRequestedDocuments((Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID]);
                return Json(new { success = true, message = objDocs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = false, message = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetApproverRequests()
        {
            try
            {
                DocumentUpload obj = new DocumentUpload();
                var objDocs = obj.GetApprovalPendingDocuments((Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID]);
                return Json(new { success = true, message = objDocs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = false, message = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetApprovedRequests()
        {
            try
            {
                DocumentUpload obj = new DocumentUpload();
                var objDocs = obj.GetApprovedDocuments((Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID]);
                return Json(new { success = true, message = objDocs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = false, message = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<Requests> GetRequests()
        {
            List<Requests> lstRequests = null;
            try
            {
                lstRequests = new List<Requests>();

                Requests rst = new Requests();
                rst.DocumentNo = "<a href='http://goolgle.com' target='_blank' style='text-decoration:underline;'>TEPL-NNB101-EN-AE-PO-0001.docx</a>";
                rst.Category = "Standard Document";
                rst.Department = "";
                rst.Section = "";
                rst.UploadedBy = "";
                rst.UploadedOn = DateTime.Now;
                rst.Stage = "";
                rst.PendingWith = "";
            }
            catch(Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
            }
            return lstRequests;
        }
    }
}