using Microsoft.Win32;
using System.ServiceProcess;
using System.Threading;
using RFID_OPCUA_Service.AdditionalClasses;

namespace RFID_OPCUA_Service
{
    public partial class RFIDCOMSERVICE : ServiceBase
    {
        #region Constants
        const int START_SCAN = 130;     // Запуск постоянного сканирования RFID меток
        const int STOP_SCAN = 131;      // Остановка сканирования

        const int READ_TAG = 140;       // Считать EPC ID(96bit)
        const int WRITE_TAG = 141;      // Записать EPC ID(96bit)
        #endregion

        #region Fields
        ServiceWorker worker;
        #endregion

        #region Contructor
        public RFIDCOMSERVICE()
        {
            InitializeComponent();
            worker = new ServiceWorker();
        }
        #endregion

        protected override void OnStart(string[] args)
        {
            // Запуск логики в отдельном потоке для взаимодействия с контроллером служб windows
            Thread tWorker = new Thread(worker.init);
            tWorker.Start();
        }

        protected override void OnStop()
        {
            // Остановка внешнего процесса при остановке службы
            worker.destroy();
        }

        protected override void OnCustomCommand(int command)
        {
            // Обработчик команд на начало/конец считывания | чтение/запись EPC ID
            switch (command)
            {
                case START_SCAN:
                    worker.startScan();
                    break;
                case STOP_SCAN:
                    worker.stopScan();
                    break;
                case READ_TAG:
                    worker.readTag();
                    break;
                case WRITE_TAG:
                    worker.writeTag();
                    break;
                default:
                    worker.unkCmd(command);
                    break;
            }
           // base.OnCustomCommand(command);
        }
    }


}
