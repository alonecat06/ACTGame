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

    public override bool Initialize()
    {
        m_goMainCamera = GameObject.Find("MainCamera");
        if (m_goMainCamera == null)
        {
            Debug.LogError("摄像机节点为空");
            return false;
        }

        return true;
    }

    public bool SetTarget(GameObject goTarget)
    {
        SimpleRpgCamera camera = m_goMainCamera.GetComponent<SimpleRpgCamera>();
        camera.enabled = true;
        camera.target = goTarget.transform;
        camera.targetOffset.y = 2.1f;
        camera.lockToTarget = true;
        camera.m_bUseTargetAxis = true;

        return true;
    }
}
