using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Runtime.Serialization;

[Serializable]
public class SceneCfg : ISerializable
{
    public uint uSceneId;
    public string strSceneName;
    public uint uTerrainId;
    public uint uSkyId;
    public uint uWeatherId;

    public SceneCfg()
    {

    }
    protected SceneCfg(SerializationInfo info, StreamingContext context)
    {
        uSceneId = info.GetUInt32("SceneId");
        strSceneName = info.GetString("SceneName");
        uTerrainId = info.GetUInt32("TerrainId");
        uSkyId = info.GetUInt32("SkyId");
        uWeatherId = info.GetUInt32("WeatherId");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("SceneId", uSceneId);
        info.AddValue("SceneName", strSceneName);
        info.AddValue("TerrainId", uTerrainId);
        info.AddValue("SkyId", uSkyId);
        info.AddValue("WeatherId", uWeatherId);
    }
}

public class SceneCfgConfigProvider : IConfigProvider
{
    enum SceneCfgColumn
    {
        SceneCfgColumn_SceneId = 0,
        SceneCfgColumn_SceneName = 1,
        SceneCfgColumn_TerrainId = 2,
        SceneCfgColumn_SkyId = 3,
        SceneCfgColumn_WeatherId = 4,
    };

    private Dictionary<uint, SceneCfg> m_dictData = new Dictionary<uint, SceneCfg>();

    public override string ConfigProvidePath
    {
        get { return ""; }
    }
    public override string ConfigProvideName
    {
        get { return "SceneCfg"; }
    }
    public override uint ResId
    {
        get { return 5002; }
    }

    public SceneCfg GetSceneCfg(uint uResId)
    {
        SceneCfg resInfo;
        if (m_dictData.TryGetValue(uResId, out resInfo))
        {
            return resInfo;
        }

        return null;
    }

    public override bool LoadBinaryFile(StreamWrapper stream)
    {
        BinaryFormatter binFormat = new BinaryFormatter();
        m_dictData = (Dictionary<uint, SceneCfg>)binFormat.Deserialize(stream.GetStream());
        return true;
    }

    public override bool LoadTextFile(ConfigFile file)
    {
        int iRowCount = file.GetRow();
        for (int iRow = 0; iRow < iRowCount; ++iRow )
        {
            SceneCfg cfgScene = new SceneCfg();
            cfgScene.uSceneId = file.GetUIntData(iRow, (int)SceneCfgColumn.SceneCfgColumn_SceneId);
            cfgScene.strSceneName = file.GetContent(iRow, (int)SceneCfgColumn.SceneCfgColumn_SceneName);
            cfgScene.uTerrainId = file.GetUIntData(iRow, (int)SceneCfgColumn.SceneCfgColumn_TerrainId);
            cfgScene.uSkyId = file.GetUIntData(iRow, (int)SceneCfgColumn.SceneCfgColumn_SkyId);
            cfgScene.uWeatherId = file.GetUIntData(iRow, (int)SceneCfgColumn.SceneCfgColumn_WeatherId);

            m_dictData.Add(cfgScene.uSceneId, cfgScene);
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