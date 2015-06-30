using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems; 

public class PlayerManipulateUI : UIPanelBase
{
    private bool m_bDragging = false;

    public override void Start()
    {
        GameObject btnJoystick = GameObject.Find("btnJoystick");
        if (btnJoystick != null)
        {
            //UUIEventListener.Get(btnJoystick).onDown = OnDownJoystick;
            UUIEventListener.Get(btnJoystick).onDrag = OnDragJoystick;
            UUIEventListener.Get(btnJoystick).onDragEnd = OnDragEndJoystick;

            UUIEventListener.Get(btnJoystick).onEnter = OnPlayerManipulateEnter;
            UUIEventListener.Get(btnJoystick).onExit = OnPlayerManipulateExit;
        }
        GameObject btnNormal = GameObject.Find("btnNormal");
        if (btnNormal != null)
        {
            UUIEventListener.Get(btnNormal).onClick = OnClickNormal;
            UUIEventListener.Get(btnNormal).onEnter = OnPlayerManipulateEnter;
            UUIEventListener.Get(btnNormal).onExit = OnPlayerManipulateExit;
        }
        GameObject btnSkill1 = GameObject.Find("btnSkill1");
        if (btnSkill1 != null)
        {
            UUIEventListener.Get(btnSkill1).onClick = OnClickSkill1;
            UUIEventListener.Get(btnSkill1).onEnter = OnPlayerManipulateEnter;
            UUIEventListener.Get(btnSkill1).onExit = OnPlayerManipulateExit;
        }
        GameObject btnSkill2 = GameObject.Find("btnSkill2");
        if (btnSkill2 != null)
        {
            UUIEventListener.Get(btnSkill2).onClick = OnClickSkill2;
            UUIEventListener.Get(btnSkill2).onEnter = OnPlayerManipulateEnter;
            UUIEventListener.Get(btnSkill2).onExit = OnPlayerManipulateExit;
        }
        GameObject btnSkill3 = GameObject.Find("btnSkill3");
        if (btnSkill3 != null)
        {
            UUIEventListener.Get(btnSkill3).onClick = OnClickSkill3;
            UUIEventListener.Get(btnSkill3).onEnter = OnPlayerManipulateEnter;
            UUIEventListener.Get(btnSkill3).onExit = OnPlayerManipulateExit;
        }

        base.Start();
    }

    public override void Update()
    {
        if (m_bDragging)
        {
            GameObject go = GameObject.Find("btnJoystick");
            float fDistanceSqrt = Mathf.Pow(go.transform.localPosition.x, 2.0f) + Mathf.Pow(go.transform.localPosition.y, 2.0f);
            float h = go.transform.localPosition.x;
            float v = go.transform.localPosition.y;

            if (fDistanceSqrt > 3600)
            {
                if (fDistanceSqrt > 10000)
                {//移动底盘
                    float fDistance = Mathf.Sqrt(fDistanceSqrt);
                    Vector3 vecPlate = go.transform.parent.transform.position;
                    vecPlate = vecPlate + new Vector3(h - 100 * h / fDistance, v - 100 * v / fDistance, 0);
                    go.transform.parent.transform.position = vecPlate;
                }

                h = h < 100 ? h / 100 : 1;
                v = v < 100 ? v / 100 : 1;
                SingletonManager.Inst.GetManager<CInputManager>().UIDragJoystick(h, v, true);
            }
            else
            {
                h = h < 60 ? h / 60 : 1;
                v = v < 60 ? v / 60 : 1;
                SingletonManager.Inst.GetManager<CInputManager>().UIDragJoystick(h, v, false);
            }
        }
    }

    private void OnPlayerManipulateEnter(GameObject go, PointerEventData eventData)
    {
        SingletonManager.Inst.GetManager<CCameraManager>().EnableCameraControl(false);
    }

    private void OnPlayerManipulateExit(GameObject go, PointerEventData eventData)
    {
        SingletonManager.Inst.GetManager<CCameraManager>().EnableCameraControl(true);
    }

    //private void OnDownJoystick(GameObject go, PointerEventData eventData)
    //{
    //    SingletonManager.Inst.GetManager<CLogManager>().Log("OnDown x:" + go.transform.localPosition.x + "y:" + go.transform.localPosition.y + "z:" + go.transform.localPosition.z);
    //}

    private void OnDragJoystick(GameObject go, PointerEventData eventData)
    {
        m_bDragging = true;

        go.transform.position = new Vector3(eventData.position.x, eventData.position.y, go.transform.position.z);
    }

    private void OnDragEndJoystick(GameObject go, PointerEventData eventData)
    {
        m_bDragging = false;
        go.transform.parent.position = new Vector3(100, 100, go.transform.parent.localPosition.z);
        go.transform.localPosition = new Vector3(0, 0, go.transform.localPosition.z);

        SingletonManager.Inst.GetManager<CInputManager>().UIDragJoystick(0, 0, false);
    }

    private void OnClickNormal(GameObject go, PointerEventData eventData)
    {
        SingletonManager.Inst.GetManager<CInputManager>().UIPressSkillBtn(1);
    }

    private void OnClickSkill1(GameObject go, PointerEventData eventData)
    {
        SingletonManager.Inst.GetManager<CInputManager>().UIPressSkillBtn(2);
    }

    private void OnClickSkill2(GameObject go, PointerEventData eventData)
    {
        SingletonManager.Inst.GetManager<CInputManager>().UIPressSkillBtn(3);
    }

    private void OnClickSkill3(GameObject go, PointerEventData eventData)
    {
        SingletonManager.Inst.GetManager<CInputManager>().UIPressSkillBtn(4);
    } 
}
