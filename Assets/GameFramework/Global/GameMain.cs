﻿using UnityEngine;
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
            Debug.LogError("UI摄像机为空，严重错误");
        }

        GameObject goLoadingUI = GameObject.Find("goLoadingUI");
        if (goLoadingUI != null)
        {
            m_LoadingUI = goLoadingUI.GetComponent<LoadingUI>();
        }

        //管理类初始化
        SingletonManager.Inst.Initialize(this);
        //显示加载进度画面
        OpenLoadingAnimation(LoadAnimation.LoadAnimation_WholeScreen);
    }

    void Update()
    {
        SingletonManager.Inst.Update();
    }

    void OnApplicationQuit()
    {
        SingletonManager.Inst.Uninitialize();
    }


    public void LoadFirstUI()
    {
        //加载第一个界面
        SingletonManager.Inst.GetManager<CUIManager>().LoadUI(1, LoadAnimation.LoadAnimation_WholeScreen);
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
