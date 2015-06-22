using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Runtime.Serialization;

public enum EquipType
{
    EquipType_RHandWeapon,
    EquipType_LHandShield,
    EquipType_LArmShield,
}

[Serializable]
public class EquipCfg : ISerializable
{
    public uint uEquipId;
    public uint uResId;
    public EquipType eEquipType;

    public EquipCfg()
    {

    }
    protected EquipCfg(SerializationInfo info, StreamingContext context)
    {
        uEquipId = info.GetUInt32("EquipId");
        uResId = info.GetUInt32("ResId");
        eEquipType = (EquipType)info.GetInt32("EquipType");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("EquipId", uEquipId);
        info.AddValue("ResId", uResId);
        info.AddValue("EquipType", (int)eEquipType);
    }
}

public class EquipCfgConfigProvider : IConfigProvider
{
    private Dictionary<uint, EquipCfg> m_dictData = new Dictionary<uint, EquipCfg>();

    public override string ConfigProvidePath
    {
        get { return ""; }
    }
    //public override string ConfigProvideName
    //{
    //    get { return "EquipCfg"; }
    //}
    public override uint ResId
    {
        get { return 5003; }
    }

    public EquipCfg GetEquipCfg(uint uResId)
    {
        EquipCfg resInfo;
        if (m_dictData.TryGetValue(uResId, out resInfo))
        {
            return resInfo;
        }

        return null;
    }

    public override bool LoadBinaryFile(StreamWrapper stream)
    {
        BinaryFormatter binFormat = new BinaryFormatter();
        m_dictData = (Dictionary<uint, EquipCfg>)binFormat.Deserialize(stream.GetStream());
        return true;
    }

    public override bool LoadTextFile(ConfigFile file)
    {
        int iRowCount = file.GetRow();
        for (int iRow = 0; iRow < iRowCount; ++iRow )
        {
            EquipCfg cfgEquip = new EquipCfg();
            cfgEquip.uEquipId = file.GetUIntData(iRow, file.GetColumnIdxByName("EquipId"));
            cfgEquip.uResId = file.GetUIntData(iRow, file.GetColumnIdxByName("ResId"));
            cfgEquip.eEquipType = (EquipType)file.GetIntData(iRow, file.GetColumnIdxByName("EquipType"));

            m_dictData.Add(cfgEquip.uEquipId, cfgEquip);
        }

        return true;
    }

    public override bool GenerateBinaryFile(StreamWrapper stream)
    {
        BinaryFormatter binFormat = new BinaryFormatter();
        binFormat.Serialize(stream.GetStream(), m_dictData);

        return true;
    }
}