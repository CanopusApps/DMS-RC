using System.Configuration;
using TEPL.QMS.Common.Constants;

namespace TEPLQDMSWS
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.QDMSserviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.QDMSServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.QDMSserviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.QDMSServiceInstaller.Description = "TEPL QDMS Windows services";
            this.QDMSServiceInstaller.DisplayName = "TEPLQDMSService";
            // 
            // QDMSserviceProcessInstaller
            // 
            //this.QDMSserviceProcessInstaller.Username = "tepl\\itqdmsspprd";// QMSConstants.ServiceAccountName;
            //this.QDMSserviceProcessInstaller.Password = "T3p1#dw$#22";// QMSConstants.ServiceAccountPassword;
            // 
            // QDMSServiceInstaller
            // 
            this.QDMSServiceInstaller.ServiceName = "QDMSService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.QDMSserviceProcessInstaller,
            this.QDMSServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller QDMSserviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller QDMSServiceInstaller;
    }
}