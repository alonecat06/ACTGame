using UnityEngine;
using System.Collections;

public class PlayerManipulateUI : UIPanelBase
{
    public override void Start()
    {
        GameObject btnJoystick = GameObject.Find("btnJoystick");
        if (btnJoystick != null)
        {
            UUIEventListener.Get(btnJoystick).onDown = OnDownJoystick;
        }
        GameObject btnNormal = GameObject.Find("btnNormal");
        if (btnNormal != null)
        {
            UUIEventListener.Get(btnNormal).onClick = OnClickNormal;
        }
        GameObject btnSkill1 = GameObject.Find("btnSkill1");
        if (btnSkill1 != null)
        {
            UUIEventListener.Get(btnSkill1).onClick = OnClickSkill1;
        }
        GameObject btnSkill2 = GameObject.Find("btnSkill2");
        if (btnSkill2 != null)
        {
            UUIEventListener.Get(btnSkill2).onClick = OnClickSkill2;
        }
        GameObject btnSkill3 = GameObject.Find("btnSkill3");
        if (btnSkill3 != null)
        {
            UUIEventListener.Get(btnSkill3).onClick = OnClickSkill3;
        }

        base.Start();
    }

    private void OnDownJoystick(GameObject go)
    {
    }

    private void OnClickNormal(GameObject go)
    {
    } 

    private void OnClickSkill1(GameObject go)
    {
    }

    private void OnClickSkill2(GameObject go)
    {
    } 

    private void OnClickSkill3(GameObject go)
    {
    } 
}
