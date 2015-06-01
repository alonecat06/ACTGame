using System.Collections.Generic;
using UnityEngine;

public enum ResourceMaintainType
{
    //ResourceMaintain_Immediate,
    ResourceMaintain_AutoRelease,
    ResourceMaintain_Manual,
}

public delegate void ResourceLoaded(Object go, uint uResId);
//public delegate void ResourceLoadCancle(uint uResId);
//public delegate void ResourceLoaded(CResource Res);
public delegate void ResourceLoadCancel(CResource Res);

public class CResource
{
    public event ResourceLoaded OnResourceLoaded;
    public event ResourceLoadCancel OnResourceLoadCancel;

    private uint m_uResId;
    public uint ResId
    {
        get { return m_uResId; }
    }

    private string m_strAssetPath;
    public string AssetPath
    {
        get { return m_strAssetPath; }
        //set { m_strAssetPath = value; }
    }

    private AssetBundle m_AssetBundle;
    public AssetBundle AssetBundle
    {
        get { return m_AssetBundle; }
        set { m_AssetBundle = value; }
    }

    private int m_iRequireResVer;
    public int RequireResVer
    {
        get { return m_iRequireResVer; }
        //set { m_iRequireResVer = value; }
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
                    , string strAssetPath
                    , int iReqVer
                    , ResourceMaintainType eMaintainType
                    , int iCacheTime)
    {
        m_uResId = uResId;
        m_strAssetPath = strAssetPath;
        m_iRequireResVer = iReqVer;
        m_eMaintainType = eMaintainType;
        m_fReleaseTime = Time.time + (float)iCacheTime;

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

    //public void FinishResourceLoad()
    //{
    //    OnResourceLoaded(this);
    //}
    //public void CancelResourceLoad()
    //{
    //    OnResourceLoaded(this);
    //}

    //public void LoadAsset()
    //{
    //    m_AssetBundle.LoadAssetAsync()
    //}

    public void InstantiateResource()
    {
        UnityEngine.Object obj = m_AssetBundle.mainAsset;
        OnResourceLoaded(obj, ResId);
    }
}
