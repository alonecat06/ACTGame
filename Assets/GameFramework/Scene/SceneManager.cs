//**********************************************************************
//                         CSceneManager
//负责功能:
//  1. 场景加载，包括地形/天空盒/天气/静态物品/npc/动态角色的加载
//  2. 
//**********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSceneManager : Singletone
{
    private GameObject m_goSceneRoot;
    private GameObject m_goWeather;
    private SceneCfg m_cfgScene;

    //private uint m_uMasterHandle;

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

    public GameObject SceneRoot
    {
        get { return m_goSceneRoot; }
    }
    public SceneCfg SceneConfig
    {
        get { return m_cfgScene; }
    }
    //public uint MasterHandle
    //{
    //    get { return m_uMasterHandle; }
    //}

    public void LoadScene(uint iSceneId)
    {
        CConfigManager cm = SingletonManager.Inst.GetManager<CConfigManager>();
        SceneCfg cfgScene = cm.GetConfigProvider<SceneCfgConfigProvider>().GetSceneCfg(iSceneId);
        if (cfgScene != null)
        {
            m_cfgScene = cfgScene;

            //界面切换到过场加载界面
            SingletonManager.Inst.GameMain.OpenLoadingAnimation(LoadAnimation.LoadAnimation_WholeScreen);

            CTask taskLoadTerrain = new CTask();
            CResourceManager mgrResource = SingletonManager.Inst.GetManager<CResourceManager>();

            //加载天空盒
            CSubTask subTask1 = new CSubTask();
            mgrResource.LoadResource(cfgScene.uSkyId, FinishLoadingSkyBox, out subTask1.routine);
            taskLoadTerrain.AddTask(subTask1);

            //加载天气（挂在摄像机下的粒子特效）
            if (cfgScene.uWeatherId != 0)
            {
                CSubTask subTask2 = new CSubTask();
                mgrResource.LoadResource(cfgScene.uWeatherId, FinishLoadingWeather, out subTask2.routine);
                taskLoadTerrain.AddTask(subTask2);
            }

            //加载地形（包括地形几何，光源，阴影）
            CSubTask subTask3 = new CSubTask();
            mgrResource.LoadResource(cfgScene.uTerrainId, FinishLoadingTerrain, out subTask3.routine);
            taskLoadTerrain.AddTask(subTask3);

            taskLoadTerrain.OnTaskFinished += FinishTerrainLoading;
            SingletonManager.Inst.GetManager<CTaskManager>().StartTask(taskLoadTerrain);
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

    //地形加载完成回调
    private void FinishTerrainLoading(CTask task)
    {
        CTask taskLoadObject = new CTask();
        //CResourceManager mgrResource = SingletonManager.Inst.GetManager<CResourceManager>();
        CCharacterManager mgrCharacter = SingletonManager.Inst.GetManager<CCharacterManager>();

        //加载静态的建筑与物件

        //加载玩家
        CSubTask subTask2 = new CSubTask();
        mgrCharacter.MasterHandle = mgrCharacter.CreateCharaterToScene(mgrCharacter.GetPlayerSetting(0), SetPlayerToScene, out subTask2.routine);
        taskLoadObject.AddTask(subTask2);

        taskLoadObject.OnTaskFinished += FinishSceneLoading;
        SingletonManager.Inst.GetManager<CTaskManager>().StartTask(taskLoadObject);
    }

    private void SetPlayerToScene(CCharacter entity)
    {
        entity.m_trans.SetParent(m_goSceneRoot.transform);
        entity.m_trans.position = new Vector3(m_cfgScene.fEnterX, m_cfgScene.fEnterY, m_cfgScene.fEnterZ);
        entity.m_trans.localRotation = new Quaternion(0, 0, 0, 1);
    }

    //场景加载完成回调
    private void FinishSceneLoading(CTask task)
    {
        //结束加载进度条
        SingletonManager.Inst.GameMain.CloseLoadingAnimation();

        CCharacter player = SingletonManager.Inst.GetManager<CCharacterManager>().GetMaster();
        SingletonManager.Inst.GetManager<CCameraManager>().SetTarget(player.m_goCharacter);
        SingletonManager.Inst.GetManager<CInputManager>().SetMaster(player);

        //加载玩家操作界面
        SingletonManager.Inst.GetManager<CUIManager>().LoadUI(UIId.UIId_PlayerManipulate, LoadAnimation.LoadAnimation_Icon);
    }
}
