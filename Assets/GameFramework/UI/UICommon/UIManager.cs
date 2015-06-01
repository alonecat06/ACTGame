﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public enum UIId
//{
//    UIId_EnterGame,
//    UIId_ClientSelect,
//}

public class CUIManager : MonoBehaviour {

    private GameObject m_uiRoot;

    private Dictionary<int, GameObject> m_dictUIPanel = new Dictionary<int, GameObject>();
    private Dictionary<int, uint> m_dictUIResId = new Dictionary<int, uint>();

    //void Start () 
    //{
	
    //}

    //void Update () {
	
    //}

    public void SetUIRoot(GameObject goUiRoot)
    {
        m_uiRoot = goUiRoot;
    }

    public void LoadUI(uint uUIId)
    {
        SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(uUIId
            , FinishLoadingUI);
    }
    public void UnloadUI(int iUINameHashCode)
    {
        GameObject goForRemove;
        if (m_dictUIPanel.TryGetValue(iUINameHashCode, out goForRemove))
        {
            m_dictUIPanel.Remove(iUINameHashCode);
            goForRemove.transform.SetParent(null);
            Object.Destroy(goForRemove);
        }
        uint uResIdForRemove;
        if (m_dictUIResId.TryGetValue(iUINameHashCode, out uResIdForRemove))
        {
            m_dictUIResId.Remove(iUINameHashCode);
            SingletonManager.Inst.GetManager<CResourceManager>().UnloadResource(uResIdForRemove);
            Object.Destroy(goForRemove);
        }
    }

    private void FinishLoadingUI(Object go, uint uResId)
    //private void FinishLoadingUI(CResource Res)
    {
        GameObject gobj = go as GameObject;
        //GameObject gobj = Res.MainAsset as GameObject;
        if (gobj == null)
        {
            return;
        }
        gobj = GameObject.Instantiate(gobj);
        gobj.transform.SetParent(m_uiRoot.transform);
        gobj.transform.localScale = new Vector3(1, 1, 1);
        gobj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        gobj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

        m_dictUIPanel.Add(gobj.name.GetHashCode(), gobj);
        m_dictUIResId.Add(gobj.name.GetHashCode(), uResId);
        //m_dictUIResId.Add(gobj.name.GetHashCode(), Res.ResId);
    }
}