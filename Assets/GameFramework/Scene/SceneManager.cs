using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSceneManager : Singletone//MonoBehaviour 
{
    private GameObject m_goSceneRoot;

    //void Start () 
    //{
    //}

    public override bool Initialize()
    {
        m_goSceneRoot = GameObject.Find("Scene");
        if (m_goSceneRoot == null)
        {
            Debug.LogError("场景根节点为空");
            return false;
        }

        return true;
    }

    public void LoadScene(uint iSceneId)
    {
        SceneCfg cfgScene = SingletonManager.Inst.GetManager<CConfigManager>().GetConfigProvider<SceneCfgConfigProvider>().GetSceneCfg(iSceneId);
        if (cfgScene != null)
        {
            //显示加载动画
            SingletonManager.Inst.GameMain.OpenLoadingAnimation(LoadAnimation.LoadAnimation_WholeScreen);

            //界面切换到过场加载界面

            //加载地形（包括地形几何，光源）

            //加载建筑与物件

            //加载天空盒和天气（挂在摄像机下的粒子特效）
            CResource res = SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(900
                                                    , FinishLoadingSkyBox);
            res.OnResourceLoaded += FinishSceneLoading;
        }
        else
        {
            Debug.LogError("未能得到场景配置信息,场景编号为" + iSceneId);
        }
    }

    private void FinishLoadingSkyBox(CResource res)
    {
        //Material mat = obj as Material;
        Material mat = res.MainAsset as Material;
        if (mat == null)
        {
            return;
        }

        RenderSettings.skybox = mat;
    }

    //场景所有东西加载完成都回调
    private void FinishSceneLoading(CResource res)
    {
        SingletonManager.Inst.GameMain.CloseLoadingAnimation();
    }
}
