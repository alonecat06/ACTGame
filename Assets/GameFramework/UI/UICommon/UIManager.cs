using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public enum UIId
//{
//    UIId_EnterGame,
//    UIId_ClientSelect,
//}

public class CUIManager : Singletone
{

    private GameObject m_uiRoot;

    private Dictionary<int, GameObject> m_dictUIPanel = new Dictionary<int, GameObject>();
    private Dictionary<int, uint> m_dictUIResId = new Dictionary<int, uint>();

    public override bool Initialize()
    {
        m_uiRoot = GameObject.Find("UIRoot");
        if (m_uiRoot == null)
        {
            Debug.LogError("界面根节点为空");
            return false;
        }
        return true;
    }

    public override bool InitializeData()
    {
        //加载第一个界面
        LoadUI(1, LoadAnimation.LoadAnimation_WholeScreen);
        return true;
    }

    //public void SetUIRoot(GameObject goUiRoot)
    //{
    //    m_uiRoot = goUiRoot;
    //}

    public CResource LoadUI(uint uUIId, LoadAnimation eLoadAnimation)
    {
        //显示加载动画
        SingletonManager.Inst.GameMain.OpenLoadingAnimation(eLoadAnimation);

        return SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(uUIId
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

    private void FinishLoadingUI(CResource res)
    {
        SingletonManager.Inst.GameMain.CloseLoadingAnimation();

        GameObject gobj = (GameObject)res.AssetBundle.LoadAsset(res.ResourceName, typeof(GameObject));
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
        m_dictUIResId.Add(gobj.name.GetHashCode(), res.ResId);
    }
}
