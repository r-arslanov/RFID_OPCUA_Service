using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Microsoft.Win32;
using Opc.Ua.Client;
using Siemens.UAClientHelper;
using Opc.Ua;
using RfidOpcUaForm;
using System.Xml;

namespace RFID_OPCUA_Service.AdditionalClasses
{
    class ServiceWorker
    {
        #region Fields
        int cRFID;
        RegistryKey rfidKey;
        RegistryKey software;
        RegistryKey configData;

        List<RF680R> listRF;
        #endregion

        public ServiceWorker()
        {
            RegistryKey cu = Registry.CurrentUser;
            software = cu.OpenSubKey("Software", true);
            rfidKey = software.CreateSubKey("RFID", true);
            configData = rfidKey.OpenSubKey("config", true);
            listRF = new List<RF680R>();

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
