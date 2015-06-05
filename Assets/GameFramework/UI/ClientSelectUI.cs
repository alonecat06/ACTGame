using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClientSelectUI : MonoBehaviour 
{
    PhotoStage psSinglePlayer;

    private bool m_bTakeWeapon = true;
    private bool m_bTakeShield = true;
    private bool m_bTakeShoulderArmor = true;

	void Start () 
    {
        SetupOnClickListener("btnMale", OnClickMaleBtn);
        SetupOnClickListener("btnFemale", OnClickFemaleBtn);

        SetupOnClickListener("btnWeaponFish", OnClickWeaponFishBtn); 
        SetupOnClickListener("btnWeaponSword", OnClickWeaponSwordBtn);
        SetupOnClickListener("btnWeaponSpade", OnClickWeaponSpadeBtn);
        SetupOnClickListener("btnWeaponWand", OnClickWeaponWandBtn);

        SetupOnClickListener("btnLevel1", OnClickLevel1);
        SetupOnClickListener("btnLevel2", OnClickLevel2);

        if (psSinglePlayer == null)
        {
            GameObject temp = GameObject.Find("psSinglePlayer");
            if (temp != null)
            {
                psSinglePlayer = temp.GetComponent<PhotoStage>();
                if (psSinglePlayer == null)
                {
                    Debug.LogError("没有得到PhotoStage脚本");
                }
            }
        }     
    }

    private void SetupOnClickListener(string strCtrlName, UUIEventListener.VoidDelegate deleg)
    {
        GameObject btnServer = GameObject.Find(strCtrlName);
        if (btnServer != null)
        {
            UUIEventListener.Get(btnServer).onClick = deleg;
        }
    }

    private void OnClickMaleBtn(GameObject go)
    {
        OnChangePlayer(0);
    }
    private void OnClickFemaleBtn(GameObject go)
    {
        OnChangePlayer(1);
    }
    private void OnChangePlayer(int iPlayerType)
    {
        if (iPlayerType == 0)
        {
            psSinglePlayer.LoadCharaterToStage(CharaterType.CharaterType_Player, 100);
        } 
        else if (iPlayerType == 1)
        {
            psSinglePlayer.LoadCharaterToStage(CharaterType.CharaterType_Player, 101);
        }
    }

    private void OnClickWeaponFishBtn(GameObject go)
    {
        OnChangePlayerWeapon(0, m_bTakeWeapon);
        m_bTakeWeapon = !m_bTakeWeapon;
    }
    private void OnClickWeaponSwordBtn(GameObject go)
    {
        OnChangePlayerWeapon(1, m_bTakeWeapon);
        m_bTakeWeapon = !m_bTakeWeapon;
    }
    private void OnClickWeaponSpadeBtn(GameObject go)
    {
        OnChangePlayerWeapon(2, m_bTakeShield);
        m_bTakeShield = !m_bTakeShield;
    }
    private void OnClickWeaponWandBtn(GameObject go)
    {
        OnChangePlayerWeapon(3, m_bTakeShoulderArmor);
        m_bTakeShoulderArmor = !m_bTakeShoulderArmor;
    }
    private void OnChangePlayerWeapon(int iWeaponId, bool bTake)
    {
        GameObject goPlayer = psSinglePlayer.GetCharaterOnStage();
        if (goPlayer == null 
            || goPlayer.GetComponent<PlayerAppearanceController>() == null)
        {
            Debug.LogError("不能得到人物外观脚本");
            return;
        }

        PlayerAppearanceController ctrlAppear = goPlayer.GetComponent<PlayerAppearanceController>();
        ActionCommandInput ctrlAnimation = goPlayer.GetComponent<ActionCommandInput>();

        if (iWeaponId == 0)
        {
            if (bTake)
            {
                ctrlAppear.ChangeWeapon(500);
            } 
            else
            {
                ctrlAppear.ChangeWeapon(0);
            }
        }
        else if (iWeaponId == 1)
        {
            if (bTake)
            {
                ctrlAppear.ChangeWeapon(600);
            } 
            else
            {
                ctrlAppear.ChangeWeapon(0);
            }
        }
        else if (iWeaponId == 2)
        {
            if (bTake)
            {
                ctrlAppear.ChangeShield(700);
            } 
            else
            {
                ctrlAppear.ChangeShield(0);
            }
        }
        else if (iWeaponId == 3)
        {
            if (bTake)
            {
                ctrlAppear.ChangeShoulderArm(800);
            } 
            else
            {
                ctrlAppear.ChangeShoulderArm(0);
            }
        }

        ctrlAnimation.m_LogicController.Attack01();
    }

    private void OnClickLevel1(GameObject go)
    {
        SingletonManager.Inst.GetManager<CUIManager>().UnloadUI(gameObject.name.GetHashCode());
        SingletonManager.Inst.GetManager<CSceneManager>().LoadScene(1);
    }
    private void OnClickLevel2(GameObject go)
    {
        SingletonManager.Inst.GetManager<CUIManager>().UnloadUI(gameObject.name.GetHashCode());
        SingletonManager.Inst.GetManager<CSceneManager>().LoadScene(2);
    }
}
