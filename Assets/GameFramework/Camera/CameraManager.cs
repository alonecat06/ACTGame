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
    private Transform m_transMainCamera;

    public override bool Initialize()
    {
        m_goMainCamera = GameObject.Find("MainCamera");
        if (m_goMainCamera == null)
        {
            Debug.LogError("摄像机节点为空");
            return false;
        }
        m_transMainCamera = m_goMainCamera.transform;

        //m_goMainCamera.AddComponent<SimpleRpgCamera>();

        return true;
    }

    public bool SetTarget(GameObject goTarget)
    {
        SimpleRpgCamera camera = m_goMainCamera.AddComponent<SimpleRpgCamera>();
        camera.target = goTarget.transform;
        camera.targetOffset.y = 2.1f;
        camera.lockToTarget = true;
        camera.useTargetAxis = true;

        return true;
    }

    //public override bool Uninitialize()
    //{
    //    GameObject.Destroy(m_goCoroutine);
    //    return true;
    //}

    //public override bool Update()
    //{
    //    int iCount = m_listTask.Count;
    //    for (int i = iCount -1; i >= 0; --i)
    //    {
    //        if (m_listTask[i].CheckFinishAllSubTask())
    //        {
    //            m_listTask[i].FinishTask();
    //            m_listTask.Remove(m_listTask[i]);
    //        }
    //    }
    //    return true;
    //}

    //public void StartCoroutine(IEnumerator routine)
    //{
    //    m_mbCoroutine.StartCoroutine(routine);
    //}

    //public void StartTask(CTask task)
    //{
    //    int iCount = task.listSubTask.Count;
    //    for (int i = 0; i < iCount; ++i)
    //    {
    //        m_mbCoroutine.StartCoroutine(task.listSubTask[i].StartSubTask());
    //    }

    //    m_listTask.Add(task);
    //}
}
