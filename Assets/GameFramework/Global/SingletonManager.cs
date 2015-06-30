using System;
using System.Collections.Generic;
using UnityEngine;

public class Singletone
{
    public virtual bool Initialize()
    {
        return true;
    }
    public virtual bool InitializeData()
    {
        return true;
    }
    public virtual bool Uninitialize()
    {
        return true;
    }

    public virtual bool Update()
    {
        return true;
    }

    public virtual bool LateUpdate()
    {
        return true;
    }

    public virtual bool FixedUpdate()
    {
        return true;
    }
}

public class SingletonManager
{
    private GameMain m_GameMain;
    public GameMain GameMain
    {
        get { return m_GameMain; }
    }

    private Dictionary<Type, Singletone> m_dictManager = new Dictionary<Type, Singletone>();

    protected SingletonManager()
    {
        m_dictManager.Add(typeof(CTaskManager), new CTaskManager());
        m_dictManager.Add(typeof(CLogManager), new CLogManager());

        m_dictManager.Add(typeof(CResourceManager), new CResourceManager());
        m_dictManager.Add(typeof(CConfigManager), new CConfigManager());
        m_dictManager.Add(typeof(CUIManager), new CUIManager());
        m_dictManager.Add(typeof(CModelManager), new CModelManager());
        m_dictManager.Add(typeof(CCharacterManager), new CCharacterManager());
        m_dictManager.Add(typeof(CSceneManager), new CSceneManager());
        m_dictManager.Add(typeof(CCameraManager), new CCameraManager());
        m_dictManager.Add(typeof(CInputManager), new CInputManager());
    }

    private static SingletonManager m_instance;
    public static SingletonManager Inst
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new SingletonManager();
            }

            return m_instance;
        }
    }

    public void Initialize(GameMain main)
    {
        m_GameMain = main;
        foreach (KeyValuePair<Type, Singletone> kvpMgr in m_dictManager)
        {
            kvpMgr.Value.Initialize();
        }
    }

    public void InitializeData()
    {
        foreach (KeyValuePair<Type, Singletone> kvpMgr in m_dictManager)
        {
            kvpMgr.Value.InitializeData();
        }
    }

    public void Uninitialize()
    {
        foreach (KeyValuePair<Type, Singletone> kvpMgr in m_dictManager)
        {
            kvpMgr.Value.Uninitialize();
        }
    }

    public void Update()
    {
        foreach (KeyValuePair<Type, Singletone> kvpMgr in m_dictManager)
        {
            kvpMgr.Value.Update();
        }
    }

    public void LateUpdate()
    {
        foreach (KeyValuePair<Type, Singletone> kvpMgr in m_dictManager)
        {
            kvpMgr.Value.LateUpdate();
        }
    }

    public void FixedUpdate()
    {
        foreach (KeyValuePair<Type, Singletone> kvpMgr in m_dictManager)
        {
            kvpMgr.Value.FixedUpdate();
        }
    }

    public T GetManager<T>() where T : Singletone
    {
        return m_dictManager[typeof(T)] as T;
    }
}