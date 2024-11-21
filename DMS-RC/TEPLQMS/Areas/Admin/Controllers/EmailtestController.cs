using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TEPL.QMS.BLL.Component;
using TEPL.QMS.Common;
using TEPL.QMS.Common.Constants;
using TEPL.QMS.Common.Models;
using TEPLQMS.Controllers;
using System.Configuration;
using OfficeOpenXml;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using TEPL.QMS.Workflow.Constants;

namespace TEPLQMS.Areas.Admin.Controllers
{
    
    public class EmailtestController : Controller
    {
        //[CustomAuthorize(Roles = "QADM")]
        public ActionResult Index()
        {
            try
            {

            }
            catch (Exception ex)
            {
                //return Json(new { success = false, message = "error" }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public JsonResult SendEmail(string fromAddress, string toAddress, string message)
        {
            if (string.IsNullOrEmpty(fromAddress) || string.IsNullOrEmpty(toAddress) || string.IsNullOrEmpty(message))
            {
                return Json("Invalid input data.", JsonRequestBehavior.AllowGet);
            }
            try
            {
                string smtpServer = ConfigurationManager.AppSettings["SMTPHOST"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPORT"]);
                var Email = toAddress;
                var ApprovalName = "Manas";
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(Email);
                    mail.Subject = ConfigurationManager.AppSettings["Subject"];
                    mail.IsBodyHtml = true;
                    mail.Body = "Dear " + ApprovalName + ",<br /><br />" + message + ".  <br /><br /><br /><br /><br /> Thanks & Regards,<br /> Kaizen Team ";
                    using (SmtpClient smtp = new SmtpClient(smtpServer, port))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailUserName"], ConfigurationManager.AppSettings["EmailPassword"]);
                        smtp.EnableSsl = false;
                        smtp.Send(mail);
                    }
                }

                return Json("Email sent successfully!");
            }
            catch (Exception ex)
            {
                return Json("Error sending email: " + ex.Message);
            }
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file) 
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    if (fileExtension == ".xlsx" || fileExtension == ".xls")
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var worksheet = package.Workbook.Worksheets[0]; 
                            var rowCount = worksheet.Dimension.Rows;
                            var colCount = worksheet.Dimension.Columns;
                            for (int row = 2; row <= rowCount; row++) 
                            {
                                string UserPrincipalName = worksheet.Cells[row, 1].Text;
                                string UserName = UserPrincipalName.Split('@')[0];
                                string DisplayName = worksheet.Cells[row, 3].Text;

                                SaveDataToDatabase(UserName, DisplayName,UserPrincipalName);
                            }
                        }
                        return Json("File uploaded successfully.");
                    }
                    else
                    {
                        return Json("Please upload a valid Excel file.");
                    }
                }
                catch (Exception ex)
                {
                    // Log error (ex) if needed
                    return Json("Error: " + ex.Message);
                }
            }
            else
            {
                return Json("No file selected.");
            }
        }
        public void SaveDataToDatabase(string UserName, string DisplayName,string UserPrincipalName)
        {
            using (SqlConnection con = new SqlConnection(WFConstants.WFDBCon))
            {
                using (SqlCommand cmd = new SqlCommand(WFConstants.sp_upload_bulkdata, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;
                    cmd.Parameters.Add("@DisplayName", SqlDbType.NVarChar).Value = DisplayName;
                    cmd.Parameters.Add("@UserPrincipalName", SqlDbType.NVarChar).Value = UserPrincipalName;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

    }
}