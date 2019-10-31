using System.Collections.Generic;
using Microsoft.Win32;

namespace RFID_OPCUA_Service.AdditionalClasses
{
    class ServiceWorker
    {
        #region Fields
        int cRFID;
        RegistryKey rfidKey;
        RegistryKey software;
        RegistryKey configData;
        RegistryKey argsKey;

        List<RF680R> listRF;
        #endregion

        public ServiceWorker()
        {
            listRF = new List<RF680R>();

            RegistryKey cu = Registry.CurrentUser;

            software = cu.OpenSubKey("Software", true);
            rfidKey = software.CreateSubKey("RFID", true);

            argsKey = rfidKey.CreateSubKey("args", true);
            configData = rfidKey.OpenSubKey("config", true);
        }

        public void startScan()
        {
            int num = (int)argsKey.GetValue("num");
            listRF[num].startScan(rfidKey);
        }
        public void stopScan()
        {
            int num = (int)argsKey.GetValue("num");
            listRF[0].stopScan(rfidKey);
        }

        public void init()
        {
            cRFID = (int)rfidKey.GetValue("num");
            List<string> ip_list = new List<string>(configData.GetValue("ipPort") as string[]);

            for(int i=0; i<ip_list.Count; i++)
            {
                listRF.Add(new RF680R(ip_list[i]));
            }
            
            foreach(var rfid in listRF)
            {
                rfid.connectOPC(rfidKey);
                rfid.startScan(rfidKey);
            }
        }

        public void destroy()
        {
            foreach (var rfid in listRF)
            {
                rfid.stopScan(rfidKey);
            }

            rfidKey.Close();
            software.Close();
        }

        

    }

}
