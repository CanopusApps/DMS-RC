using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEPL.QMS.BLL.Component;
using System.Configuration;
using Newtonsoft.Json;
using TEPL.QMS.Common.Models;
using TEPL.QMS.Common.Constants;
using System.IO;
using TEPL.QMS.Common.Model;
using TEPL.QMS.Common;
using System.Text;

namespace TEPLQMS.Areas.Admin.Controllers
{
    public class DirectUploadController : Controller
    {
        // GET: DocumentInitiate
        public static Guid CurrentStageID = new Guid("f5d83a73-c4a3-48aa-b3d7-568301eea27b");
        // GET: Admin/DocumentUpload
        public ActionResult Index()
        {

            ViewBag.FileTypes = ConfigurationManager.AppSettings["FileTypes"].ToString();
            ViewBag.FormsFileTypes = ConfigurationManager.AppSettings["FormsFileTypes"].ToString();
            ViewBag.ReadableFileTypes = ConfigurationManager.AppSettings["ReadableFileTypes"].ToString();
            ViewBag.AllowedFileSize = ConfigurationManager.AppSettings["AllowedFileSize"].ToString();

            return View();
        }

        [HttpPost]
        public ActionResult ValidateDocumentNumber()
        {
            try
            {
                string DocumentNumber = Request.Form["DocumentNumber"].ToString();
                string SerialNumber = Request.Form["SerialNumber"].ToString();
                string catCode = Request.Form["CategoryCode"].ToString();
                string projCode = Request.Form["ProjectCode"].ToString();
                string deptCode = Request.Form["DepartmentCode"].ToString();
                string secCode = Request.Form["SectionCode"].ToString();
                int sNo = Convert.ToInt32(SerialNumber);
                SerialNumber = SerialNumber.PadLeft(4,'0');
                DocumentNumber = DocumentNumber + SerialNumber;

                QMSAdmin objAdmin = new QMSAdmin();
                string result = objAdmin.ValidateDocumentNumber(DocumentNumber, sNo, catCode, projCode, deptCode, secCode);

                return Json(new { success = true, message = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ValidateDoc()
        {
            try
            {
                string DocumentNumber = Request.Form["DocumentNumber"].ToString();
                string SerialNumber = Request.Form["SerialNumber"].ToString();
                int sNo = Convert.ToInt32(SerialNumber);
                SerialNumber = SerialNumber.PadLeft(4, '0');
                DocumentNumber = DocumentNumber + SerialNumber;

                QMSAdmin objAdmin = new QMSAdmin();
                string result = objAdmin.ValidateDocNo(DocumentNumber);
                return Json(new { success = true, message = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SubmitDocument()
        {
            string result = "";
            try
            {
                LoggerBlock.WriteLog("SubmitDocument function called.");
                LoggerBlock.WriteLog("GetDocumentObject function called.");

                DraftDocument objDoc = CommonMethods.GetDocumentObject(Request.Form);

                LoggerBlock.WriteLog("GetDocumentObject function end.");
                StringBuilder strMessage = new StringBuilder();
                string DocumentNumber = Request.Form["DocumentNumber"].ToString();
                string SerialNumber = Request.Form["SerialNumber"].ToString();
                int sNo = Convert.ToInt32(SerialNumber);
                SerialNumber = SerialNumber.PadLeft(4, '0');
                DocumentNumber = DocumentNumber + SerialNumber;
                objDoc.DocumentNo = DocumentNumber;
                objDoc.CompanyCode = QMSConstants.CompanyCode;
                objDoc.WorkflowID = QMSConstants.DirectUploadWorkflowID;
                objDoc.CurrentStageID = CurrentStageID;
                objDoc.UploadedUserID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
                DocumentUpload bllOBJ = new DocumentUpload();
                LoggerBlock.WriteLog("SaveDocumentNumber function called.");
                objDoc = bllOBJ.SaveDocumentNumber(objDoc);
                LoggerBlock.WriteLog("SaveDocumentNumber function end.");

                objDoc.CompanyCode = QMSConstants.CompanyCode;
                objDoc.UploadedUserID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
                objDoc.UploadedUserName = System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserDisplayName].ToString();
                objDoc.RevisionReason = "";
                objDoc.CurrentStageID = CurrentStageID;
                objDoc.DraftVersion = Convert.ToDecimal(Request.Form["DocumentVersion"].ToString());
                objDoc.Action = "Submitted";

                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string flname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        flname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        LoggerBlock.WriteLog("Document save function called.");
                        flname = file.FileName;
                        byte[] fileByteArray = new byte[file.ContentLength];
                        file.InputStream.Read(fileByteArray, 0, file.ContentLength);
                        if (i == 0)
                        {
                            objDoc.EditableByteArray = fileByteArray;
                            objDoc.EditableDocumentName = objDoc.DocumentNo + Path.GetExtension(file.FileName);
                            //objDoc.EditableFilePath = CommonMethods.CombineUrl(objDoc.ProjectID.ToString(), QMSConstants.EditableFolder, objDoc.DepartmentID.ToString(), objDoc.SectionID.ToString(), objDoc.DocumentCategoryID.ToString());
                            objDoc.EditableFilePath = CommonMethods.CombineUrl(objDoc.ProjectID.ToString(), QMSConstants.EditableFolder);
                        }
                        else if (i == 1)
                        {
                            objDoc.ReadableByteArray = fileByteArray;
                            objDoc.ReadableDocumentName = objDoc.DocumentNo + Path.GetExtension(file.FileName);
                            //objDoc.ReadableFilePath = CommonMethods.CombineUrl(objDoc.ProjectID.ToString(), QMSConstants.ReadableFolder, objDoc.DepartmentID.ToString(), objDoc.SectionID.ToString(), objDoc.DocumentCategoryID.ToString());
                            objDoc.ReadableFilePath = CommonMethods.CombineUrl(objDoc.ProjectID.ToString(), QMSConstants.ReadableFolder);
                        }
                    }
                    LoggerBlock.WriteLog("Document save function end.");
                }
                LoggerBlock.WriteLog("DocumentDirectUpload function Called.");
                bllOBJ.DocumentDirectUpload(objDoc);
                LoggerBlock.WriteLog("DocumentDirectUpload function end.");
                result = "success";
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                result = "failed";
                //throw ex;
            }
            return Json(new { success = true, message = result }, JsonRequestBehavior.AllowGet);
        }

    }
}