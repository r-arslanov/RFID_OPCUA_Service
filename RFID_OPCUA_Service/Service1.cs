using Microsoft.Win32;
using System.ServiceProcess;

namespace RFID_OPCUA_Service
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            RegistryKey cu = Registry.CurrentUser;
            RegistryKey software = cu.OpenSubKey("Software", true);
            RegistryKey rfidKey = software.CreateSubKey("RFID", true);
            rfidKey.SetValue("testPar", "testVal123");
            rfidKey.Close();
            software.Close();
        }

        protected override void OnStop()
        {
        }
    }
}
