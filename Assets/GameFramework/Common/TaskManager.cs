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

public delegate void TaskFinish(CTask task);
public delegate void TaskCancel(CTask task);

public class CSubTask
{
    public bool bTaskFinish = false;
    public IEnumerator routine;
    public CTask task;

    bool running = true;
    bool paused = false;
    bool stopped = false;

    public IEnumerator StartSubTask()
    {
        yield return null;
        IEnumerator e = routine;
        while (running)
        {
            if (paused)
                yield return null;
            else
            {
                if (e != null && e.MoveNext())
                {
                    yield return e.Current;
                }
                else
                {
                    running = false;
                }
            }
        }
        bTaskFinish = true;
        task.CheckFinishAllSubTask();
    }
}

public class CTask
{
    public List<CSubTask> listSubTask = new List<CSubTask>();

    public event TaskFinish OnTaskFinished;
    public event TaskCancel OnTaskCancel;

    public void AddTask(CSubTask subTask)
    {
        subTask.task = this;
        listSubTask.Add(subTask);
    }

    public void CheckFinishAllSubTask()
    {
        int iCount = listSubTask.Count;
        for (int i = 0; i < iCount; ++i)
        {
            if (listSubTask[i].bTaskFinish == false)
            {
                return;
            }
        }

        FinishTask();
        return;
    }

    public void FinishTask()
    {
        OnTaskFinished(this);
    }
    public void CancelTask()
    {
        OnTaskCancel(this);
    }
}


public class CTaskManager : Singletone
{
    private GameObject m_goCoroutine;
    private MonoBehaviour m_mbCoroutine;
    private List<CTask> m_listTask = new List<CTask>();

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

    public void StartCoroutine(IEnumerator routine)
    {
        m_mbCoroutine.StartCoroutine(routine);
    }

    public void StartTask(CTask task)
    {
        int iCount = task.listSubTask.Count;
        for (int i = 0; i < iCount; ++i)
        {
            m_mbCoroutine.StartCoroutine(task.listSubTask[i].StartSubTask());
        }

        m_listTask.Add(task);
    }
}
