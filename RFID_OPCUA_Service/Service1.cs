using Microsoft.Win32;
using System.ServiceProcess;
using System.Threading;
using RFID_OPCUA_Service.AdditionalClasses;

namespace RFID_OPCUA_Service
{
    public partial class Service1 : ServiceBase
    {
        #region Fields
        ServiceWorker worker;
        #endregion
        public Service1()
        {
            InitializeComponent();
            worker = new ServiceWorker();
        }

        protected override void OnStart(string[] args)
        {
            Thread tWorker = new Thread(worker.init);
            tWorker.Start();
        }

        protected override void OnStop()
        {
            worker.destroy();
        }
    }


}
