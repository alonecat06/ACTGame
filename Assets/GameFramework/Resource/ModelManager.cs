using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//*******************************************************
//模型管理器   
//目的：确保同一个模型只有一个资源被加载
//      场景中的都是它的副本
//********************************************************

public class CModelManager : MonoBehaviour
{
    private Dictionary<uint, GameObject> m_dictModel = new Dictionary<uint, GameObject>();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetModel(uint uModelId, ResourceLoaded deleg)
    {
        //查询是否已有实例化的样本
        GameObject goRes;
        if (m_dictModel.TryGetValue(uModelId, out goRes))
        {
            //拷贝副本
            //goRes = GameObject.Instantiate(goRes);

            //返回结果
            deleg(goRes, uModelId);
        } 
        else
        {
            CResource res = SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(uModelId
                    , FinishLoadingModel);
            res.OnResourceLoaded += deleg;
        }
    }

    private void FinishLoadingModel(Object go, uint uResId)
    //private void FinishLoadingModel(CResource res)
    {
        GameObject gobj = go as GameObject;
        //GameObject gobj = res.MainAsset as GameObject;
        if (gobj == null)
        {
            return;
        }

        m_dictModel.Add(uResId, gobj);
        //m_dictModel.Add(res.ResId, gobj);
    }
}