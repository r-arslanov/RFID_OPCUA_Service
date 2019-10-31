using Microsoft.Win32;
using Opc.Ua;
using Opc.Ua.Client;
using RfidOpcUaForm;
using Siemens.UAClientHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RFID_OPCUA_Service.AdditionalClasses
{
    
    class RF680R
    {
        #region Fields
        private UAClientHelperAPI myHelperApi;
        private UARfidMethodIdentifiers myRfidMethodIdentifiers;

        private Subscription mySubscription;
        private XmlDocument myDoc;

        private string url;
        RegistryKey rfidKey;
        ushort rfidNamespaceIdx;
        #endregion

        #region Constructor
        public RF680R(string url)
        {
            RegistryKey cu = Registry.CurrentUser;
            RegistryKey software = cu.OpenSubKey("Software", true);
            rfidKey = software.CreateSubKey("RFID", true);
            myHelperApi = new UAClientHelperAPI();
            this.url = url;
            rfidKey.SetValue("Debug", "APPEND OBJ " + url);
        }
        #endregion

        #region Controls
        public void connectOPC(RegistryKey rfidKey)
        {
            EndpointDescription eds = new EndpointDescription("opc.tcp://" + url + "/");
            try
            {
                myHelperApi.Connect(eds, true, false, "", "").Wait();
                rfidKey.SetValue("Debug", url + " | Success connected");
            }
            catch
            {
                rfidKey.SetValue("Debug", url + " | Error connected");
            }

            if (myHelperApi.Session != null && myHelperApi.Session.Connected)
            {
                myRfidMethodIdentifiers = new UARfidMethodIdentifiers(myHelperApi);
                rfidNamespaceIdx = GetRfidNamespaceIndex();
                try
                {
                    mySubscription = myHelperApi.Subscribe(500);
                }
                catch
                {
                    rfidKey.SetValue("Debug", url + " | Error subscribe");
                }
            }


        }

        public void startScan(RegistryKey rfidKey)
        {
            NodeId met = null;
            NodeId obj = null;

            met = new NodeId(myRfidMethodIdentifiers.MethodIdList[0].Find(x => x.method == MethodToCall.ScanStart).methodNodeId, rfidNamespaceIdx);
            obj = new NodeId(myRfidMethodIdentifiers.MethodIdList[0].Find(x => x.method == MethodToCall.ScanStart).objectNodeId, rfidNamespaceIdx);

            IList<object> cs = new List<object>();
            ScanSettings scs = new ScanSettings
            {
                DataAvailable = false,
                Duration = 0D,
                Cycles = 0
            };

            try
            {
                cs = myHelperApi.Session.Call(obj, met, scs);
                rfidKey.SetValue("Debug", url + " | Success called start read");
            }
            catch
            {
                rfidKey.SetValue("Debug", url + " | Error called start read");
            }
        }

        public void stopScan(RegistryKey rfidKey)
        {
            NodeId met = new NodeId(myRfidMethodIdentifiers.MethodIdList[0].Find(x => x.method == MethodToCall.ScanStop).methodNodeId, rfidNamespaceIdx);
            NodeId obj = new NodeId(myRfidMethodIdentifiers.MethodIdList[0].Find(x => x.method == MethodToCall.ScanStop).objectNodeId, rfidNamespaceIdx);

            IList<object> cs = new List<object>();

            try
            {
                cs = myHelperApi.Session.Call(obj, met, null);
                rfidKey.SetValue("Debug", url + " | Success called stop read");
            }
            catch
            {
                rfidKey.SetValue("Debug", url + " | Error called stop read");
            }
        }
        #endregion

        #region Helper
        public byte[] ConvertStringToByteArray(string hex)
        {
            // If string is not codable caused by the length is not even add a string 0 at the front.
            if (hex.Length % 2 != 0)
            {
                return null;
            }

            int numberChars = hex.Length;

            var bytes = new byte[numberChars / 2];//create a new byteArray with the half of the length of the previous hex

            try
            {
                for (var i = 0; i < numberChars; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);//
                }
                return bytes;
            }
            catch
            {
                return null;
            }
        }

        public string ConvertByteArrayToString(byte[] arr)
        {
            StringBuilder hex = new StringBuilder(arr.Length * 2);
            foreach (byte b in arr)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public void AddEndpoint(EndpointDescription a_EndpointDesc)
        {
            bool alreadyExists = false;

            XmlNode Endpoints = myDoc.SelectSingleNode("//Endpoints");
            XmlNode newEndpoint = myDoc.CreateNode(XmlNodeType.Element, "Endpoint", null);
            XmlNode url = myDoc.CreateNode(XmlNodeType.Element, "url", null);
            XmlNode security = myDoc.CreateNode(XmlNodeType.Element, "securityMode", null);
            XmlNode serverName = myDoc.CreateNode(XmlNodeType.Element, "serverName", null);

            url.InnerText = a_EndpointDesc.EndpointUrl;
            string[] policy = a_EndpointDesc.SecurityPolicyUri.Split('#');
            security.InnerText = policy[1] + "-" + a_EndpointDesc.SecurityMode.ToString();
            serverName.InnerText = a_EndpointDesc.Server.ApplicationName.ToString();

            if (Endpoints.HasChildNodes)
            {
                XmlNodeList children = Endpoints.SelectNodes("//Endpoint");
                foreach (XmlNode node in children)
                {
                    if (node.SelectSingleNode("url").InnerText == url.InnerText)
                    {
                        if (node.SelectSingleNode("securityMode").InnerText == security.InnerText)
                        {
                            alreadyExists = true;
                        }
                    }
                }

                if (!alreadyExists)
                {
                    if (children.Count > 10)
                    {
                        Endpoints.RemoveChild(Endpoints.FirstChild);
                    }

                    newEndpoint.InsertAfter(url, null);
                    newEndpoint.InsertAfter(security, null);
                    newEndpoint.InsertAfter(serverName, null);
                    Endpoints.InsertAfter(newEndpoint, null);
                    myDoc.Save("Endpoints.xml");
                }
            }
            else
            {
                newEndpoint.InsertAfter(url, null);
                newEndpoint.InsertAfter(security, null);
                newEndpoint.InsertAfter(serverName, null);
                Endpoints.InsertAfter(newEndpoint, null);
                myDoc.Save("Endpoints.xml");
            }
        }

        private ushort GetRfidNamespaceIndex()
        {
            ushort nameSpaceIndex = 0;

            ReferenceDescriptionCollection refDescCol = new ReferenceDescriptionCollection();
            refDescCol = myHelperApi.BrowseRoot();

            //Browse to variable "AutoIdModelVersion" (mandatory in AutoID) in RfidReaderDeviceType object to find out namespace
            foreach (ReferenceDescription refDescA in refDescCol)
            {
                if (refDescA.BrowseName.Name == "Objects")
                {
                    refDescCol = myHelperApi.BrowseNode(refDescA);
                    foreach (ReferenceDescription refDescB in refDescCol)
                    {
                        if (refDescB.BrowseName.Name == "DeviceSet")
                        {
                            refDescCol = myHelperApi.BrowseNode(refDescB);
                            foreach (ReferenceDescription refDescC in refDescCol)
                            {
                                if (refDescC.TypeDefinition == new ExpandedNodeId(RfidOpcUaForm.AutoID.ObjectTypes.RfidReaderDeviceType, (ushort)myHelperApi.GetNamespaceIndex(RfidOpcUaForm.AutoID.Namespaces.AutoID)))
                                {
                                    refDescCol = myHelperApi.BrowseNode(refDescC);
                                    foreach (ReferenceDescription refDescD in refDescCol)
                                    {
                                        if (refDescD.BrowseName.Name == "AutoIdModelVersion")
                                        {
                                            nameSpaceIndex = refDescD.NodeId.NamespaceIndex;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }

            return nameSpaceIndex;
        }
        #endregion

    }
}
