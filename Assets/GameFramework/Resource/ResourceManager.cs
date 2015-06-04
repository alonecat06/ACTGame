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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

class CResourceManager : MonoBehaviour
{
    //网上资源总表
    private Dictionary<uint, uint> m_dictNetResVer = new Dictionary<uint, uint>();
    //本地资源总表
    private Dictionary<uint, uint> m_dictLocalResVer = new Dictionary<uint, uint>();

    //加载中的资源
    private Dictionary<uint, CResource> m_dictResLoading = new Dictionary<uint, CResource>();

    //已加载好，手动删除的资源
    private Dictionary<uint, CResource> m_dictResManual = new Dictionary<uint, CResource>();
    //已加载好，自动删除的资源
    private List<CResource> m_listResAutoRelease = new List<CResource>();

    //本地资源路径
    private string m_strLocalResPath = Application.persistentDataPath ;

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

    public void Initialize()
    {
        //下载网上的资源总表

        //加载本地的资源总表
        FileStream fs = new FileStream(GlobalDef.s_LocalFileRootPath + GlobalDef.s_ResVerName, FileMode.Open);
        StreamWrapper sw = new StreamWrapper(fs);
        LoadResVerBinaryFile(sw, true);
    }

    public void UnInitialize()
    {
        m_dictNetResVer.Clear();
        SaveResVerFile();
        m_dictLocalResVer.Clear();

        foreach (KeyValuePair<uint, CResource> kvp in m_dictResLoading)
        {
            kvp.Value.CancelResourceLoading();
        }
        m_dictResLoading.Clear();

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
        uint uReqVer;
        if (!m_dictNetResVer.TryGetValue(uResId, out uReqVer))
        {
            Debug.LogWarning("尝试获取旧资源，id：" + uResId);
        }

        #region 通过查找配置表得到ResourceInfo实现，暂时硬编码
        //string strAssetPath;
        //ResourceMaintainType eMaintainType;
        //int iCacheTime;
        //switch (uResId)
        //{
        //    case 0:
        //        {
        //            strAssetPath = GameMain.PathURL + "uiEnterGame.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_Manual;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 1:
        //        {
        //            strAssetPath = GameMain.PathURL + "uiClientSelect.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_Manual;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 100:
        //        {
        //            strAssetPath = GameMain.PathURL + "Player_1_male.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 101:
        //        {
        //            strAssetPath = GameMain.PathURL + "Player_1_female.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 500:
        //        {
        //            strAssetPath = GameMain.PathURL + "wp_blune04.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 600:
        //        {
        //            strAssetPath = GameMain.PathURL + "Spear02.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 700:
        //        {
        //            strAssetPath = GameMain.PathURL + "epic_shield3.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 800:
        //        {
        //            strAssetPath = GameMain.PathURL + "epic_shield2.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 900:
        //        {
        //            strAssetPath = GameMain.PathURL + "sky_01.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 901:
        //        {
        //            strAssetPath = GameMain.PathURL + "sky_02.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 950:
        //        {
        //            strAssetPath = GameMain.PathURL + "terrain01.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    case 951:
        //        {
        //            strAssetPath = GameMain.PathURL + "terrain02.assetbundle";
        //            eMaintainType = ResourceMaintainType.ResourceMaintain_AutoRelease;
        //            iCacheTime = 0;
        //        }
        //        break;
        //    default:
        //        {
        //            Debug.LogError("没有相应编号的资源，编号为" + uResId);
        //            return null;
        //        }
        //}
        #endregion
        CResourceInfo resInfo = SingletonManager.Inst.GetManager<CConfigManager>().GetConfigProvider<ResourceInfoConfigProvider>().GetResourceInfo(uResId);
        if (resInfo == null)
        {
            Debug.LogError("资源配置表中找不到配置信息，ResId为" + uResId);
            return null;
        }

        CResource res;
        if (!m_dictResLoading.TryGetValue(uResId, out res))
        {
            //res = new CResource(uResId, strAssetPath, uReqVer, eMaintainType, iCacheTime);
            res = new CResource(resInfo, uReqVer);
            m_dictResLoading.Add(uResId, res);
            res.OnResourceLoaded += deleg;
            res.OnResourceLoadCancel += ResourceLoadCancle;
            StartCoroutine(LoadAssetBundle(res));
        }

        return res;
    }

    private void ResourceLoadCancle(CResource Res)
    {
        m_dictResLoading.Remove(Res.ResId);
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
        if (IsLocalResUpToDate(res))
        {
            WWW www = new WWW(GlobalDef.s_LocalFileRootPath + res.ResourcePath + res.ResourceName);
            yield return www;

            CompleteResourceLoading(www.assetBundle, res);
        } 
        else
        {//下载需要更新的资源
            WWW www = new WWW(GlobalDef.s_FileServerUrl + res.ResourcePath + res.ResourceName);
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
                Debug.LogWarning("www is null--path" + res.ResourcePath + res.ResourceName);
            }

            CompleteResourceLoading(www.assetBundle, res);
            //将资源写入本地目录，并清理旧资源（如果有的话）
            UpdateLocalResource(res.ResourcePath, res.ResourceName, new StreamWrapper(www.bytes));
            //更新本地资源总表版本号
            UpdateLocalResVer(res.ResId, res.RequireResVer);
        }

    }

    private bool IsLocalResUpToDate(CResource res)
    {
        if (m_dictLocalResVer.ContainsKey(res.ResId))
        {
            return res.RequireResVer <= m_dictLocalResVer[res.ResId];
        } 
        else
        {
            return false;
        }
    }

    private void CompleteResourceLoading(AssetBundle ab, CResource res)
    {
        res.AssetBundle = ab;
        res.IsLoaded = true;
        res.FinishResourceLoad();

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

    private bool UpdateLocalResource(string path, string name, StreamWrapper swResource)
    {
        using (FileStream fs = new FileStream(path + "//" + name, FileMode.Create))
        {
            StreamWrapper sw = new StreamWrapper(fs);
            sw.Write(swResource);
            sw.Close();
        }

        return false;
    }

    private bool UpdateLocalResVer(uint uResId, uint uVerId)
    {
        if (m_dictLocalResVer.ContainsKey(uResId))
        {
            m_dictLocalResVer[uResId] = uVerId;
            return true;
        }
        return false;
    }

    public bool LoadResVerBinaryFile(StreamWrapper stream, bool bIsLocal)
    {
        BinaryFormatter binFormat = new BinaryFormatter();
        if (bIsLocal)
        {
            m_dictLocalResVer = (Dictionary<uint, uint>)binFormat.Deserialize(stream.GetStream());
        } 
        else
        {
            m_dictNetResVer = (Dictionary<uint, uint>)binFormat.Deserialize(stream.GetStream());
        }
        return true;
    }

    public bool SaveResVerFile()
    {
        using (FileStream fs = new FileStream(GlobalDef.s_LocalFileRootPath + "/" + GlobalDef.s_ResVerName, FileMode.Create))
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(fs, m_dictLocalResVer);
        }

        return true;
    }
}
