//**********************************************************************
//                         CResourceManager
//负责功能:
//  1. 本地/网络资源校验
//  2. 管理资源，释放不使用的资源
//  3. 得到加载资源的进度
//  4. 依赖资源自动加载（本地加载/网上下载）
//**********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CResourceManager : MonoBehaviour
{
    //加载中的资源
    private Dictionary<uint, CResource> m_dictResLoading = new Dictionary<uint, CResource>();

    //已加载好，手动删除的资源
    private Dictionary<uint, CResource> m_dictResManual = new Dictionary<uint, CResource>();
    //已加载好，自动删除的资源
    private List<CResource> m_listResAutoRelease = new List<CResource>();

    //void Start()
    //{
    //    Initialize();
    //}

    void Update()
    {
        if (Time.frameCount % 10 == 0)
        {
            for (int i = m_listResAutoRelease.Count - 1; i >= 0; --i)
            {
                if (m_listResAutoRelease[i].ReleaseTime < Time.time)
                {
                    m_listResAutoRelease[i].UnloadResource();
                    m_listResAutoRelease.RemoveAt(i);
                }
            }

            Resources.UnloadUnusedAssets();
        }

        if (Time.frameCount % 100 == 0)
        { 
            System.GC.Collect(); 
        }
    }

    void Destroy()
    {
        UnInitialize();
    }

    //public void Initialize()
    //{
    //    m_dictResManual = new Dictionary<uint, CResource>();
    //    m_listResAutoRelease = new List<CResource>();
    //}

    public void UnInitialize()
    {
        for (int i = m_listResAutoRelease.Count - 1; i >= 0; --i)
        {
             m_listResAutoRelease[i].UnloadResource();
        }
        m_listResAutoRelease.Clear();

        foreach (KeyValuePair<uint, CResource> kvp in m_dictResManual)
        {
            kvp.Value.UnloadResource();
        }
        m_dictResManual.Clear();
    }

    public CResource LoadResource(uint uResId
                                , ResourceLoaded deleg)
    {
        string strAssetPath;
        ResourceMaintainType eMaintainType;
        int iReqVer;
        int iCacheTime;

        #region 通过查找配置表实现，暂时硬编码
        switch (uResId)
        {
            case 0:
                {
                    strAssetPath = GameMain.PathURL + "uiEnterGame.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_Manual;
                    iCacheTime = 0;
                }
                break;
            case 1:
                {
                    strAssetPath = GameMain.PathURL + "uiClientSelect.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_Manual;
                    iCacheTime = 0;
                }
                break;
            case 100:
                {
                    strAssetPath = GameMain.PathURL + "Player_1_male.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
                    iCacheTime = 0;
                }
                break;
            case 101:
                {
                    strAssetPath = GameMain.PathURL + "Player_1_female.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
                    iCacheTime = 0;
                }
                break;
            case 500:
                {
                    strAssetPath = GameMain.PathURL + "wp_blune04.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
                    iCacheTime = 0;
                }
                break;
            case 600:
                {
                    strAssetPath = GameMain.PathURL + "Spear02.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
                    iCacheTime = 0;
                }
                break;
            case 700:
                {
                    strAssetPath = GameMain.PathURL + "epic_shield3.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
                    iCacheTime = 0;
                }
                break;
            case 800:
                {
                    strAssetPath = GameMain.PathURL + "epic_shield2.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
                    iCacheTime = 0;
                }
                break;
            case 900:
                {
                    strAssetPath = GameMain.PathURL + "sky_01.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
                    iCacheTime = 0;
                }
                break;
            case 901:
                {
                    strAssetPath = GameMain.PathURL + "sky_02.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
                    iCacheTime = 0;
                }
                break;
            case 950:
                {
                    strAssetPath = GameMain.PathURL + "terrain01.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
                    iCacheTime = 0;
                }
                break;
            case 951:
                {
                    strAssetPath = GameMain.PathURL + "terrain02.assetbundle";
                    iReqVer = 0;
                    eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
                    iCacheTime = 0;
                }
                break;
            default:
                {
                    Debug.LogError("没有相应编号的资源，编号为" + uResId);
                    return null;
                }
        }
        #endregion

        CResource res;
        if (!m_dictResLoading.TryGetValue(uResId, out res))
        {
            res = new CResource(uResId
                        , strAssetPath
                        , iReqVer
                        , eMaintainType
                        , iCacheTime);
            m_dictResLoading.Add(uResId, res);
            res.OnResourceLoaded += deleg;
            res.OnResourceLoadCancel += ResourceLoadCancle;
            StartCoroutine(LoadAssetBundle(res));
        }

        return res;
    }

    public void UnloadResource(uint uResId)
    {
        CResource cResForRemove;
        if (m_dictResManual.TryGetValue(uResId, out cResForRemove))
        {
            m_dictResManual.Remove(uResId);
            cResForRemove.UnloadResource();
        }
    }

    private IEnumerator LoadAssetBundle(CResource res)
    {
        //WWW www = WWW.LoadFromCacheOrDownload(res.AssetPath, res.RequireResVer);
        //yield return www;
        WWW www = new WWW(res.AssetPath);
        yield return www;

        if (null != www)
        {
            if (null != www.error)
            {
                Debug.LogError(www.error);
            }
        }
        else
        {
            Debug.LogWarning("www is null--path" + res.AssetPath);
        }

        ResourceLoaded(www, res);
    }

    private void ResourceLoaded(WWW www, CResource res)
    {
        res.AssetBundle = www.assetBundle;
        res.IsLoaded = true;
        //res.FinishResourceLoad();
        res.InstantiateResource();

        if (res.MaintainType == ResourceMaintainType.ResourceMaintain_Manual)
        {
            m_dictResManual.Add(res.ResId, res);
        } 
        else //eMaintainType == ResourceMaintainType.ResourceMaintain_AutoRelease)
        {
            m_listResAutoRelease.Add(res);
        }

        m_dictResLoading.Remove(res.ResId);
    }

    private void ResourceLoadCancle(CResource Res)
    {
        m_dictResLoading.Remove(Res.ResId);
    }
}
