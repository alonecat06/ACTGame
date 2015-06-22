using UnityEngine;
using System.Collections;

public class ActionAnimatorController
{
    private Animator m_Animator;

    private static int p_trgAttack01 = Animator.StringToHash("trgAttack01");
    private static int p_trgAttack02 = Animator.StringToHash("trgAttack02");
    private static int p_trgAttack03 = Animator.StringToHash("trgAttack03");
    private static int p_trgAttack04 = Animator.StringToHash("trgAttack04");

    private static int p_trgTakeDamage = Animator.StringToHash("trgTakeDamage");

    private static int p_trgAvoidBack = Animator.StringToHash("trgAvoidBack");
    private static int p_trgAvoidLeft = Animator.StringToHash("trgAvoidLeft");
    private static int p_trgAvoidRight = Animator.StringToHash("trgAvoidRight");

    private static int p_bDead = Animator.StringToHash("bDead");
    private static int p_bIdle = Animator.StringToHash("bIdle");

    private static int p_trgJump = Animator.StringToHash("trgJump");
    private static int p_bWalk = Animator.StringToHash("bWalk");
    private static int p_bRun = Animator.StringToHash("bRun");
    //private static int p_fMoveDirect = Animator.StringToHash("m_fMoveDirect");
    private static int p_fLocalXDirect = Animator.StringToHash("fLocalXDirect");
    private static int p_fLocalZDirect = Animator.StringToHash("fLocalZDirect");

	// Use this for initialization
    public ActionAnimatorController(Animator animator) 
    {
        m_Animator = animator;
	}
	
    //// Update is called once per frame
    //void Update () {
	
    //}

    #region 动画控制
    public void LaunchAttack01()
    {
        m_Animator.SetTrigger(p_trgAttack01);
    }
    public void LaunchAttack02()
    {
        m_Animator.SetTrigger(p_trgAttack02);
    }
    public void LaunchAttack03()
    {
        m_Animator.SetTrigger(p_trgAttack03);
    }
    public void LaunchAttack04()
    {
        m_Animator.SetTrigger(p_trgAttack04);
    }

    public void LaunchAvoidBack()
    {
        m_Animator.SetTrigger(p_trgAvoidBack);
    }
    public void LaunchAvoidLeft()
    {
        m_Animator.SetTrigger(p_trgAvoidLeft);
    }
    public void LaunchAvoidRight()
    {
        m_Animator.SetTrigger(p_trgAvoidRight);
    }

    public void Walk(bool bWalk, float fLocX, float fLocZ, float fPlaySpeed)
    {
        m_Animator.SetBool(p_bWalk, bWalk);

        m_Animator.SetFloat(p_fLocalXDirect, fLocX);
        m_Animator.SetFloat(p_fLocalZDirect, fLocZ);
        //Debug.Log("LocX:" + fLocX + ",LocZ:" + fLocZ);

        m_Animator.speed = fPlaySpeed;
    }

    public void ActRun(bool bRun, float fPlaySpeed)
    {
        m_Animator.SetBool(p_bRun, bRun);
        m_Animator.speed = fPlaySpeed;
    }
    public void ActJump()
    {
        m_Animator.SetTrigger(p_trgJump);
    }

    public void ActIdle(bool bIdle)
    {
        m_Animator.SetBool(p_bIdle, bIdle);
    }

    public void ActHit()
    {
        m_Animator.SetTrigger(p_trgTakeDamage);
    }
    public void ActDie()
    {
        m_Animator.SetBool(p_bDead, true);
    }
    #endregion

    #region 动画帧事件回调处理
    #endregion

    #region 动画曲线处理
    #endregion

    #region 特效挂载
    #endregion
}
