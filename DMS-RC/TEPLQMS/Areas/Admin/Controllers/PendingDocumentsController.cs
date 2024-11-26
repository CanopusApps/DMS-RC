using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEPL.QMS.BLL.Component;
using TEPL.QMS.Common;
using TEPL.QMS.Common.Constants;
using TEPL.QMS.Common.Models;
using TEPLQMS.Common;
using TEPLQMS.Controllers;

namespace TEPLQMS.Areas.Admin.Controllers
{
    public class PendingDocumentsController : Controller
    {
        // GET: Dashboard
        [CustomAuthorize(Roles = "QADM,QPADM")]
        public ActionResult Index()
        {
            var pageLength = ConfigurationManager.AppSettings["PageLength"];
            ViewBag.PageLength = pageLength; // Pass it to the view
            return View();
        }
        [HttpPost]
        public ActionResult GetDropDownBoxes()
        {
            try
            {
                QMSAdmin objAdm = new QMSAdmin();
                List<Departments> list1 = objAdm.GetDepartments();
                List<DocumentCategory> list2 = objAdm.GetDocumentCategories();


                var list3 = objAdm.GetProjectsbyRole("PendingDocuments", System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserRoles].ToString(),
                       (List<Project>)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserProjects]);

                return Json(new { success = true, message1 = list1, message2 = list2, message3 = list3 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetExportPendingDocuments(string department, string section, string project, string category, string DocumentDescription,bool IsProjectActive)
        {
            Guid LoggedInUserID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
            DocumentUpload obj = new DocumentUpload();
            byte[] fileContent = obj.GetExportPendingDocuments(department, section, project, category, DocumentDescription, LoggedInUserID, IsProjectActive);
            string base64 = Convert.ToBase64String(fileContent, 0, fileContent.Length);
            return Content(base64);
        }
        public ActionResult GetDocumentsPending(string department, string section, string project, string category, string DocumentDescription,bool IsProjectActive)
        {
            try
            {
                Guid LoggedInUserID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
                DocumentUpload obj = new DocumentUpload();
                var objDocs = obj.GetDraftDocuments(department, section, project, category, DocumentDescription, LoggedInUserID, IsProjectActive);
                return Json(new { success = true, message = objDocs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteLog("In exception in GetDocumentsPending in PendingDocuments COntroller and message is " + ex.InnerException.Message.ToString());
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = false, message = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DownloadDraftDocument(string folderPath, double versionNo, string fileName)
        {
            try
            {
                string URL = CommonMethods.CombineUrl(QMSConstants.StoragePath, QMSConstants.DraftFolder, folderPath, fileName);
                DocumentUpload bllOBJ = new DocumentUpload();
                byte[] fileContent = bllOBJ.DownloadDocument(URL);

                return File(fileContent, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = true, message = "failed" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetPendingDocumentDataFromServerSide()
        {
            int pageLength = Convert.ToInt16(ConfigurationManager.AppSettings["PageLength"]);
            int draw = DatatableCall.GetRequestValue<int>("draw", Request);
            int start = DatatableCall.GetRequestValue<int>("start", Request);
            int length = DatatableCall.GetRequestValue<int>("length", Request);
            if (start < 0) start = 0;
            if (length <= 0) length = 10;
            int pageNumber = (start / length) + 1;
            string sortColumnIndex = DatatableCall.GetRequestValue<string>("order[0][column]", Request);
            string sortColumn = DatatableCall.GetRequestValue<string>($"columns[{sortColumnIndex}][data]", Request);
            string sortDirection = DatatableCall.GetRequestValue<string>("order[0][dir]", Request);
            string searchValue = DatatableCall.GetRequestValue<string>("search[value]", Request);

            string DepartmentCode = DatatableCall.GetRequestValue<string>("department", Request);
            string SectionCode = DatatableCall.GetRequestValue<string>("section", Request);
            string ProjectCode = DatatableCall.GetRequestValue<string>("project", Request);
            string DocumentCategoryCode = DatatableCall.GetRequestValue<string>("category", Request);
            string Documentno = DatatableCall.GetRequestValue<string>("documentno", Request);
            string DocumentDescription = DatatableCall.GetRequestValue<string>("description", Request);

            Guid LoggedInUserID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
            QMSAdmin objQMSAdmin = new QMSAdmin();
            string strRoles = System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserRoles].ToString();
            if (strRoles.Contains("QADM"))
                ViewBag.isQMSAdmin = true;
            else ViewBag.isQMSAdmin = false;

            DocumentUpload obj = new DocumentUpload();
            (List<DraftDocument> data, int totalpage) = obj.GetPendingDocumentFromServer(DepartmentCode, SectionCode, ProjectCode, DocumentCategoryCode, Documentno, DocumentDescription, LoggedInUserID, true, pageNumber, length, sortColumn, sortDirection);
            var recordsTotal = totalpage;//data.Count();
            data = DatatableCall.ApplyPaging(data, start, length).ToList();
            var response = new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = data
            };

            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}