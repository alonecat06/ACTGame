using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum LoadAnimation
{
    LoadAnimation_WholeScreen,
    LoadAnimation_Icon,
}

public class GameMain : MonoBehaviour 
{
    public GameObject m_uiRoot;

    private LoadingUI m_LoadingUI;

    void Start()
    {
        if (m_uiRoot == null)
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogError("UI摄像机为空，严重错误");
        }

        GameObject goLoadingUI = GameObject.Find("goLoadingUI");
        if (goLoadingUI != null)
        {
            m_LoadingUI = goLoadingUI.GetComponent<LoadingUI>();
        }

        //管理类初始化
        SingletonManager.Inst.Initialize(this);

        SingletonManager.Inst.GetManager<CLogManager>().Log(Application.platform);

        //显示加载进度画面
        OpenLoadingAnimation(LoadAnimation.LoadAnimation_WholeScreen);
    }

    void Update()
    {
        SingletonManager.Inst.Update();
    }

    void LateUpdate()
    {
        SingletonManager.Inst.LateUpdate();
    }

    void FixedUpdate()
    {
        SingletonManager.Inst.FixedUpdate();
    }

    void OnApplicationQuit()
    {
        SingletonManager.Inst.Uninitialize();
    }

    public void OpenLoadingAnimation(LoadAnimation eLoadAnimation)
    {
        m_LoadingUI.SetLoadAnimation(eLoadAnimation);
    }

    public void CloseLoadingAnimation()
    {
        m_LoadingUI.CloseLoading();
    }
}
