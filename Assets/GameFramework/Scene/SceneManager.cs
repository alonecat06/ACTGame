using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSceneManager : MonoBehaviour 
{
    private GameObject m_goSceneRoot;

	void Start () 
    {
        m_goSceneRoot = GameObject.Find("Scene");
        if (m_goSceneRoot == null)
        {
            Debug.LogError("场景根节点为空");
        }
	}

    public void LoadScene(int iSceneId)
    {
        if (iSceneId == 1)
        {
            //界面切换到过场加载界面

            //加载地形（包括地形几何，光源）

            //加载建筑与物件

            //加载天空盒和天气（挂在摄像机下的粒子特效）
            CResource res = SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(900
                                                    , FinishLoadingSkyBox);
            res.OnResourceLoaded += FinishSceneLoading;
        } 
        else if (iSceneId == 2)
        {
            //界面切换到过场加载界面

            //加载地形（包括地形几何，光源）

            //加载建筑与物件

            //加载天空盒和天气（挂在摄像机下的粒子特效）
            CResource res = SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(901
                                                    , FinishLoadingSkyBox);
            res.OnResourceLoaded += FinishSceneLoading;
        }
    }

    private void FinishLoadingSkyBox(Object obj, uint uResId)
    {
        Material mat = obj as Material;
        if (mat == null)
        {
            return;
        }

        RenderSettings.skybox = mat;
    }

    //场景所有东西加载完成都回调
    private void FinishSceneLoading(Object obj, uint uResId)
    {

    }
}
