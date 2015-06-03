using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CConfigManager : MonoBehaviour 
{
    Dictionary<Type, IConfigProvider> m_dictConfigProvider = new Dictionary<Type, IConfigProvider>();

    //void Start ()
    //{

    //}

    public T GetConfigProvider<T>() where T : IConfigProvider
    {
        return m_dictConfigProvider[typeof(T)] as T;
    }
}
