//=============================================================================
// Siemens AG
// (c)Copyright (2019) All Rights Reserved
//----------------------------------------------------------------------------- 
// Tested with: Windows 10 Enterprise x64
// Engineering: Visual Studio 2017
// Version: 1.0
//=============================================================================

using Siemens.UAClientHelper;
using Opc.Ua;
using System.Collections.Generic;

namespace RfidOpcUaForm
{
    public class UARfidMethodIdentifiers
    {
        #region Fields
        private UAClientHelperAPI myHelperApi;
        private uint methodIdentifier;
        private uint objectIdentifier;
        private List<List<MethodIds>> methodIdList;
        #endregion

        #region Constructor
        public UARfidMethodIdentifiers(UAClientHelperAPI helperApi)
        {
            myHelperApi = helperApi;            
            methodIdList = new List<List<MethodIds>>();
            methodIdList.Add(new List<MethodIds>());
            methodIdList.Add(new List<MethodIds>());
            methodIdList.Add(new List<MethodIds>());
            methodIdList.Add(new List<MethodIds>());
            GetIDsAtConstruction();
        }
        #endregion

        #region Properties
        public List<List<MethodIds>> MethodIdList
        {
            get { return methodIdList; }
        }
        #endregion

        #region PrivateMethods

        private void GetIDsAtConstruction()
        {
            ReferenceDescriptionCollection refDescCol = new ReferenceDescriptionCollection();
            ReferenceDescriptionCollection foundRefDescCol = new ReferenceDescriptionCollection();
            refDescCol = myHelperApi.BrowseRoot();

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
                                if (refDescC.TypeDefinition == new ExpandedNodeId(AutoID.ObjectTypes.RfidReaderDeviceType, (ushort)myHelperApi.GetNamespaceIndex(AutoID.Namespaces.AutoID)))
                                {
                                    foundRefDescCol.Add(refDescC);
                                }
                            }
                        }
                    }
                }
            }

            MethodIds methodIds;

            for (int i = 0; i < foundRefDescCol.Count; i++)
            {
                refDescCol = myHelperApi.BrowseNode(foundRefDescCol[i]);

                foreach (ReferenceDescription refDescD in refDescCol)
                {
                    if (refDescD.BrowseName.Name == "Scan")
                    {
                        objectIdentifier = (uint)foundRefDescCol[i].NodeId.Identifier;
                        methodIdentifier = (uint)refDescD.NodeId.Identifier;
                        methodIds.method = MethodToCall.Scan;
                        methodIds.readpoint = i;
                        methodIds.methodNodeId = methodIdentifier;
                        methodIds.objectNodeId = objectIdentifier;
                        methodIdList[i].Add(methodIds);
                    }
                    else if (refDescD.BrowseName.Name == "ScanStart")
                    {
                        objectIdentifier = (uint)foundRefDescCol[i].NodeId.Identifier;
                        methodIdentifier = (uint)refDescD.NodeId.Identifier;
                        methodIds.method = MethodToCall.ScanStart;
                        methodIds.readpoint = i;
                        methodIds.methodNodeId = methodIdentifier;
                        methodIds.objectNodeId = objectIdentifier;
                        methodIdList[i].Add(methodIds);
                    }
                    else if (refDescD.BrowseName.Name == "ScanStop")
                    {
                        objectIdentifier = (uint)foundRefDescCol[i].NodeId.Identifier;
                        methodIdentifier = (uint)refDescD.NodeId.Identifier;
                        methodIds.method = MethodToCall.ScanStop;
                        methodIds.readpoint = i;
                        methodIds.methodNodeId = methodIdentifier;
                        methodIds.objectNodeId = objectIdentifier;
                        methodIdList[i].Add(methodIds);
                    }
                    else if (refDescD.BrowseName.Name == "ReadTag")
                    {
                        objectIdentifier = (uint)foundRefDescCol[i].NodeId.Identifier;
                        methodIdentifier = (uint)refDescD.NodeId.Identifier;
                        methodIds.method = MethodToCall.ReadTag;
                        methodIds.readpoint = i;
                        methodIds.methodNodeId = methodIdentifier;
                        methodIds.objectNodeId = objectIdentifier;
                        methodIdList[i].Add(methodIds);
                    }
                    else if (refDescD.BrowseName.Name == "WriteTag")
                    {
                        objectIdentifier = (uint)foundRefDescCol[i].NodeId.Identifier;
                        methodIdentifier = (uint)refDescD.NodeId.Identifier;
                        methodIds.method = MethodToCall.WriteTag;
                        methodIds.readpoint = i;
                        methodIds.methodNodeId = methodIdentifier;
                        methodIds.objectNodeId = objectIdentifier;
                        methodIdList[i].Add(methodIds);
                    }
                    else if (refDescD.BrowseName.Name == "KillTag")
                    {
                        objectIdentifier = (uint)foundRefDescCol[i].NodeId.Identifier;
                        methodIdentifier = (uint)refDescD.NodeId.Identifier;
                        methodIds.method = MethodToCall.KillTag;
                        methodIds.readpoint = i;
                        methodIds.methodNodeId = methodIdentifier;
                        methodIds.objectNodeId = objectIdentifier;
                        methodIdList[i].Add(methodIds);
                    }
                    else if (refDescD.BrowseName.Name == "LockTag")
                    {
                        objectIdentifier = (uint)foundRefDescCol[i].NodeId.Identifier;
                        methodIdentifier = (uint)refDescD.NodeId.Identifier;
                        methodIds.method = MethodToCall.LockTag;
                        methodIds.readpoint = i;
                        methodIds.methodNodeId = methodIdentifier;
                        methodIds.objectNodeId = objectIdentifier;
                        methodIdList[i].Add(methodIds);
                    }
                    else if (refDescD.BrowseName.Name == "SetTagPassword")
                    {
                        objectIdentifier = (uint)foundRefDescCol[i].NodeId.Identifier;
                        methodIdentifier = (uint)refDescD.NodeId.Identifier;
                        methodIds.method = MethodToCall.SetTagPw;
                        methodIds.readpoint = i;
                        methodIds.methodNodeId = methodIdentifier;
                        methodIds.objectNodeId = objectIdentifier;
                        methodIdList[i].Add(methodIds);
                    }
                }
            }
        }
        #endregion
    }

    public enum MethodToCall
    {
        Scan = 1,
        ScanStart = 2,
        ScanStop = 3,
        ReadTag = 4,
        WriteTag = 5,
        KillTag = 6,
        LockTag = 7,
        SetTagPw = 8,
    }

    public struct MethodIds
    {
        public MethodToCall method;
        public int readpoint;
        public uint methodNodeId;
        public uint objectNodeId;
    }
}
