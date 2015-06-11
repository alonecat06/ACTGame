using System;
using System.Collections.Generic;
using UnityEngine;

public class Singletone
{
    public virtual bool Initialize()
    {
        return false;
    }
    public virtual bool Uninitialize()
    {
        return false;
    }

    public virtual bool Update()
    {
        return false;
    }
}

public class SingletonManager
{
    private GameMain m_GameMain;
    public GameMain GameMain
    {
        get { return m_GameMain; }
    }

    //private Dictionary<Type, Singletone> m_dictManager = new Dictionary<Type, Singletone>();
    private List<Singletone> m_dictManager = new List<Singletone>();

    protected SingletonManager()
    {
        m_dictManager.Add(new CTaskManager());
        m_dictManager.Add(new CResourceManager());
        m_dictManager.Add(new CConfigManager());
        m_dictManager.Add(new CUIManager());
        m_dictManager.Add(new CModelManager());
        m_dictManager.Add(new CCharacterManager());
        m_dictManager.Add(new CSceneManager());
        m_dictManager.Add(new CInputManager());

        //m_dictManager.Add(typeof(CResourceManager),  new CResourceManager());
        //m_dictManager.Add(typeof(CConfigManager), new CConfigManager());
        //m_dictManager.Add(typeof(CUIManager), new CUIManager());
        //m_dictManager.Add(typeof(CModelManager), new CModelManager());
        //m_dictManager.Add(typeof(CCharacterManager), new CCharacterManager());
        //m_dictManager.Add(typeof(CSceneManager), new CSceneManager());
        //m_dictManager.Add(typeof(CInputManager), new CInputManager());
        //m_dictManager.Add(typeof(CCoroutineManager), new CCoroutineManager());
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
        for (int i = 0; i < m_dictManager.Count; ++i)
        {
            m_dictManager[i].Initialize();
        }

        GetManager<CUIManager>().SetUIRoot(m_GameMain.m_uiRoot);

        //foreach (KeyValuePair<Type, Singletone> kvp in m_dictManager)
        //{
        //    kvp.Value.Initialize();
        //}
    }

    public void Uninitialize()
    {
        for (int i = 0; i < m_dictManager.Count; ++i)
        {
            m_dictManager[i].Uninitialize();
        }
        //foreach (KeyValuePair<Type, Singletone> kvp in m_dictManager)
        //{
        //    kvp.Value.Uninitialize();
        //}

    }

    public void Update()
    {
        for (int i = 0; i < m_dictManager.Count; ++i)
        {
            m_dictManager[i].Update();
        }
        //Dictionary<Type, Singletone>.Enumerator iter = m_dictManager.GetEnumerator();
        //while(iter.MoveNext())
        //{
        //    iter.Current.Value.Update();
        //}
    }

    public T GetManager<T>() where T : Singletone//MonoBehaviour
    {
        if (typeof(T) == typeof(CTaskManager))
        {
            return m_dictManager[0] as T;
        }
        else if (typeof(T) == typeof(CResourceManager))
        {
            return m_dictManager[1] as T;
        }
        else if (typeof(T) == typeof(CConfigManager))
        {
            return m_dictManager[2] as T;
        }
        else if (typeof(T) == typeof(CUIManager))
        {
            return m_dictManager[3] as T;
        }
        else if (typeof(T) == typeof(CModelManager))
        {
            return m_dictManager[4] as T;
        }
        else if (typeof(T) == typeof(CCharacterManager))
        {
            return m_dictManager[5] as T;
        }
        else if (typeof(T) == typeof(CSceneManager))
        {
            return m_dictManager[6] as T;
        }
        else if (typeof(T) == typeof(CInputManager))
        {
            return m_dictManager[7] as T;
        }
        return null;
        //return m_dictManager[typeof(T)] as T;
    }
}