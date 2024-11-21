using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEPL.QMS.BLL.Component;
using TEPL.QMS.Common;
using TEPL.QMS.Common.Constants;
using TEPL.QMS.Common.Models;

namespace TEPLQMS.Controllers
{
    public class PrintRequestController : Controller
    {
        public static Guid CurrentStageID = new Guid("f5d83a73-c4a3-48aa-b3d7-568301eea27b");
        // GET: ExistingDocumentUpload
        [CustomAuthorize(Roles = "USER")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            DocumentUpload obj = new DocumentUpload();
            string strID = "";
            if (Request.QueryString["ID"] != null)
                strID = Request.QueryString["ID"].ToString();
            if (strID != "")
            {
                Guid loggedUsedID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
                PrintRequest request = obj.GetPrintRequestDetailsByID("Approver", loggedUsedID, new Guid(strID));
                ViewBag.Data = request;
            }
            else
            {
                ViewBag.Data = null;
            }
            ViewBag.DisplayDownload = false;
            if (Request.QueryString["display"] != null)
            {
                if(Request.QueryString["display"].ToString() == "true")
                    ViewBag.DisplayDownload = true;
            }
            
            ViewBag.ViewerURL = ConfigurationManager.AppSettings["ViewerURL"].ToString();
            return View();
        }

        [HttpPost]
        public ActionResult GetDropDownBoxes()
        {
            try
            {
                QMSAdmin objAdm = new QMSAdmin();
                List<Location> list1 = objAdm.GetActiveLocations();                
                return Json(new { success = true, message1 = list1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetDocumentDetails(string documentNumber)
        {
            try
            {
                DocumentUpload bllOBJ = new DocumentUpload();
                string userID = System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID].ToString();
                Guid UserID = new Guid(userID);
                List<Project> list3 = (List<Project>)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserProjects];
                if (documentNumber.Split('-').Length == 1)
                {
                    return Json(new { success = true, message = "invalid" }, JsonRequestBehavior.AllowGet);
                }
                //string ProjectCode = documentNumber.Split('-')[1];
                //bool blAccess = false;
                //foreach(Project pt in list3)
                //{
                //    if(pt.Code==ProjectCode)
                //    {
                //        blAccess = true;
                //    }
                //}
                //if(ProjectCode == "QM")
                //{
                //    blAccess = true;
                //}
                //if (blAccess)
                //{

                //}
                //else
                //{
                //    return Json(new { success = true, message = "noaccess" }, JsonRequestBehavior.AllowGet);
                //}
                Object[] ArrayOfObjects = new Object[3];
                ArrayOfObjects = bllOBJ.GetDocumentDetailsForPrintRequest(documentNumber, UserID);
                return Json(new { success = true, message = ArrayOfObjects }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SubmitPrintRequest()
        {
            string result = "";
            try
            {
                PrintRequest objPrint = CommonMethods.GetPrintRequestObject(Request.Form);
                objPrint.RequestorID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
                objPrint.RequestorName = System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserDisplayName].ToString();
                objPrint.WorkflowID = new Guid(QMSConstants.PrintWorkflowID.ToString());
                objPrint.CurrentStageID = CurrentStageID;
                objPrint.Action = "Submitted";
                objPrint.Status = "Completed";

                DocumentUpload bllOBJ = new DocumentUpload();
                bllOBJ.SubmitPrintRequest(objPrint);
                result = "success";
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                result = "failed"; return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = result }, JsonRequestBehavior.AllowGet);
        }
    }
}