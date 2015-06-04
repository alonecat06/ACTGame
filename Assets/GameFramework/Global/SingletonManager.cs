using System;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager
{
    private Dictionary<Type, MonoBehaviour> m_dictManager;

    protected SingletonManager()
    {
        GameObject goWorld = GameObject.Find("GameWorld");

        m_dictManager = new Dictionary<Type, MonoBehaviour>();

        //获取各种管理类的引用
        m_dictManager.Add(typeof(CResourceManager), goWorld.GetComponent<CResourceManager>());
        m_dictManager.Add(typeof(CConfigManager), goWorld.GetComponent<CConfigManager>());
        m_dictManager.Add(typeof(CUIManager), goWorld.GetComponent<CUIManager>());
        m_dictManager.Add(typeof(CModelManager), goWorld.GetComponent<CModelManager>());
        m_dictManager.Add(typeof(CCharacterManager), goWorld.GetComponent<CCharacterManager>());
        m_dictManager.Add(typeof(CSceneManager), goWorld.GetComponent<CSceneManager>());
        m_dictManager.Add(typeof(CInputManager), goWorld.GetComponent<CInputManager>());
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

    public T GetManager<T>() where T : MonoBehaviour
    {
        return m_dictManager[typeof(T)] as T;
    }
}