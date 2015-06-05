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

    public CResourceInfo()
    {

    }
    protected CResourceInfo(SerializationInfo info, StreamingContext context)
    {
        uResId = info.GetUInt32("ResId");
        strResName = info.GetString("ResName");
        eMaintainType = (ResourceMaintainType)info.GetInt32("MaintainType");
        iCacheTime = info.GetInt32("CacheTime");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("ResId", uResId);
        info.AddValue("ResName", strResName);
        info.AddValue("MaintainType", (int)eMaintainType);
        info.AddValue("CacheTime", iCacheTime);
    }
}

public class ResourceInfoConfigProvider : IConfigProvider
{
    enum ResourceInfoColumn
    {
        ResourceInfoColumn_ResId = 0,
        ResourceInfoColumn_ResName = 1,
        ResourceInfoColumn_MaintainType = 2,
        ResourceInfoColumn_CacheTime = 3,
        ResourceInfoColumn_ResPath = 4,
    };

    private Dictionary<uint, CResourceInfo> m_dictData = new Dictionary<uint, CResourceInfo>();

    public override string ConfigProvidePath
    {
        get { return ""; }
    }
    public override string ConfigProvideName
    {
        get { return "ResourceInfo"; }
    }
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
        for (int iRow = 2; iRow < iRowCount; ++iRow )
        {
            CResourceInfo resInfo = new CResourceInfo();
            resInfo.uResId = file.GetUIntData(iRow, (int)ResourceInfoColumn.ResourceInfoColumn_ResId);
            resInfo.strResName = file.GetContent(iRow, (int)ResourceInfoColumn.ResourceInfoColumn_ResName);
            resInfo.eMaintainType = (ResourceMaintainType)file.GetIntData(iRow, (int)ResourceInfoColumn.ResourceInfoColumn_MaintainType);
            resInfo.iCacheTime = file.GetIntData(iRow, (int)ResourceInfoColumn.ResourceInfoColumn_CacheTime);
            resInfo.strResPath = file.GetContent(iRow, (int)ResourceInfoColumn.ResourceInfoColumn_ResPath);

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