using UnityEngine;
using System.Collections;

public class ActionLogicController
{
    private CharacterController m_CharaterController;
    public ActionAnimatorController m_AnimatorController;

    private float m_fWalkSpeed;
    private float m_fRunSpeed;

    private Vector3 m_vec3RunDirect;

    private float m_fPlaySpeed;

    public ActionLogicController(CharacterController charaterController, Animator animator) 
    {
        m_CharaterController = charaterController;
        m_AnimatorController = new ActionAnimatorController(animator);

        m_fWalkSpeed = 3.0f;
        m_fRunSpeed = 10.0f;
        m_vec3RunDirect = new Vector3(0, 0, 0);

        m_fPlaySpeed = 1.0f;
	}
	
    //// Update is called once per frame
    //void Update () {

    //}

    #region 网络部分
    //向服务器发送指令进行验证

    //处理服务器返回结果（主要包括扣血，吃buffer）
    #endregion

    #region 战斗数据FightData部分
    //查询战斗数据

    //通知战斗数据系统进行数据修改
    #endregion

    #region 人物移动
    private bool CanWalk(float fGlobalX, float fGlobalZ)
    {
        return true;
    }
    public void Walk(bool bWalk, float fLocHoz, float fLocVet, Transform transCharater)
    {
        if (bWalk)
        {
            if (CanWalk(transCharater.position.x, transCharater.position.z))
            {
                //移动角色
                Vector3 targetDir = new Vector3(fLocHoz, 0, fLocVet);
                targetDir = targetDir + transCharater.position;
                m_CharaterController.SimpleMove(targetDir * m_fWalkSpeed);

                //播放动画
                m_AnimatorController.Walk(bWalk, fLocHoz, fLocVet, m_fPlaySpeed);
            }
        } 
        else
        {
            //停止播放动画
            m_AnimatorController.Walk(bWalk, 0, 0, 1);
        }
    }

    private bool CanRun(float fGlobalX, float fGlobalZ)
    {
        return true;
    }
    public void Run(bool bRun, float fLocHoz, float fLocVet, Transform transCharater)
    {
        Vector3 vec3Input = new Vector3(fLocHoz, 0, fLocVet);
        if (bRun)
        {
            if (CanRun(transCharater.position.x, transCharater.position.z))
            {
                //移动角色
                if (m_vec3RunDirect != vec3Input)
                {
                    m_vec3RunDirect = vec3Input;
                    m_vec3RunDirect += transCharater.position;
                    transCharater.LookAt(m_vec3RunDirect);
                }

                m_CharaterController.SimpleMove(m_vec3RunDirect * m_fRunSpeed);

                //播放动画
                m_AnimatorController.ActRun(bRun, m_fPlaySpeed);
            }
        }
        else
        {
            m_vec3RunDirect = vec3Input;

            //停止播放动画
            m_AnimatorController.ActRun(bRun, 1);
        }

    }

    private bool CanJump(float fGlobalX, float fGlobalZ, float fSpeedY)
    {
        return true;
    }
    public void Jump(float fY, float fSpeed, Transform transCharater)
    {
        if (CanRun(transCharater.position.x, transCharater.position.z))
        {
            m_AnimatorController.ActJump();
        }
    }

    private bool CanIdle(float fGlobalX, float fGlobalZ)
    {
        return true;
    }
    public void Idle(bool bIdle, Transform transCharater)
    {
        if (bIdle)
        {
            if (CanRun(transCharater.position.x, transCharater.position.z))
            {
                m_AnimatorController.ActIdle(bIdle);
            }
        } 
        else
        {
            m_AnimatorController.ActIdle(bIdle);
        }
    }
    #endregion

    #region 出招检测
    private bool IsCombo()
    {
        return false;
    }

    private bool CanAttack01()
    {
        return true;
    }
    private bool CanAttack02()
    {
        return true;
    }
    private bool CanAttack03()
    {
        return true;
    }
    private bool CanAttack04()
    {
        return true;
    }

    public void Attack01()
    {
        if (CanAttack01())
        {
            //可以写一些辅助追尾自动对位功能的实现

            //根据招式的需求移动人物的Root motion

            if (CanAttack02() && IsCombo())
            {
                m_AnimatorController.LaunchAttack02();
            } 
            else
            {
                m_AnimatorController.LaunchAttack01();
            }
        }
    }
    public void Attack02()
    {
        //可以写一些辅助追尾自动对位功能的实现

        //根据招式的需求移动人物的Root motion

        if (CanAttack02())
        {
            m_AnimatorController.LaunchAttack02();
        }
    }
    public void Attack03()
    {
        //可以写一些辅助追尾自动对位功能的实现

        //根据招式的需求移动人物的Root motion

        if (CanAttack03())
        {
            m_AnimatorController.LaunchAttack03();
        }
    }
    public void Attack04()
    {
        //可以写一些辅助追尾自动对位功能的实现

        //根据招式的需求移动人物的Root motion

        if (CanAttack04())
        {
            m_AnimatorController.LaunchAttack04();
        }
    }
    #endregion

    #region 闪避检测
    private bool CanDodge()
    {
        return true;
    }

    public void DodgeBack()
    {
        if (CanDodge())
        {
            //做一些后退的位移

            m_AnimatorController.LaunchAvoidBack();
        }
    }
    public void DodgeLeft()
    {
        //做一些左移的位移

        m_AnimatorController.LaunchAvoidLeft();
    }
    public void DodgeRight()
    {
        //做一些右移的位移

        m_AnimatorController.LaunchAvoidBack();
    }
    #endregion

    #region 受击检测
    public bool CheckAttack()
    {
        //处于攻击过程

        //
        return false;
    }

    public void TakeDamage()
    {

    }
    private void Die()
    {

    }
    #endregion
}
