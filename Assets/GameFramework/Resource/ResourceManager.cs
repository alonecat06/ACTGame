//**********************************************************************
//                         CResourceManager
//负责功能:
//  1. 本地/网络资源校验
//  2. 管理资源，释放不使用的资源
//  3. 得到加载资源的进度
//  4. 依赖资源自动加载（本地加载/网上下载）
//**********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

class CResourceManager : Singletone
{
    //网上资源总表
    private Dictionary<uint, uint> m_dictNetResVer = new Dictionary<uint, uint>();
    //本地资源总表
    private Dictionary<uint, uint> m_dictLocalResVer = new Dictionary<uint, uint>();
    //是否存在本地资源
    private bool m_bNoLocalRes;
    //只使用本地资源(没有搭建资源服务器时供Android使用)
    private bool m_bOnlyLocalRes = false;

    private ResourceInfoConfigProvider cpResInfo;

    //加载中的资源
    private Dictionary<uint, CResource> m_dictResLoading = new Dictionary<uint, CResource>();

    //已加载好，手动删除的资源
    private Dictionary<uint, CResource> m_dictResManual = new Dictionary<uint, CResource>();
    //已加载好，自动删除的资源
    private List<CResource> m_listResAutoRelease = new List<CResource>();

    public override bool Initialize()
    {
        //判断是否存在本地的资源总表
        m_bNoLocalRes = !File.Exists(Application.streamingAssetsPath + '/' + GlobalDef.s_ResVerName);

        //下载网上资源总表和加载本地资源总表
        SingletonManager.Inst.GetManager<CTaskManager>().StartCoroutine(LoadResVer());
        return true;
    }

    public override bool Uninitialize()
    {
        SaveResVerFile();
        m_dictNetResVer.Clear();
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

        return true;
    }

    public override bool Update()
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

