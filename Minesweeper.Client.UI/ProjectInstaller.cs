using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Minesweeper.Client.UI
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            var path = Path.Combine(Context.Parameters["TargetDir"], "appsettings-client.json");
            AddWritePermissions(path);
        }

        private void AddWritePermissions(string pathToFile)
        {
            var fSecurity = File.GetAccessControl(pathToFile);
            var account = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var rights = FileSystemRights.Read | FileSystemRights.Write;
            var controlType = AccessControlType.Allow;
            var policy = new FileSystemAccessRule(account, rights, controlType);

            fSecurity.AddAccessRule(policy);
            File.SetAccessControl(pathToFile, fSecurity);
        }
    }
}
