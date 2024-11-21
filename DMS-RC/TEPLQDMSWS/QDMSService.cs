using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TEPL.QMS.Common;
using TEPL.QMS.Common.DAL;

namespace TEPLQDMSWS
{
    public partial class QDMSService : ServiceBase
    {
        Timer timer = new Timer();
        public QDMSService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LoggerBlock.WriteLog("Service is started at ");
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds  
            timer.Enabled = true;
        }
        protected override void OnStop()
        {
            LoggerBlock.WriteLog("Service is stopped at ");
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            LoggerBlock.WriteLog("Service is recall started at ");
            ProgramToRun();
            LoggerBlock.WriteLog("Service is recall ended at ");
        }
        private void ProgramToRun()
        {
            LoggerBlock.WriteLog("Task Schedules started");
            DataTable dt = ConfigDAL.GetTaskSchedules();
            foreach (DataRow dr in dt.Rows)
            {
                LoggerBlock.WriteLog(dr["ProgramName"].ToString() + " is started by Windows service");
                switch (dr["ProgramName"])
                {
                    case "ApprovalReminder":
                        ApproverReminder arObj = new ApproverReminder();
                        Task st = new Task(arObj.GetApproverReminderDocuments);
                        st.Start();
                        break;
                    case "ApprovalEsclation":
                        ApproverReminder arObj1 = new ApproverReminder();
                        Task st1 = new Task(arObj1.GetApproverEsclationDocuments);
                        st1.Start();
                        break;
                    case "SendDigestmailForPublichedDocuments":
                        UserNotifications unObj = new UserNotifications();
                        Task st2 = new Task(unObj.SendDigestmailForPublichedDocuments);
                        st2.Start();
                        break;
                    case "DocumentRevalidation":
                        UserNotifications unObj1 = new UserNotifications();
                        Task st3 = new Task(unObj1.DocumentRevalidation);
                        st3.Start();
                        break;
                }
                LoggerBlock.WriteLog(dr["ProgramName"].ToString() + " is ended by Windows service");
            }
            LoggerBlock.WriteLog("Task Schedules ended");
        }
    }
}
