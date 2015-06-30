//**********************************************************************
//                                CCameraManager
//负责功能:
//  1. 摄像机管理
//**********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum MouseButton
{
    None = 20,
    Left = 0,
    Middle = 2,
    Right = 1
}

public class CCameraManager : Singletone
{
    private GameObject m_goMainCamera;
    private SimpleRpgCamera m_Camera;

    public override bool Initialize()
    {
        m_goMainCamera = GameObject.Find("MainCamera");
        if (m_goMainCamera == null)
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogError("摄像机节点为空");
            return false;
        }

        return true;
    }

    public bool SetTarget(GameObject goTarget)
    {
        m_Camera = m_goMainCamera.GetComponent<SimpleRpgCamera>();
        m_Camera.enabled = true;
        m_Camera.target = goTarget.transform;
        m_Camera.targetOffset.y = 2.1f;
        m_Camera.lockToTarget = true;
        m_Camera.m_bUseTargetAxis = true;

        return true;
    }

    public void EnableCameraControl(bool bControl)
    {
        m_Camera.Controllable = bControl;
    }
}
