//=============================================================================
// Siemens AG
// (c)Copyright (2019) All Rights Reserved
//----------------------------------------------------------------------------- 
// Tested with: Windows 10 Enterprise x64
// Engineering: Visual Studio 2017
// Version: 1.0
//=============================================================================

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Opc.Ua;

namespace RfidOpcUaForm
{
    public class ScanSettings : EncodeableObject
    {
        #region Union Type
        private enum ScanSettingsSet
        {
            LocationType = 1,
        }
        #endregion

        #region Constructors
        public ScanSettings()
        {
            Initialize();
        }

        private void Initialize()
        {
            EncodingMask = 0;
            Duration = 0.0;
            Cycles = 0;
            DataAvailable = false;
            LocationType = null;
        }
        #endregion

        #region Encodable Members
        public override ExpandedNodeId TypeId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.DataTypes.ScanSettings);
        public override ExpandedNodeId BinaryEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID +  ";i=" + AutoID.Objects.ScanSettings_Encoding_DefaultBinary);
        public override ExpandedNodeId XmlEncodingId => NodeId.Null;

        public override void Encode(IEncoder encoder)
        {
            encoder.WriteUInt32("EncodingMask", EncodingMask);
            encoder.WriteDouble("Duration", Duration);
            encoder.WriteInt32("Cycles", Cycles);
            encoder.WriteBoolean("DataAvailable", DataAvailable);
            if (LocationType == null)
            {
                LocationType = LocationTypeEnumeration.LOCAL;
            }
            encoder.WriteEnumerated("LocationType", LocationType);
        }
        #endregion

        #region Public Properties
        public uint EncodingMask { get; set; }
        public double Duration { get; set; }
        public int Cycles { get; set; }
        public bool DataAvailable { get; set; }
        public LocationTypeEnumeration? LocationType
        {
            get
            {
                return m_LocationType;
            }
            set
            {
                m_LocationType = value;
                EncodingMask |= (uint)ScanSettingsSet.LocationType;
            }
        }
        #endregion

        #region Private Fields
        private LocationTypeEnumeration? m_LocationType;
        #endregion
    }

    public class ScanResult : EncodeableObject
    {
        #region Union Type
        private enum ScanResultSet
        {
            Location = 1,
        }
        #endregion

        #region Constructors
        public ScanResult()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            m_encodingMask = 0;
            m_CodeType = null;
            m_ScanData = null;
            m_Timestamp = DateTime.MinValue;
            m_Location = null;
        }
        #endregion

        #region Encodable Members
        public override ExpandedNodeId TypeId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.DataTypes.ScanResult);
        public override ExpandedNodeId BinaryEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.ScanResult_Encoding_DefaultBinary);
        public override ExpandedNodeId XmlEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.ScanResult_Encoding_DefaultXml);
        public override void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(AutoID.Namespaces.AutoID);
            m_encodingMask = decoder.ReadUInt32("EncodingMask");
            CodeType = decoder.ReadString("CodeType");
            ScanData = (ScanData)decoder.ReadEncodeable("ScanData", typeof(ScanData));
            Timestamp = decoder.ReadDateTime("Timestamp");
            if (IsLocationSet())
            {
                Location = (Location)decoder.ReadEncodeable("Location", typeof(Location));
            }
            decoder.PopNamespace();
        }
        #endregion

        #region Public Properties
        private uint EncodingMask
        {
            get
            {
                return m_encodingMask;
            }
            set
            {
                m_encodingMask = value;
            }
        }
        public string CodeType
        {
            get
            {
                return m_CodeType;
            }
            set
            {
                m_CodeType = value;
            }
        }
        public ScanData ScanData
        {
            get
            {
                return m_ScanData;
            }
            set
            {
                m_ScanData = value;
            }
        }
        public DateTime Timestamp
        {
            get
            {
                return m_Timestamp;
            }
            set
            {
                m_Timestamp = value;
            }
        }
        public Location Location
        {
            get
            {
                return m_Location;
            }
            set
            {
                m_Location = value;
                m_encodingMask |= (uint)ScanResultSet.Location;
            }
        }
        public bool IsLocationSet()
        {
            return (m_encodingMask & (uint)ScanResultSet.Location) != 0;
        }

        public void UnsetLocation()
        {
            m_encodingMask &= (~((uint)ScanResultSet.Location));
            m_Location = null;
        }
        #endregion

        #region Private Fields
        protected uint m_encodingMask;
        private string m_CodeType;
        private ScanData m_ScanData;
        private DateTime m_Timestamp;
        private Location m_Location;
        #endregion
    }

    public class RfidScanResult : ScanResult
    {
        #region Constructors
        public RfidScanResult()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_Sighting = new RfidSightingCollection();
        }
        #endregion

        #region Public Properties
        public RfidSightingCollection Sighting
        {
            get
            {
                return m_Sighting;
            }
            set
            {
                m_Sighting = value;

                if (value == null)
                {
                    m_Sighting = new RfidSightingCollection();
                }
            }
        }
        #endregion

        #region Encodeable Members
        public override ExpandedNodeId TypeId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.DataTypes.RfidScanResult);
        public override ExpandedNodeId BinaryEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.RfidScanResult_Encoding_DefaultBinary);
        public override ExpandedNodeId XmlEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.RfidScanResult_Encoding_DefaultXml);

        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            decoder.PushNamespace(AutoID.Namespaces.AutoID);
            Sighting = (RfidSightingCollection)decoder.ReadEncodeableArray("Sighting", typeof(RfidSighting));

            decoder.PopNamespace();
        }
        #endregion

        #region Private Fields
        private RfidSightingCollection m_Sighting;
        #endregion
    }

    public class RfidScanResultCollection : List<RfidScanResult>
    {
        #region Constructors
        /// <summary>
        /// Initializes the collection with default values.
        /// </summary>
        public RfidScanResultCollection() { }

        /// <summary>
        /// Initializes the collection with an initial capacity.
        /// </summary>
        public RfidScanResultCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes the collection with another collection.
        /// </summary>
        public RfidScanResultCollection(IEnumerable<RfidScanResult> collection) : base(collection) { }
        #endregion

        #region Static Operators
        /// <summary>
        /// Converts an array to a collection.
        /// </summary>
        public static implicit operator RfidScanResultCollection(RfidScanResult[] values)
        {
            if (values != null)
            {
                return new RfidScanResultCollection(values);
            }

            return new RfidScanResultCollection();
        }

        /// <summary>
        /// Converts a collection to an array.
        /// </summary>
        public static explicit operator RfidScanResult[] (RfidScanResultCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion
    }
    
    public class RfidSighting : EncodeableObject
    {
        #region Constructors
        public RfidSighting()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_Antenna = (int)0;
            m_Strength = (int)0;
            m_Timestamp = DateTime.MinValue;
            m_CurrentPowerLevel = (int)0;
        }
        #endregion

        #region Public Properties
        public int Antenna
        {
            get
            {
                return m_Antenna;
            }
            set
            {
                m_Antenna = value;
            }
        }
        public int Strength
        {
            get
            {
                return m_Strength;
            }
            set
            {
                m_Strength = value;
            }
        }
        public DateTime Timestamp
        {
            get
            {
                return m_Timestamp;
            }
            set
            {
                m_Timestamp = value;
            }
        }
        public int CurrentPowerLevel
        {
            get
            {
                return m_CurrentPowerLevel;
            }
            set
            {
                m_CurrentPowerLevel = value;
            }
        }
        #endregion

        #region Encodeable Members
        public override ExpandedNodeId TypeId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.DataTypes.RfidSighting);
        public override ExpandedNodeId BinaryEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.RfidSighting_Encoding_DefaultBinary);
        public override ExpandedNodeId XmlEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.RfidSighting_Encoding_DefaultXml);

        public override void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(AutoID.Namespaces.AutoID);

            encoder.WriteInt32("Antenna", Antenna);
            encoder.WriteInt32("Strength", Strength);
            encoder.WriteDateTime("Timestamp", Timestamp);
            encoder.WriteInt32("CurrentPowerLevel", CurrentPowerLevel);

            encoder.PopNamespace();
        }

        public override void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);
            Antenna = decoder.ReadInt32("Antenna");
            Strength = decoder.ReadInt32("Strength");
            Timestamp = decoder.ReadDateTime("Timestamp");
            CurrentPowerLevel = decoder.ReadInt32("CurrentPowerLevel");

            decoder.PopNamespace();
        }
        #endregion

        #region Private Fields
        private int m_Antenna;
        private int m_Strength;
        private DateTime m_Timestamp;
        private int m_CurrentPowerLevel;
        #endregion
    }

    public class RfidSightingCollection : List<RfidSighting>
    {
        #region Constructors
        /// <summary>
        /// Initializes the collection with default values.
        /// </summary>
        public RfidSightingCollection() { }

        /// <summary>
        /// Initializes the collection with an initial capacity.
        /// </summary>
        public RfidSightingCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes the collection with another collection.
        /// </summary>
        public RfidSightingCollection(IEnumerable<RfidSighting> collection) : base(collection) { }
        #endregion

        #region Static Operators
        /// <summary>
        /// Converts an array to a collection.
        /// </summary>
        public static implicit operator RfidSightingCollection(RfidSighting[] values)
        {
            if (values != null)
            {
                return new RfidSightingCollection(values);
            }

            return new RfidSightingCollection();
        }

        /// <summary>
        /// Converts a collection to an array.
        /// </summary>
        public static explicit operator RfidSighting[] (RfidSightingCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion
    }

    public class ScanData : EncodeableObject
    {
        #region Union Type
        public enum ScanDataType
        {
            Null = 0,
            ByteString = 1,
            String = 2,
            Epc = 3,
            Custom = 4,
        }
        #endregion

        #region Constructors
        public ScanData()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_switchField = ScanDataType.Null;
            m_value = null;
        }
        #endregion

        #region Public Properties
        private uint SwitchField
        {
            get
            {
                return (uint)m_switchField;
            }
            set
            {
                m_switchField = (ScanDataType)value;
            }
        }

        public ScanDataType Type
        {
            get { return m_switchField; }
        }

        public object Value
        {
            get { return m_value; }
        }

        public byte[] ByteString
        {
            get
            {
                if (m_switchField == ScanDataType.ByteString)
                {
                    return (byte[])m_value;
                }
                return null;
            }
            set
            {
                m_value = value;
                m_switchField = ScanDataType.ByteString;
            }
        }

        public string String
        {
            get
            {
                if (m_switchField == ScanDataType.String)
                {
                    return (string)m_value;
                }
                return null;
            }
            set
            {
                m_value = value;
                m_switchField = ScanDataType.String;
            }
        }

        public ScanDataEpc Epc
        {
            get
            {
                if (m_switchField == ScanDataType.Epc)
                {
                    return (ScanDataEpc)m_value;
                }
                return null;
            }
            set
            {
                m_value = value;
                m_switchField = ScanDataType.Epc;
            }
        }

        public Variant? Custom
        {
            get
            {
                if (m_switchField == ScanDataType.Custom)
                {
                    return (Variant?)m_value;
                }
                return null;
            }
            set
            {
                m_value = value;
                m_switchField = ScanDataType.Custom;
            }
        }
        #endregion

        #region Encodeable Members
        public override ExpandedNodeId TypeId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.DataTypes.ScanData);
        public override ExpandedNodeId BinaryEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.ScanData_Encoding_DefaultBinary);
        public override ExpandedNodeId XmlEncodingId => NodeId.Null;
        public override void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);

            encoder.WriteUInt32("SwitchField", (uint)m_switchField);

            switch (m_switchField)
            {
                case ScanDataType.Null:
                    break;
                case ScanDataType.ByteString:
                    encoder.WriteByteString("ByteString", (byte[])m_value);
                    break;
                case ScanDataType.String:
                    encoder.WriteString("String", (string)m_value);
                    break;
                case ScanDataType.Epc:
                    encoder.WriteEncodeable("Epc", (ScanDataEpc)m_value, typeof(ScanDataEpc));
                    break;
                case ScanDataType.Custom:
                    encoder.WriteVariant("Custom", (Variant)m_value);
                    break;
            }

            encoder.PopNamespace();
        }
        public override void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);

            m_switchField = (ScanDataType)decoder.ReadInt32("SwitchField");

            switch (m_switchField)
            {
                case ScanDataType.Null:
                    m_value = null;
                    break;
                case ScanDataType.ByteString:
                    m_value = decoder.ReadByteString("ByteString");
                    break;
                case ScanDataType.String:
                    m_value = decoder.ReadString("String");
                    break;
                case ScanDataType.Epc:
                    m_value = decoder.ReadEncodeable("Epc", typeof(ScanDataEpc));
                    break;
                case ScanDataType.Custom:
                    m_value = decoder.ReadVariant("Custom");
                    break;
            }
            decoder.PopNamespace();
        }
        #endregion

        #region Private Fields
        private ScanDataType m_switchField;
        private object m_value;
        #endregion
    }

    public class Location : EncodeableObject
    {
        #region Union Type
        public enum LocationType
        {
            Null = 0,
            NMEA = 1,
            Local = 2,
            WGS84 = 3,
            Name = 4,
        }
        #endregion

        #region Constructors
        public Location()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_switchField = LocationType.Null;
            m_value = null;
        }
        #endregion

        #region Public Properties
        private uint SwitchField
        {
            get
            {
                return (uint)m_switchField;
            }
            set
            {
                m_switchField = (LocationType)value;
            }
        }

        public LocationType Type
        {
            get { return m_switchField; }
        }

        public object Value
        {
            get { return m_value; }
        }

        public string NMEA
        {
            get
            {
                if (m_switchField == LocationType.NMEA)
                {
                    return (string)m_value;
                }
                return null;
            }
            set
            {
                m_value = value;
                m_switchField = LocationType.NMEA;
            }
        }

        public LocalCoordinate Local
        {
            get
            {
                if (m_switchField == LocationType.Local)
                {
                    return (LocalCoordinate)m_value;
                }
                return null;
            }
            set
            {
                m_value = value;
                m_switchField = LocationType.Local;
            }
        }

        public WGS84Coordinate WGS84
        {
            get
            {
                if (m_switchField == LocationType.WGS84)
                {
                    return (WGS84Coordinate)m_value;
                }
                return null;
            }
            set
            {
                m_value = value;
                m_switchField = LocationType.WGS84;
            }
        }

        public string Name
        {
            get
            {
                if (m_switchField == LocationType.Name)
                {
                    return (string)m_value;
                }
                return null;
            }
            set
            {
                m_value = value;
                m_switchField = LocationType.Name;
            }
        }
        #endregion

        #region Encodeable Members
        public override ExpandedNodeId TypeId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.DataTypes.Location);
        public override ExpandedNodeId BinaryEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.Location_Encoding_DefaultBinary);
        public override ExpandedNodeId XmlEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.Location_Encoding_DefaultXml);
        public override void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);

            encoder.WriteUInt32("SwitchField", (uint)m_switchField);

            switch (m_switchField)
            {
                case LocationType.Null:
                    break;
                case LocationType.NMEA:
                    encoder.WriteString("NMEA", (string)m_value);
                    break;
                case LocationType.Local:
                    encoder.WriteEncodeable("Local", (LocalCoordinate)m_value, typeof(LocalCoordinate));
                    break;
                case LocationType.WGS84:
                    encoder.WriteEncodeable("WGS84", (WGS84Coordinate)m_value, typeof(WGS84Coordinate));
                    break;
                case LocationType.Name:
                    encoder.WriteString("Name", (string)m_value);
                    break;
            }

            encoder.PopNamespace();
        }
        public override void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);

            m_switchField = (LocationType)decoder.ReadInt32("SwitchField");

            switch (m_switchField)
            {
                case LocationType.Null:
                    m_value = null;
                    break;
                case LocationType.NMEA:
                    m_value = decoder.ReadString("NMEA");
                    break;
                case LocationType.Local:
                    m_value = decoder.ReadEncodeable("Local", typeof(LocalCoordinate));
                    break;
                case LocationType.WGS84:
                    m_value = decoder.ReadEncodeable("WGS84", typeof(WGS84Coordinate));
                    break;
                case LocationType.Name:
                    m_value = decoder.ReadString("Name");
                    break;
            }
            decoder.PopNamespace();
        }
        #endregion

        #region Private Fields
        private LocationType m_switchField;
        private object m_value;
        #endregion
    }

    public class ScanDataEpc : EncodeableObject
    {
        #region Constructors
        public ScanDataEpc()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_PC = (ushort)0;
            m_UId = null;
            m_XPC_W1 = (ushort)0;
            m_XPC_W2 = (ushort)0;
        }
        #endregion

        #region Public Properties
        public ushort PC
        {
            get
            {
                return m_PC;
            }
            set
            {
                m_PC = value;
            }
        }
        public byte[] UId
        {
            get
            {
                return m_UId;
            }
            set
            {
                m_UId = value;
            }
        }
        public ushort XPC_W1
        {
            get
            {
                return m_XPC_W1;
            }
            set
            {
                m_XPC_W1 = value;
            }
        }
        public ushort XPC_W2
        {
            get
            {
                return m_XPC_W2;
            }
            set
            {
                m_XPC_W2 = value;
            }
        }
        #endregion

        #region Encodeable Members
        public override ExpandedNodeId TypeId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.DataTypes.ScanDataEpc);
        public override ExpandedNodeId BinaryEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.ScanDataEpc_Encoding_DefaultBinary);
        public override ExpandedNodeId XmlEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.Objects.ScanDataEpc_Encoding_DefaultXml);

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);

            encoder.WriteUInt16("PC", PC);
            encoder.WriteByteString("UId", UId);
            encoder.WriteUInt16("XPC_W1", XPC_W1);
            encoder.WriteUInt16("XPC_W2", XPC_W2);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);
            PC = decoder.ReadUInt16("PC");
            UId = decoder.ReadByteString("UId");
            XPC_W1 = decoder.ReadUInt16("XPC_W1");
            XPC_W2 = decoder.ReadUInt16("XPC_W2");

            decoder.PopNamespace();
        }
        #endregion

        #region Private Fields
        private ushort m_PC;
        private byte[] m_UId;
        private ushort m_XPC_W1;
        private ushort m_XPC_W2;
        #endregion
    }

    public class LocalCoordinate : EncodeableObject
    {
        #region Constructors
        public LocalCoordinate()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_X = 0.0;
            m_Y = 0.0;
            m_Z = 0.0;
            m_Timestamp = DateTime.MinValue;
            m_DilutionOfPrecision = 0.0;
            m_UsefulPrecicision = (int)0;
        }
        #endregion

        #region Public Properties
        public double X
        {
            get
            {
                return m_X;
            }
            set
            {
                m_X = value;
            }
        }
        public double Y
        {
            get
            {
                return m_Y;
            }
            set
            {
                m_Y = value;
            }
        }
        public double Z
        {
            get
            {
                return m_Z;
            }
            set
            {
                m_Z = value;
            }
        }
        public DateTime Timestamp
        {
            get
            {
                return m_Timestamp;
            }
            set
            {
                m_Timestamp = value;
            }
        }
        public double DilutionOfPrecision
        {
            get
            {
                return m_DilutionOfPrecision;
            }
            set
            {
                m_DilutionOfPrecision = value;
            }
        }
        public int UsefulPrecicision
        {
            get
            {
                return m_UsefulPrecicision;
            }
            set
            {
                m_UsefulPrecicision = value;
            }
        }
        #endregion

        #region IEncodeable Members


        public override ExpandedNodeId TypeId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.DataTypeIds.LocalCoordinate);
        public override ExpandedNodeId BinaryEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.ObjectIds.LocalCoordinate_Encoding_DefaultBinary);
        public override ExpandedNodeId XmlEncodingId => NodeId.Null;

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);

            encoder.WriteDouble("X", X);
            encoder.WriteDouble("Y", Y);
            encoder.WriteDouble("Z", Z);
            encoder.WriteDateTime("Timestamp", Timestamp);
            encoder.WriteDouble("DilutionOfPrecision", DilutionOfPrecision);
            encoder.WriteInt32("UsefulPrecicision", UsefulPrecicision);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);
            X = decoder.ReadDouble("X");
            Y = decoder.ReadDouble("Y");
            Z = decoder.ReadDouble("Z");
            Timestamp = decoder.ReadDateTime("Timestamp");
            DilutionOfPrecision = decoder.ReadDouble("DilutionOfPrecision");
            UsefulPrecicision = decoder.ReadInt32("UsefulPrecicision");

            decoder.PopNamespace();
        }
        #endregion

        #region Private Fields
        private double m_X;
        private double m_Y;
        private double m_Z;
        private DateTime m_Timestamp;
        private double m_DilutionOfPrecision;
        private int m_UsefulPrecicision;
        #endregion
    }

    public class WGS84Coordinate : EncodeableObject
    {
        #region Constructors
        public WGS84Coordinate()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_N_S_Hemisphere = null;
            m_Latitude = 0.0;
            m_E_W_Hemisphere = null;
            m_Longitude = 0.0;
            m_Altitude = 0.0;
            m_Timestamp = DateTime.MinValue;
            m_DilutionOfPrecision = 0.0;
            m_UsefulPrecisionLatLon = (int)0;
            m_UsefulPrecisionAlt = (int)0;
        }
        #endregion

        #region Public Properties
        public string N_S_Hemisphere
        {
            get
            {
                return m_N_S_Hemisphere;
            }
            set
            {
                m_N_S_Hemisphere = value;
            }
        }
        public double Latitude
        {
            get
            {
                return m_Latitude;
            }
            set
            {
                m_Latitude = value;
            }
        }
        public string E_W_Hemisphere
        {
            get
            {
                return m_E_W_Hemisphere;
            }
            set
            {
                m_E_W_Hemisphere = value;
            }
        }
        public double Longitude
        {
            get
            {
                return m_Longitude;
            }
            set
            {
                m_Longitude = value;
            }
        }
        public double Altitude
        {
            get
            {
                return m_Altitude;
            }
            set
            {
                m_Altitude = value;
            }
        }
        public DateTime Timestamp
        {
            get
            {
                return m_Timestamp;
            }
            set
            {
                m_Timestamp = value;
            }
        }
        public double DilutionOfPrecision
        {
            get
            {
                return m_DilutionOfPrecision;
            }
            set
            {
                m_DilutionOfPrecision = value;
            }
        }
        public int UsefulPrecisionLatLon
        {
            get
            {
                return m_UsefulPrecisionLatLon;
            }
            set
            {
                m_UsefulPrecisionLatLon = value;
            }
        }
        public int UsefulPrecisionAlt
        {
            get
            {
                return m_UsefulPrecisionAlt;
            }
            set
            {
                m_UsefulPrecisionAlt = value;
            }
        }
        #endregion

        #region Encodeable Members
        public override ExpandedNodeId TypeId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.DataTypeIds.WGS84Coordinate);
        public override ExpandedNodeId BinaryEncodingId => ExpandedNodeId.Parse("nsu=" + AutoID.Namespaces.AutoID + ";i=" + AutoID.ObjectIds.WGS84Coordinate_Encoding_DefaultBinary);
        public override ExpandedNodeId XmlEncodingId => NodeId.Null;

        public override void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);

            encoder.WriteString("N_S_Hemisphere", N_S_Hemisphere);
            encoder.WriteDouble("Latitude", Latitude);
            encoder.WriteString("E_W_Hemisphere", E_W_Hemisphere);
            encoder.WriteDouble("Longitude", Longitude);
            encoder.WriteDouble("Altitude", Altitude);
            encoder.WriteDateTime("Timestamp", Timestamp);
            encoder.WriteDouble("DilutionOfPrecision", DilutionOfPrecision);
            encoder.WriteInt32("UsefulPrecisionLatLon", UsefulPrecisionLatLon);
            encoder.WriteInt32("UsefulPrecisionAlt", UsefulPrecisionAlt);

            encoder.PopNamespace();
        }

        public override void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(AutoID.Namespaces.AutoIDXsd);
            N_S_Hemisphere = decoder.ReadString("N_S_Hemisphere");
            Latitude = decoder.ReadDouble("Latitude");
            E_W_Hemisphere = decoder.ReadString("E_W_Hemisphere");
            Longitude = decoder.ReadDouble("Longitude");
            Altitude = decoder.ReadDouble("Altitude");
            Timestamp = decoder.ReadDateTime("Timestamp");
            DilutionOfPrecision = decoder.ReadDouble("DilutionOfPrecision");
            UsefulPrecisionLatLon = decoder.ReadInt32("UsefulPrecisionLatLon");
            UsefulPrecisionAlt = decoder.ReadInt32("UsefulPrecisionAlt");

            decoder.PopNamespace();
        }
        #endregion

        #region Private Fields
        private string m_N_S_Hemisphere;
        private double m_Latitude;
        private string m_E_W_Hemisphere;
        private double m_Longitude;
        private double m_Altitude;
        private DateTime m_Timestamp;
        private double m_DilutionOfPrecision;
        private int m_UsefulPrecisionLatLon;
        private int m_UsefulPrecisionAlt;
        #endregion
    }

    public class RfidLockOperationEnumerationCollection : List<RfidLockOperationEnumeration>
    {
        #region Constructors
        /// <summary>
        /// Initializes the collection with default values.
        /// </summary>
        public RfidLockOperationEnumerationCollection() { }

        /// <summary>
        /// Initializes the collection with an initial capacity.
        /// </summary>
        public RfidLockOperationEnumerationCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes the collection with another collection.
        /// </summary>
        public RfidLockOperationEnumerationCollection(IEnumerable<RfidLockOperationEnumeration> collection) : base(collection) { }
        #endregion

        #region Static Operators
        /// <summary>
        /// Converts an array to a collection.
        /// </summary>
        public static implicit operator RfidLockOperationEnumerationCollection(RfidLockOperationEnumeration[] values)
        {
            if (values != null)
            {
                return new RfidLockOperationEnumerationCollection(values);
            }

            return new RfidLockOperationEnumerationCollection();
        }

        /// <summary>
        /// Converts a collection to an array.
        /// </summary>
        public static explicit operator RfidLockOperationEnumeration[] (RfidLockOperationEnumerationCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion
    }

    public class RfidLockRegionEnumerationCollection : List<RfidLockRegionEnumeration>
    {
        #region Constructors
        /// <summary>
        /// Initializes the collection with default values.
        /// </summary>
        public RfidLockRegionEnumerationCollection() { }

        /// <summary>
        /// Initializes the collection with an initial capacity.
        /// </summary>
        public RfidLockRegionEnumerationCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes the collection with another collection.
        /// </summary>
        public RfidLockRegionEnumerationCollection(IEnumerable<RfidLockRegionEnumeration> collection) : base(collection) { }
        #endregion

        #region Static Operators
        /// <summary>
        /// Converts an array to a collection.
        /// </summary>
        public static implicit operator RfidLockRegionEnumerationCollection(RfidLockRegionEnumeration[] values)
        {
            if (values != null)
            {
                return new RfidLockRegionEnumerationCollection(values);
            }
            return new RfidLockRegionEnumerationCollection();
        }

        /// <summary>
        /// Converts a collection to an array.
        /// </summary>
        public static explicit operator RfidLockRegionEnumeration[] (RfidLockRegionEnumerationCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion
    }

    public enum LocationTypeEnumeration
    {
        NMEA = 0,
        LOCAL = 1,
        WGS84 = 2,
        NAME = 3,
    }

    public enum AutoIdOperationStatusEnumeration
    {
        /// <summary>
        /// Successful operation
        /// </summary>
        SUCCESS = 0,
        /// <summary>
        /// The operation has not be executed in total
        /// </summary>
        MISC_ERROR_TOTAL = 1,
        /// <summary>
        /// The operation has been executed only partial
        /// </summary>
        MISC_ERROR_PARTIAL = 2,
        /// <summary>
        /// Password required
        /// </summary>
        PERMISSON_ERROR = 3,
        /// <summary>
        /// Password is wrong
        /// </summary>
        PASSWORD_ERROR = 4,
        /// <summary>
        /// Memory region not available for the actual tag
        /// </summary>
        REGION_NOT_FOUND_ERROR = 5,
        /// <summary>
        /// Operation not supported by the actual tag
        /// </summary>
        OP_NOT_POSSIBLE_ERROR = 6,
        /// <summary>
        /// Addressed memory not available for the actual tag
        /// </summary>
        OUT_OF_RANGE_ERROR = 7,
        /// <summary>
        /// The operation cannot be executed because no tag or code was inside the range of the AutoID Device or the tag or code has been moved out of the range during execution
        /// </summary>
        NO_IDENTIFIER = 8,
        /// <summary>
        /// Multiple tags or codes have been selected, but the command can only be used with a single tag or code
        /// </summary>
        MULTIPLE_IDENTIFIERS = 9,
        /// <summary>
        /// The tag or code exists and has a valid format, but there was a problem reading the data (e.g. still CRC error after maximum number of retries)
        /// </summary>
        READ_ERROR = 10,
        /// <summary>
        /// The (optical) code or plain text has too many failures and cannot be detected
        /// </summary>
        DECODING_ERROR = 11,
        /// <summary>
        /// The code doesn’t match the given target value
        /// </summary>
        MATCH_ERROR = 12,
        /// <summary>
        /// The code format is not supported by the AutoID Device
        /// </summary>
        CODE_NOT_SUPPORTED = 13,
        /// <summary>
        /// The tag exists, but there was a problem writing the data 
        /// </summary>
        WRITE_ERROR = 14,
        /// <summary>
        /// The command or a parameter combination is not supported by the AutoID Device
        /// </summary>
        NOT_SUPPORTED_BY_DEVICE = 15,
        /// <summary>
        /// The command or a parameter combination is not supported by the tag
        /// </summary>
        NOT_SUPPORTED_BY_TAG = 16,
        /// <summary>
        /// The AutoID Device is in a state not ready to execute the command
        /// </summary>
        DEVICE_NOT_READY = 17,
        /// <summary>
        /// The AutoID Device configuration is not valid
        /// </summary>
        INVALID_CONFIGURATION = 18,
        /// <summary>
        /// This error indicates that there is a general error in the communication between the transponder and the reader
        /// </summary>
        RF_COMMUNICATION_ERROR = 19,
        /// <summary>
        /// The AutoID Device has a hardware fault
        /// </summary>
        DEVICE_FAULT = 20,
        /// <summary>
        /// The battery of the (active) tag is low
        /// </summary>
        TAG_HAS_LOW_BATTERY = 21,
    }

    public enum RfidLockOperationEnumeration
    {
        Lock = 0,
        Unlock = 1,
        PermanentLock = 2,
        PermanentUnlock = 3,
    }

    public enum RfidLockRegionEnumeration
    {
        Kill = 0,
        Access = 1,
        EPC = 2,
        TID = 3,
        User = 4,
    }
}