        return true;
    }

    public CResource LoadResource(uint uResId, ResourceLoaded deleg)
    {
        uint uReqVer;
        if (!m_dictNetResVer.TryGetValue(uResId, out uReqVer))
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogWarning("尝试获取旧资源，id：" + uResId);
        }

        CResourceInfo resInfo = cpResInfo.GetResourceInfo(uResId);
        if (resInfo == null)
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogError("资源配置表中找不到配置信息，ResId为" + uResId);
            return null;
        }
        CResource res;
        if (!m_dictResLoading.TryGetValue(uResId, out res))
        {
            res = new CResource(resInfo, uReqVer);
            m_dictResLoading.Add(uResId, res);
            res.OnResourceLoaded += deleg;
            res.OnResourceLoadCancel += ResourceLoadCancle;
            SingletonManager.Inst.GetManager<CTaskManager>().StartCoroutine(LoadAssetBundle(res));
        }
        return res;
    }

    public CResource LoadResource(uint uResId, ResourceLoaded deleg, out IEnumerator iter)
    {
        iter = null;

        uint uReqVer;
        if (m_dictNetResVer.Count == 0)
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogWarning("由于没有资源服务器，暂时使用了本地资源模式，可修改成员变量m_bOnlyLocalRes修复");
            uReqVer = 0;
        }
        else if (!m_dictNetResVer.TryGetValue(uResId, out uReqVer))
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogWarning("尝试获取旧资源，id：" + uResId);
        }

        CResourceInfo resInfo = cpResInfo.GetResourceInfo(uResId);
        if (resInfo == null)
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogError("资源配置表中找不到配置信息，ResId为" + uResId);
            return null;
        }

        CResource res = new CResource(resInfo, uReqVer);
        m_dictResLoading.Add(uResId, res);
        res.OnResourceLoaded += deleg;
        res.OnResourceLoadCancel += ResourceLoadCancle;        
        iter = LoadAssetBundle(res);

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

    private IEnumerator LoadResVer()
    {
        #region 下载网络资源总表
        if (!m_bOnlyLocalRes)
        {
            WWW www = new WWW(GlobalDef.s_FileServerUrl + GlobalDef.s_ResVerName);
            yield return www;

            if (null != www.error)
            {
                SingletonManager.Inst.GetManager<CLogManager>().LogError(www.url + "资源网络下载出问题" + www.error);
                yield break;
            }

            StreamWrapper sw = new StreamWrapper(www.bytes);
            LoadResVerBinaryFile(sw, false);
        }
        #endregion

        #region 加载本地资源总表
        if (!m_bNoLocalRes)
        {
            WWW www1 = new WWW(GlobalDef.s_LocalFileRootPath + GlobalDef.s_ResVerName);
            //WWW www1 = new WWW(System.IO.Path.Combine(Application.streamingAssetsPath, GlobalDef.s_ResVerName));
            yield return www1;

            if (null != www1.error)
            {
                SingletonManager.Inst.GetManager<CLogManager>().LogError(www1.url + "资源本地加载出问题" + www1.error);
                yield break;
            }

            StreamWrapper sw1 = new StreamWrapper(www1.bytes);
            LoadResVerBinaryFile(sw1, true);                    
        } 
        #endregion

        #region 加载资源信息表
        CResource res;
        if (!m_dictResLoading.TryGetValue(GlobalDef.s_ResourceInfoResId, out res))
        {
            res = new CResource(GlobalDef.s_ResourceInfoResId
                                , GlobalDef.s_ResourceInfoName
                                , "Config/"
                                , m_bOnlyLocalRes? 0 : m_dictNetResVer[GlobalDef.s_ResourceInfoResId]
                                , ResourceMaintainType.ResourceMaintain_AutoRelease
                                , 10);
            m_dictResLoading.Add(GlobalDef.s_ResourceInfoResId, res);
            res.OnResourceLoaded += ResourceInfoConfigLoaded;
            res.OnResourceLoadCancel += ResourceLoadCancle;
            SingletonManager.Inst.GetManager<CTaskManager>().StartCoroutine(LoadAssetBundle(res));
        }
        #endregion
    }
    private void ResourceInfoConfigLoaded(CResource res)
    {
        object[] txt = res.AssetBundle.LoadAllAssets(typeof(TextAsset));
        if (null != txt && txt.Length == 1)
        {
            #region 读取资源信息表数据
            TextAsset tt = txt[0] as TextAsset;
            StreamWrapper sw = new StreamWrapper(tt.bytes);
            cpResInfo = new ResourceInfoConfigProvider();
            cpResInfo.LoadBinaryFile(sw);
            #endregion


            #region 初始化所有管理器的数据
            SingletonManager.Inst.InitializeData();
            #endregion
        }
    }

    private IEnumerator LoadAssetBundle(CResource res)
    {
        if (m_bOnlyLocalRes || (!m_bNoLocalRes && IsLocalResUpToDate(res) && IsResExistLocal(res)))
        {
            #region 读取本地资源
            WWW www = new WWW(GlobalDef.s_LocalFileRootPath + res.ResourcePath + res.ResourceName);
            //WWW www = new WWW(System.IO.Path.Combine(Application.streamingAssetsPath, res.ResourcePath + res.ResourceName));
            yield return www;

            if (null != www.error)
            {
                SingletonManager.Inst.GetManager<CLogManager>().LogError(www.url + "资源本地加载出问题" + www.error);
                yield break;
            }
            #endregion

            CompleteResourceLoading(www.assetBundle, res);
        } 
        else
        {
            #region 下载需要更新的资源
            WWW www = new WWW(GlobalDef.s_FileServerUrl + res.ResourcePath + res.ResourceName);
            yield return www;

            if (null != www.error)
            {
                SingletonManager.Inst.GetManager<CLogManager>().LogError(GlobalDef.s_FileServerUrl + res.ResourcePath + res.ResourceName + "资源下载出问题" + www.error);
                yield break;
            }
            #endregion

            CompleteResourceLoading(www.assetBundle, res);
            //将资源写入本地目录，并清理旧资源（如果有的话）
            UpdateLocalResource(Application.streamingAssetsPath + "/" + res.ResourcePath, res.ResourceName, new StreamWrapper(www.bytes));
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

    private bool IsResExistLocal(CResource res)
    {
        bool bFileExist = File.Exists(Application.streamingAssetsPath + "/" + res.ResourcePath + res.ResourceName);
        return bFileExist;
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
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        using (FileStream fs = new FileStream(path + "/" + name, FileMode.Create))
        {
            StreamWrapper sw = new StreamWrapper(fs);
            sw.Write(swResource);
            sw.Close();
        }

        return false;
    }

    private bool UpdateLocalResVer(uint uResId, uint uVerId)
    {
        m_dictLocalResVer[uResId] = uVerId;
        return true;
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
        using (FileStream fs = new FileStream(Application.streamingAssetsPath + "/" + GlobalDef.s_ResVerName, FileMode.Create))
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(fs, m_dictLocalResVer);
        }

        return true;
    }
}
