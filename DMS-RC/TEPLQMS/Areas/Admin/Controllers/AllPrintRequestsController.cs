using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEPL.QMS.BLL.Component;
using TEPL.QMS.Common;
using TEPL.QMS.Common.Constants;
using TEPL.QMS.Common.Models;
using TEPLQMS.Controllers;

namespace TEPLQMS.Areas.Admin.Controllers
{

    public class AllPrintRequestsController : Controller
    {
        [CustomAuthorize(Roles = "QADM")]
        public ActionResult Index()
        {
            try
            {
                Guid LoggedInUserID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
                QMSAdmin objQMSAdmin = new QMSAdmin();
                string strRoles = System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserRoles].ToString();
                if (strRoles.Contains("QADM"))
                    ViewBag.isQMSAdmin = true;
                else ViewBag.isQMSAdmin = false;

                DocumentUpload obj = new DocumentUpload();
                ViewBag.Data = obj.GetPublishedDocuments("", "", "", "", "", LoggedInUserID, true);
                ViewBag.UTCTIME = DateTime.UtcNow.ToString();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "error" }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public ActionResult GetPublishedDocument(string department, string section, string project, string category, string DocumentDescription, bool IsProjectActive)
        {
            try
            {
                Guid LoggedInUserID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
                DocumentUpload obj = new DocumentUpload();
                var objDocs = obj.GetPublishedDocument(department, section, project, category, DocumentDescription, LoggedInUserID, IsProjectActive);
                return Json(new { success = true, message = objDocs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = false, message = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetExportPrintedDocuments(string department, string section, string project, string category, string DocumentDescription, bool IsProjectActive)
        {
            Guid LoggedInUserID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
            DocumentUpload obj = new DocumentUpload();
            byte[] fileContent = obj.GetExportPrintedDocuments(department, section, project, category, DocumentDescription, LoggedInUserID, IsProjectActive);
            //return File(fileContent, System.Net.Mime.MediaTypeNames.Application.Octet, LoggedInUserID + ".xlsx");
            string base64 = Convert.ToBase64String(fileContent, 0, fileContent.Length);
            return Content(base64);
        }
        public ActionResult GetAllApprovedPrintRequests()
        {
            try
            {
                DocumentUpload obj = new DocumentUpload();
                var objDocs = obj.GetAllApprovedPrintRequests();
                return Json(new { success = true, message = objDocs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = false, message = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}