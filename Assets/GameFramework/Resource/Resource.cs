using System.Collections.Generic;
using UnityEngine;

public enum ResourceMaintainType
{
    //ResourceMaintain_Immediate,
    ResourceMaintain_AutoRelease,
    ResourceMaintain_Manual,
}

public delegate void ResourceLoaded(CResource res);
public delegate void ResourceLoadCancel(CResource res);

public class CResource
{
    public event ResourceLoaded OnResourceLoaded;
    public event ResourceLoadCancel OnResourceLoadCancel;

    private uint m_uResId;
    public uint ResId
    {
        get { return m_uResId; }
    }

    private string m_strResName;
    public string ResourceName
    {
        get { return m_strResName; }
    }

    private string m_strResPath;
    public string ResourcePath
    {
        get { return m_strResPath; }
    }

    private AssetBundle m_AssetBundle;
    public AssetBundle AssetBundle
    {
        get { return m_AssetBundle; }
        set { m_AssetBundle = value; }
    }

    private uint m_uRequireResVer;
    public uint RequireResVer
    {
        get { return m_uRequireResVer; }
    }

    private ResourceMaintainType m_eMaintainType;
    public ResourceMaintainType MaintainType
    {
        get { return m_eMaintainType; }
    }

    private bool m_bIsResourceLoaded;
    public bool IsLoaded
    {
        get { return m_bIsResourceLoaded; }
        set { m_bIsResourceLoaded = value; }
    }

    private float m_fReleaseTime;
    public float ReleaseTime
    {
        get { return m_fReleaseTime; }
    }

    public CResource(uint uResId
                    , string strResName
                    , string strResPath
                    , uint uReqVer
                    , ResourceMaintainType eMaintainType
                    , int iCacheTime)
    {
        m_uResId = uResId;
        m_strResName = strResName;
        m_strResPath = strResPath;
        m_uRequireResVer = uReqVer;
        m_eMaintainType = eMaintainType;
        m_fReleaseTime = Time.time + (float)iCacheTime;

        IsLoaded = false;
    }

    public CResource(CResourceInfo resInfo, uint uReqVer)
    {
        m_uResId = resInfo.uResId;
        m_strResName = resInfo.strResName;
        m_strResPath = resInfo.strResPath;
        m_uRequireResVer = uReqVer;
        m_eMaintainType = resInfo.eMaintainType;
        m_fReleaseTime = Time.time + (float)resInfo.iCacheTime;

        IsLoaded = false;
    }

    public Object MainAsset
    {
        get { return m_AssetBundle.mainAsset; }
    }

    public bool UnloadResource()
    {
        m_AssetBundle.Unload(false);

        return true;
    }

    public bool CancelResourceLoading()
    {
        OnResourceLoadCancel(this);
        return true;
    }

    public void FinishResourceLoad()
    {
        OnResourceLoaded(this);
    }
    public void CancelResourceLoad()
    {
        OnResourceLoaded(this);
    }
}
