using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEPL.QMS.BLL.Component;
using TEPL.QMS.Common.Constants;

namespace TEPLQMS.Controllers
{
    public class ExternalDocumentsController : Controller
    {
        // GET: ExternalDocuments
        [CustomAuthorize(Roles = "USER,ADMIN,QPADM,QMSADMIN")]
       
        public bool IsValid(string username, string password)
        {
            // Simulated check for valid credentials (replace with actual logic)
            if (username == "admin" && password == "admin123")  // Example credentials
            {
                return true;  // Valid credentials
            }

            return false;  // Invalid credentials
        }

        
        public ActionResult Login(string username, string password)
        {
            try
            {
                if (IsValid(username, password))
                {
                    var userId = User.Identity.GetUserId(); // Get the logged-in user's ID

                    if (string.IsNullOrEmpty(userId))
                    {
                        throw new ArgumentNullException("User ID is null or empty");
                    }

                    Session["LoggedInUserID"] = userId; // Store user ID in session

                    return RedirectToAction("Index"); // Redirect to home/dashboard
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "exception", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {
            try
            {
                // Ensure user is authenticated
                if (!User.Identity.IsAuthenticated)
                {
                    ViewBag.ErrorMessage = "User is not authenticated.";
                    return RedirectToAction("Login", "Account");
                }

                // Retrieve user ID and role
                var userId = Session["LoggedInUserID"] as Guid?;
                if (!userId.HasValue)
                {
                    ViewBag.ErrorMessage = "Session expired or user not authenticated.";
                    return RedirectToAction("Login", "Account");
                }
                var loggedInUserID = userId.Value;

                // Retrieve user roles
                string strRoles = Session[QMSConstants.LoggedInUserRoles] as string;
                if (string.IsNullOrEmpty(strRoles))
                {
                    ViewBag.ErrorMessage = "User roles not found in session.";
                    return RedirectToAction("Login", "Account");
                }

                // Determine if the user is an Admin
                var loggedInUserRole = strRoles.Contains("QADM") ? "Admin" : "User";

                // Fetch documents using the adjusted procedure
                QMSAdmin objAdm = new QMSAdmin();
                var documents = objAdm.GetExternalDocumentsForUsers(loggedInUserID, loggedInUserRole);
                ViewBag.Data = documents;                                
                
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An unexpected error occurred. Please try again later.";
                return View("Error");
            }
        }











    }
}