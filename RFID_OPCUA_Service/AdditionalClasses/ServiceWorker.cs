using System.Collections.Generic;
using Microsoft.Win32;
using System.Timers;
using System;

namespace RFID_OPCUA_Service.AdditionalClasses
{
    class ServiceWorker
    {
        #region Fields
        RegistryKey rfidKey;
        RegistryKey software;
        RegistryKey configData;
        RegistryKey argsKey;


        Timer tm;
        List<RF600> listRF;
        #endregion
        // Конструктор
        public ServiceWorker()
        {
            listRF = new List<RF600>();

            RegistryKey cu = Registry.CurrentUser;

            software = cu.OpenSubKey("Software", true);
            rfidKey = software.CreateSubKey("RFID", true);

            configData = rfidKey.CreateSubKey("config", true);
            argsKey = rfidKey.CreateSubKey("args", true);
        }

        public void startScan()
        {
            // получение порядкового номера считывателя из рееста
            int num = (int)argsKey.GetValue("numRFID");
            listRF[num].startScan(); // Отправка команды на экземпляр
        }
        public void stopScan()
        {
            // получение порядкового номера считывателя из рееста
            int num = (int)argsKey.GetValue("numRFID");
            listRF[num].stopScan(); // Отправка команды на экземпляр
        }
        public void readTag() 
        {
            int numRFID = (int)argsKey.GetValue("numRFID"); // Номер считывателя
            int numRP = (int)argsKey.GetValue("numRP");     // Номер антенны
            listRF[numRFID].readTag(numRP); // Запрос на считывание в экземпляр
        }
        public void writeTag() {
            int numRFID = (int)argsKey.GetValue("numRFID"); // Номер считывателя
            int numRP = (int)argsKey.GetValue("numRP"); // Номер антенны
            listRF[numRFID].writeTag(numRP);
        }
        public void unkCmd(int cmd) { }     //TODO: Сделать обработку неизвестной команды
        // Инициализация считывателей при запуске службы
        public void init()
        {
            tm = new Timer(2000);
            tm.Elapsed += reconnectOPC;
            tm.Enabled = true;
            tm.AutoReset = false;

            // Считывание параметров из реестра
            List<string> ip_list = new List<string>(configData.GetValue("ipPort") as string[]); // Список IP адресов
            List<string> nm_list = new List<string>(configData.GetValue("name") as string[]);   // Список наименований (Должны быть уникальны, нет дополнительной проверки)
            // Создание списка экземпляров считывателя
            for (int i=0; i<ip_list.Count; i++)
            {
                listRF.Add(new RF600(ip_list[i], nm_list[i]));
            }
            // Создание подключения и запуск считывания при запуске службы
            foreach(var rfid in listRF)
            {
                rfid.connectOPC();
            }
        }
        // Закрытие соедининий с реесторм 
        public void destroy()
        {
            foreach (var rfid in listRF)
            {
                rfid.stopScan();
            }
            tm.Dispose();
            rfidKey.Close();
            software.Close();
        }

        private void reconnectOPC(Object source, ElapsedEventArgs e)
        {
            List<RF600> nc = null;
            nc = listRF.FindAll(x => !x.connected);
            if (nc.Count > 0)
            {
                foreach (RF600 rf in listRF)
                {
                    rf.connectOPC();
                    if (rf.connected)
                        rfidKey.SetValue("Debug", "Reconnect rfid | " + e.SignalTime);
                    else
                        rfidKey.SetValue("Debug", "Err reconnect rfid | " + e.SignalTime);

                }
                tm.Start();
            }
            else
            {
                rfidKey.SetValue("Debug", "All connected | " + e.SignalTime);
            }
            
        }

    }

}
