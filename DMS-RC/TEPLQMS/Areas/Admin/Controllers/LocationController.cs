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
    public class LocationController : Controller
    {
        [CustomAuthorize(Roles = "QADM")]
        public ActionResult Index()
        {
            try
            {
                QMSAdmin objAdm = new QMSAdmin();
                ViewBag.Data = objAdm.GetLocation();
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
                return Json(new { success = false, message = "error" }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddLocation()
        {
            string result = "";
            try
            {
                Location objLoc = new Location();
                if (Request.Form["LocationCode"].ToString() != "")
                    objLoc.Code = Request.Form["LocationCode"].ToString();
                if (Request.Form["LocationName"].ToString() != "")
                    objLoc.Title = Request.Form["LocationName"].ToString();
                QMSAdmin objAdmin = new QMSAdmin();
                result = objAdmin.AddLocation(objLoc);
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
        public ActionResult UpdateLocation()
        {
            string result = "";
            try
            {
                Location objLoc = new Location();
                if (Request.Form["ID"].ToString() != "")
                    objLoc.ID = new Guid(Request.Form["ID"].ToString());
                if (Request.Form["LocationCode"].ToString() != "")
                    objLoc.Code = Request.Form["LocationCode"].ToString();
                if (Request.Form["LocationName"].ToString() != "")
                    objLoc.Title = Request.Form["LocationName"].ToString();
                if (Request.Form["Active"].ToString().ToLower() == "true")
                    objLoc.Active = true;
                else
                    objLoc.Active = false;
                QMSAdmin objAdmin = new QMSAdmin();
                result = objAdmin.UpdateLocation(objLoc);
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

        public ActionResult DeleteLocation()
        {
            string result = "";

            try
            {
                Guid ID = new Guid(Request.Form["ID"].ToString());
                QMSAdmin objAdmin = new QMSAdmin();
                result = objAdmin.DeleteLocation(ID);
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
                message = "Location Deleted Successfully."
            }, JsonRequestBehavior.AllowGet);
        }
    }
}