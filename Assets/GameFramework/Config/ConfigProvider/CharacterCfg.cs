using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Runtime.Serialization;

[Serializable]
public class CharacterCfg : ISerializable
{
    public uint uCharacterId;
    public uint uResId;

    public CharacterCfg()
    {

    }
    protected CharacterCfg(SerializationInfo info, StreamingContext context)
    {
        uCharacterId = info.GetUInt32("CharacterId");
        uResId = info.GetUInt32("ResId");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("CharacterId", uCharacterId);
        info.AddValue("ResId", uResId);
    }
}

public class CharacterCfgConfigProvider : IConfigProvider
{
    private Dictionary<uint, CharacterCfg> m_dictData = new Dictionary<uint, CharacterCfg>();

    public override string ConfigProvidePath
    {
        get { return ""; }
    }
    public override uint ResId
    {
        get { return 5004; }
    }

    public CharacterCfg GetEquipCfg(uint uResId)
    {
        CharacterCfg resInfo;
        if (m_dictData.TryGetValue(uResId, out resInfo))
        {
            return resInfo;
        }

        return null;
    }

    public override bool LoadBinaryFile(StreamWrapper stream)
    {
        BinaryFormatter binFormat = new BinaryFormatter();
        m_dictData = (Dictionary<uint, CharacterCfg>)binFormat.Deserialize(stream.GetStream());
        return true;
    }

    public override bool LoadTextFile(ConfigFile file)
    {
        int iRowCount = file.GetRow();
        for (int iRow = 0; iRow < iRowCount; ++iRow )
        {
            CharacterCfg cfgCharacter = new CharacterCfg();
            cfgCharacter.uCharacterId = file.GetUIntData(iRow, file.GetColumnIdxByName("CharacterId"));
            cfgCharacter.uResId = file.GetUIntData(iRow, file.GetColumnIdxByName("ResId"));

            m_dictData.Add(cfgCharacter.uCharacterId, cfgCharacter);
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