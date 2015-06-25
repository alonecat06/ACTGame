//*******************************************************
//                       CModelManager
//负责功能:确保同一个模型只有一个资源被加载，场景中的都是它的副本
//********************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public delegate void ModelLoaded(GameObject go);

public class CModelManager : Singletone
{
    private Dictionary<uint, GameObject> m_dictModel = new Dictionary<uint, GameObject>();

    public void GetModel(uint uModelId, ResourceLoaded delegResLoad, ModelLoaded delegModelLoad)
    {
        //查询是否已有实例化的样本
        GameObject goRes;
        if (m_dictModel.TryGetValue(uModelId, out goRes))
        {
            //拷贝副本
            goRes = GameObject.Instantiate(goRes);

            //返回结果
            delegModelLoad(goRes);
        } 
        else
        {
            CResource res = SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(uModelId
                    , FinishLoadingModel);
            res.OnResourceLoaded += delegResLoad;
        }
    }
    public void GetModel(uint uModelId, ResourceLoaded delegResLoad, ModelLoaded delegModelLoad, out IEnumerator corotineLoading)
    {
        //查询是否已有实例化的样本
        GameObject goRes;
        if (m_dictModel.TryGetValue(uModelId, out goRes))
        {
            corotineLoading = null;

            //拷贝副本
            goRes = GameObject.Instantiate(goRes);

            //返回结果
            delegModelLoad(goRes);
        }
        else
        {
            CResource res = SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(uModelId
                    , FinishLoadingModel
                    , out corotineLoading);
            res.OnResourceLoaded += delegResLoad;
        }
    }

    private void FinishLoadingModel(CResource res)
    {
        GameObject gobj = (GameObject)res.AssetBundle.LoadAsset(res.ResourceName, typeof(GameObject));// res.MainAsset as GameObject;
        if (gobj == null)
        {
            return;
        }
        m_dictModel.Add(res.ResId, gobj);
    }
}