
namespace Minesweeper.Service
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
            this.eventLog = new System.Diagnostics.EventLog();
            this.eventLogInstaller = new System.Diagnostics.EventLogInstaller();
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).BeginInit();
            // 
            // eventLog
            // 
            this.eventLog.Log = "MinesweeperServiceLog";
            this.eventLog.Source = "MinesweeperServiceSource";
            // 
            // eventLogInstaller
            // 
            this.eventLogInstaller.CategoryCount = 0;
            this.eventLogInstaller.CategoryResourceFile = null;
            this.eventLogInstaller.Log = "MinesweeperServiceLog";
            this.eventLogInstaller.MessageResourceFile = null;
            this.eventLogInstaller.ParameterResourceFile = null;
            this.eventLogInstaller.Source = "MinesweeperServiceSource";
            // 
            // serviceProcessInstaller
            // 
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;
            // 
            // serviceInstaller
            // 
            this.serviceInstaller.DisplayName = "Minesweeper Service";
            this.serviceInstaller.ServiceName = "MinesweeperService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.eventLogInstaller,
            this.serviceProcessInstaller,
            this.serviceInstaller});
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).EndInit();

        }

        #endregion
        private System.Diagnostics.EventLogInstaller eventLogInstaller;
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
        private System.Diagnostics.EventLog eventLog;
    }
}