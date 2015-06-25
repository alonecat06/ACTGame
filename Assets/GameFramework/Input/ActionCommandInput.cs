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
    public void Move(float fKeyboardH, float fKeyboardV, bool bRun)
    {
        float h = fKeyboardH;
        float v = fKeyboardV;

        if (bRun && v >= 0.0f)
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

    public void Attack(uint uSkillIdx)
    {
        if (uSkillIdx == 1)
        {
            m_LogicController.Attack01();
        }
        else if (uSkillIdx == 2)
        {
            m_LogicController.Attack02();
        }
        else if (uSkillIdx == 3)
        {
            m_LogicController.Attack03();
        }
        else if (uSkillIdx == 4)
        {
            m_LogicController.Attack04();
        }
    }

    void LateUpdate()
    {
        m_LogicController.CheckAttack();
    }
}
