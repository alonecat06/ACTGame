using UnityEngine;
using System.Collections;

public class ActionCommandInput : MonoBehaviour
{
    public CCharacter m_Player;
    public ActionLogicController m_LogicController;

    public bool NeedUpdate
    {
        get;
        set;
    }

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("得不到角色的Animator");
            return;
        }
        CharacterController cc = GetComponent<CharacterController>();
        if (cc == null)
        {
            Debug.LogError("得不到角色的CharacterController");
            return;
        }
        m_LogicController = new ActionLogicController(cc, animator);
    }

    void Update()
    {
        if (NeedUpdate == false)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        ////按键的取值，以虚拟杆中的值为优先
        //if (Joystick.h != 0 || Joystick.v != 0)
        //{
        //    h = Joystick.h; v = Joystick.v;
        //}

        if (InputRun() && v >= 0.0f)
        {
            m_LogicController.Run(true, h, v, transform);
        }
        else
        {
        //if (!InputRun())
        //{
            m_LogicController.Run(false, h, v, transform);
        //}
            if (Mathf.Abs(h) <= 0.001 && Mathf.Abs(v) <= 0.001)
            {
                m_LogicController.Walk(false, h, v, transform);
            } 
            else
            {
                m_LogicController.Walk(true, h, v, transform);
            }
        }       
    }

    //void FixedUpdate()
    //{
    //    float h = Input.GetAxis("Horizontal");
    //    float v = Input.GetAxis("Vertical");
    //    if (InputRun() && (Mathf.Abs(h) > 0.001 || Mathf.Abs(v) > 0.001))
    //    {
    //        m_LogicController.Run(true, h, v, transform);
    //    }
    //}

    void LateUpdate()
    {
        m_LogicController.CheckAttack();
    }

    private bool InputRun()
    {
        return Input.GetButton("Fire3");
    }
}
