using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
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
    public class ApprovePrintRequestController : Controller
    {
        // GET: ApproveRequest
        [CustomAuthorize(Roles = "USER")]
        public ActionResult Index()
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
            ViewBag.ViewerURL = ConfigurationManager.AppSettings["ViewerURL"].ToString();
            return View();
        }        

        public ActionResult ViewDocument()
        {
            
            return View();
        }       

        [HttpPost]
        public ActionResult SubmitPrintRequest()
        {
            string result = "";
            try
            {
                PrintRequest objRequest = CommonMethods.GetPrintRequestObject(Request.Form);

                objRequest.ActionID = (Guid)System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserID];
                objRequest.ActionUserName = System.Web.HttpContext.Current.Session[QMSConstants.LoggedInUserDisplayName].ToString();
                //objRequest.Action = "Approved";             

                DocumentUpload bllOBJ = new DocumentUpload();
                if (objRequest.Action == "Approved")
                    result = bllOBJ.ApprovePrintRequest(objRequest);
                else
                    result = bllOBJ.RejectPrintRequest(objRequest);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                result = "failed";
            }
            return Json(new { success = true, message = result }, JsonRequestBehavior.AllowGet);
        }        

    }
}