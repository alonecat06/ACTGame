using UnityEngine;
using System.Collections;

public class EnterGameUI : UIPanelBase
{
    public override void Start()
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

        base.Start();
    }

    private void OnClickClientBtn(GameObject go)
    {
        SingletonManager.Inst.GetManager<CUIManager>().LoadUI(UIId.UIId_ClientSelect, LoadAnimation.LoadAnimation_Icon);
        SingletonManager.Inst.GetManager<CUIManager>().UnloadUI(gameObject.name.GetHashCode());
    }

    private void OnClickServerBtn(GameObject go)
    {
        Debug.Log("您单击服务器端模式");
    } 
}
