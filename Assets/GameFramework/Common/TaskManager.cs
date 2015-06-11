//**********************************************************************
//                                CTaskManager
//负责功能:
//  1. 启动协程
//  2. 实现协程间的等待
//  3. 得到加载任务的进度
//  4. 实现定时器
//**********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CTaskManager : Singletone
{
    private GameObject m_goCoroutine;
    private MonoBehaviour m_mbCoroutine;

    public override bool Initialize()
    {
        m_goCoroutine = new GameObject();
        m_goCoroutine.name = "CoroutineGameObject";
        m_mbCoroutine = m_goCoroutine.AddComponent<MonoBehaviour>();
        GameObject.DontDestroyOnLoad(m_goCoroutine);

        return true;
    }

    public override bool Uninitialize()
    {
        GameObject.Destroy(m_goCoroutine);
        return true;
    }

    public override bool Update()
    {
        return true;
    }

    public void StartCoroutine(IEnumerator routine)
    {
        m_mbCoroutine.StartCoroutine(routine);
    }
}
