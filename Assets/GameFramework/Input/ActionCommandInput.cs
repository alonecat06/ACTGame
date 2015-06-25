using UnityEngine;
using System.Collections;

public class ActionCommandInput
{
    public CCharacter m_Player;
    public Transform m_transPlayer;
    public ActionLogicController m_LogicController;

    public ActionCommandInput(CCharacter master)
    {
        m_Player = master;
        m_transPlayer = master.m_goCharacter.transform;

        Animator animator = master.m_goCharacter.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("得不到角色的Animator");
            return;
        }
        CharacterController cc = master.m_goCharacter.GetComponent<CharacterController>();
        if (cc == null)
        {
            Debug.LogError("得不到角色的CharacterController");
            return;
        }
        m_LogicController = new ActionLogicController(cc, animator);
    }
    public ActionCommandInput(GameObject goCharacter)
    {
        //m_Player = null;
        m_transPlayer = goCharacter.transform;

        Animator animator = goCharacter.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("得不到角色的Animator");
            return;
        }
        CharacterController cc = goCharacter.GetComponent<CharacterController>();
        if (cc == null)
        {
            Debug.LogError("得不到角色的CharacterController");
            return;
        }
        m_LogicController = new ActionLogicController(cc, animator);
    }

    //void Start()
    //{
    //    Animator animator = GetComponent<Animator>();
    //    if (animator == null)
    //    {
    //        Debug.LogError("得不到角色的Animator");
    //        return;
    //    }
    //    CharacterController cc = GetComponent<CharacterController>();
    //    if (cc == null)
    //    {
    //        Debug.LogError("得不到角色的CharacterController");
    //        return;
    //    }
    //    m_LogicController = new ActionLogicController(cc, animator);
    //}

    //void Update()
    public void Move(float fKeyboardH, float fKeyboardV, bool bPressFire3)
    {
        float h = fKeyboardH;
        float v = fKeyboardV;

        ////按键的取值，以虚拟杆中的值为优先
        //if (Joystick.h != 0 || Joystick.v != 0)
        //{
        //    h = Joystick.h; v = Joystick.v;
        //}

        if (bPressFire3 && v >= 0.0f)
        {
            m_LogicController.Run(true, h, v, m_transPlayer);
        }
        else
        {
            m_LogicController.Run(false, h, v, m_transPlayer);
            if (Mathf.Abs(h) <= 0.001 && Mathf.Abs(v) <= 0.001)
            {
                m_LogicController.Walk(false, h, v, m_transPlayer);
            } 
            else
            {
                m_LogicController.Walk(true, h, v, m_transPlayer);
            }
        }       
    }

    void LateUpdate()
    {
        m_LogicController.CheckAttack();
    }
}
