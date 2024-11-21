using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TEPLQDMSWS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Logger.SetLogWriter(new LogWriterFactory().Create());
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new QDMSService()
            };
            ServiceBase.Run(ServicesToRun);

            //ApproverReminder arObj = new ApproverReminder();
            //arObj.GetApproverReminderDocuments();
            //arObj.GetApproverEsclationDocuments();

            ////Weekly Mail
            //UserNotifications usObj = new UserNotifications();
            //usObj.SendDigestmailForPublichedDocuments();

            ////Document Revalidation
            //usObj.DocumentRevalidation();
        }
    }
}
