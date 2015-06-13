using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSceneManager : Singletone
{
    private GameObject m_goSceneRoot;
    private GameObject m_goWeather;

    public override bool Initialize()
    {
        m_goSceneRoot = GameObject.Find("Scene");
        if (m_goSceneRoot == null)
        {
            Debug.LogError("场景节点为空");
            return false;
        }
        m_goWeather = GameObject.Find("Weather");
        if (m_goWeather == null)
        {
            Debug.LogError("天气节点为空");
            return false;
        }
        return true;
    }

    public void LoadScene(uint iSceneId)
    {
        CConfigManager cm = SingletonManager.Inst.GetManager<CConfigManager>();
        SceneCfg cfgScene = cm.GetConfigProvider<SceneCfgConfigProvider>().GetSceneCfg(iSceneId);
        if (cfgScene != null)
        {
            //界面切换到过场加载界面
            SingletonManager.Inst.GameMain.OpenLoadingAnimation(LoadAnimation.LoadAnimation_WholeScreen);
            
            //加载天空盒
            CSubTask subTask1 = new CSubTask();
            SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(cfgScene.uSkyId
                                                    , FinishLoadingSkyBox
                                                    , out subTask1.routine);
            //加载天气（挂在摄像机下的粒子特效）
            CSubTask subTask2 = new CSubTask();
            SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(cfgScene.uWeatherId
                                                    , FinishLoadingWeather
                                                    , out subTask2.routine);
            //加载地形（包括地形几何，光源，阴影）
            CSubTask subTask3 = new CSubTask();
            SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(cfgScene.uTerrainId
                                                    , FinishLoadingTerrain
                                                    , out subTask3.routine);
            //加载静态的建筑与物件

            //加载角色

            CTask task = new CTask();
            task.AddTask(subTask1);
            task.AddTask(subTask2);
            task.AddTask(subTask3);
            task.OnTaskFinished += FinishSceneLoading;
            SingletonManager.Inst.GetManager<CTaskManager>().StartTask(task);
        }
        else
        {
            Debug.LogError("未能得到场景配置信息,场景编号为" + iSceneId);
        }
    }

    private void FinishLoadingSkyBox(CResource res)
    {
        Material mat = (Material)res.AssetBundle.LoadAsset(res.ResourceName, typeof(Material));
        if (mat == null)
        {
            return;
        }
        RenderSettings.skybox = mat;
    }

    private void FinishLoadingWeather(CResource res)
    {
        GameObject gobj = (GameObject)res.AssetBundle.LoadAsset(res.ResourceName, typeof(GameObject));
        if (gobj == null)
        {
            return;
        }
        gobj = GameObject.Instantiate(gobj);
        gobj.transform.SetParent(m_goWeather.transform);
    }

    private void FinishLoadingTerrain(CResource res)
    {
        GameObject gobj = (GameObject)res.AssetBundle.LoadAsset(res.ResourceName, typeof(GameObject));
        if (gobj == null)
        {
            return;
        }
        gobj = GameObject.Instantiate(gobj);
        gobj.transform.SetParent(m_goSceneRoot.transform);
    }

    //场景所有东西加载完成都回调
    private void FinishSceneLoading(CTask task)
    {
        SingletonManager.Inst.GameMain.CloseLoadingAnimation();
    }
}
