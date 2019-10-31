using Microsoft.Win32;
using System.ServiceProcess;
using System.Threading;
using RFID_OPCUA_Service.AdditionalClasses;

namespace RFID_OPCUA_Service
{
    public partial class Service1 : ServiceBase
    {
        #region Constants
        const int START_SCAN = 130;
        const int STOP_SCAN = 131;

        const int READ_TAG = 140;
        const int WRITE_TAG = 141;
        #endregion

        #region Fields
        ServiceWorker worker;
        #endregion

        #region Contructor
        public Service1()
        {
            InitializeComponent();
            worker = new ServiceWorker();
        }
        #endregion

        protected override void OnStart(string[] args)
        {
            Thread tWorker = new Thread(worker.init);
            tWorker.Start();
        }

        protected override void OnStop()
        {
            worker.destroy();
        }

        protected override void OnCustomCommand(int command)
        {
            switch (command)
            {
                case 130:
                    worker.startScan();
                    break;
                case 131:
                    worker.stopScan();
                    break;
            }
           // base.OnCustomCommand(command);
        }
    }


}
