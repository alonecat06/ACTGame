﻿using UnityEngine;
using System.Collections;

public class EnterGameUI : MonoBehaviour
{
    private WWW m_bundle;

	// Use this for initialization
    void Start()
    {
        GameObject btnClient = GameObject.Find("btnClient");
        if (btnClient != null)
        {
            UUIEventListener.Get(btnClient).onClick = OnClickClientBtn;
        }

        GameObject btnServer = GameObject.Find("btnServer");
        if (btnServer != null)
        {
            UUIEventListener.Get(btnServer).onClick = OnClickServerBtn;
        }
    }

    private void OnClickClientBtn(GameObject go)
    {
        SingletonManager.Inst.GetManager<CUIManager>().LoadUI(1);
        SingletonManager.Inst.GetManager<CUIManager>().UnloadUI(gameObject.name.GetHashCode());
    }

    private void OnClickServerBtn(GameObject go)
    {
        Debug.Log("您单击服务器端模式");
    } 
}