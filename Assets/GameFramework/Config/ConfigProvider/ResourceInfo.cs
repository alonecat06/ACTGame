using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Runtime.Serialization;

[Serializable]
public class CResourceInfo : ISerializable
{
    public uint uResId;
    public string strResName;
    public string strResPath;
    public ResourceMaintainType eMaintainType;
    public int iCacheTime;
    public ResourceType eResourceType;

    public CResourceInfo()
    {

    }
    protected CResourceInfo(SerializationInfo info, StreamingContext context)
    {
        uResId = info.GetUInt32("ResId");
        strResName = info.GetString("ResName");
        strResPath = info.GetString("ResPath");
        eMaintainType = (ResourceMaintainType)info.GetInt32("MaintainType");
        iCacheTime = info.GetInt32("CacheTime");
        eResourceType = (ResourceType)info.GetInt32("ResourceType");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("ResId", uResId);
        info.AddValue("ResName", strResName);
        info.AddValue("ResPath", strResPath);
        info.AddValue("MaintainType", (int)eMaintainType);
        info.AddValue("CacheTime", iCacheTime);
        info.AddValue("ResourceType", (int)eResourceType);
    }
}

public class ResourceInfoConfigProvider : IConfigProvider
{
    private Dictionary<uint, CResourceInfo> m_dictData = new Dictionary<uint, CResourceInfo>();

    public override string ConfigProvidePath
    {
        get { return ""; }
    }
    //public override string ConfigProvideName
    //{
    //    get { return "ResourceInfo"; }
    //}
    public override uint ResId
    {
        get { return 5001; }
    }

    public CResourceInfo GetResourceInfo(uint uResId)
    {
        CResourceInfo resInfo;
        if (m_dictData.TryGetValue(uResId, out resInfo))
        {
            return resInfo;
        }

        return null;
    }

    public override bool LoadBinaryFile(StreamWrapper stream)
    {
        BinaryFormatter binFormat = new BinaryFormatter();
        m_dictData = (Dictionary<uint, CResourceInfo>)binFormat.Deserialize(stream.GetStream());
        return true;
    }

    public override bool LoadTextFile(ConfigFile file)
    {
        int iRowCount = file.GetRow();
        for (int iRow = 0; iRow < iRowCount; ++iRow )
        {
            CResourceInfo resInfo = new CResourceInfo();
            resInfo.uResId = file.GetUIntData(iRow, file.GetColumnIdxByName("ResId"));
            resInfo.strResName = file.GetContent(iRow, file.GetColumnIdxByName("ResName")).ToLower();
            resInfo.eMaintainType = (ResourceMaintainType)file.GetIntData(iRow, file.GetColumnIdxByName("MaintainType"));
            resInfo.iCacheTime = file.GetIntData(iRow, file.GetColumnIdxByName("CacheTime"));
            resInfo.strResPath = file.GetContent(iRow, file.GetColumnIdxByName("ResPath"));
            resInfo.eResourceType = (ResourceType)file.GetIntData(iRow, file.GetColumnIdxByName("ResType"));

            m_dictData.Add(resInfo.uResId, resInfo);
        }

        return true;
    }

    public override bool GenerateBinaryFile(StreamWrapper stream)
    {
        BinaryFormatter binFormat = new BinaryFormatter();
        binFormat.Serialize(stream.GetStream(), m_dictData);

        return true;
    }

    public Dictionary<uint, CResourceInfo>.Enumerator GetEnumerator()
    {
        return m_dictData.GetEnumerator();
    }
}