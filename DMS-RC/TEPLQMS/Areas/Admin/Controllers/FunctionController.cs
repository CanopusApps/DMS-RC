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
    public class FunctionController : Controller
    {
        [CustomAuthorize(Roles = "QADM")]
        public ActionResult Index()
        {
            try
            {
                QMSAdmin objAdm = new QMSAdmin();
                ViewBag.Data = objAdm.GetFunction();
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = false, message = "error" }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddFunction()
        {
            string result = "";
            try
            {
                Function objFun = new Function();
                if (Request.Form["FunctionCode"].ToString() != "")
                    objFun.Code = Request.Form["FunctionCode"].ToString();
                if (Request.Form["FunctionName"].ToString() != "")
                    objFun.Title = Request.Form["FunctionName"].ToString();
                QMSAdmin objAdmin = new QMSAdmin();
                result = objAdmin.AddFunction(objFun);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                result = "failed";
                //throw ex;
            }
            return Json(new
            {
                success = true,
                message = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateFunction()
        {
            string result = "";
            try
            {
                Function objFun = new Function();
                if (Request.Form["FunctionID"].ToString() != "")
                    objFun.ID = new Guid(Request.Form["FunctionID"].ToString());
                if (Request.Form["FunctionCode"].ToString() != "")
                    objFun.Code = Request.Form["FunctionCode"].ToString();
                if (Request.Form["FunctionName"].ToString() != "")
                    objFun.Title = Request.Form["FunctionName"].ToString();
                if (Request.Form["Active"].ToString().ToLower() == "true")
                    objFun.Active = true;
                else
                    objFun.Active = false;
                QMSAdmin objAdmin = new QMSAdmin();
                result = objAdmin.UpdateFunction(objFun);
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                result = "failed";
                //throw ex;
            }
            return Json(new
            {
                success = true,
                message = result
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFunction()
        {
            string result = "";

            try
            {
                Guid ID = new Guid(Request.Form["ID"].ToString());
                QMSAdmin objAdmin = new QMSAdmin();
                result = objAdmin.DeleteFunction(ID);
                // result = "sucess";

            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                result = "failed";
                throw ex;
            }
            return Json(new
            {
                success = true,
                message = "Function Deleted Successfully."
            }, JsonRequestBehavior.AllowGet);
        }
    }
}