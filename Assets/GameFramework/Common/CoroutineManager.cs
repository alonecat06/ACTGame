using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CCoroutineManager : Singletone//MonoBehaviour 
{
    private GameObject m_goCoroutine;
    private MonoBehaviour m_mbCoroutine;

    public override bool Initialize()
    {
        m_goCoroutine = new GameObject();
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
