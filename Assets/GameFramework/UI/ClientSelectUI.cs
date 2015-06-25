using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClientSelectUI : UIPanelBase 
{
    PhotoStage psSinglePlayer;

    CharacterSetting m_SettingMaster;

	public override void Start () 
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

        base.Start();
    }

    public override bool InitializeUI()
    {
        CharacterSetting playerSetting = SingletonManager.Inst.GetManager<CCharacterManager>().GetPlayerSetting(0);
        if (playerSetting == null)
        {
            return false;
        }

        OnChangePlayer(playerSetting.m_uCharaterId);
        foreach (uint uEquipId in playerSetting.m_dictMountPos.Values)
        {
            OnChangePlayerWeapon(uEquipId);
        }

        return true;
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
    private void OnChangePlayer(uint uCharaterId)
    {
        CharacterSetting playerSetting = SingletonManager.Inst.GetManager<CCharacterManager>().GetPlayerSetting(0);
        if (uCharaterId == 0)
        {
            psSinglePlayer.LoadCharaterToStage(CharaterType.CharaterType_Player, 100);
        } 
        else if (uCharaterId == 1)
        {
            psSinglePlayer.LoadCharaterToStage(CharaterType.CharaterType_Player, 101);
        }
        playerSetting.m_uCharaterId = uCharaterId;
        //m_bTakeWeapon = false;
        //m_bTakeShield = false;
        //m_bTakeShoulderArmor = false;
    }

    private void OnClickWeaponFishBtn(GameObject go)
    {
        OnChangePlayerWeapon(1);
    }
    private void OnClickWeaponSwordBtn(GameObject go)
    {
        OnChangePlayerWeapon(2);
    }
    private void OnClickWeaponSpadeBtn(GameObject go)
    {
        OnChangePlayerWeapon(20);
    }
    private void OnClickWeaponWandBtn(GameObject go)
    {
        OnChangePlayerWeapon(40);
    }
    private void OnChangePlayerWeapon(uint uEquipId)
    {
        GameObject goPlayer = psSinglePlayer.GetCharaterOnStage();
        if (goPlayer == null 
            || goPlayer.GetComponent<CharacterAppearance>() == null)
        {
            Debug.LogError("不能得到人物外观脚本");
            return;
        }

        CharacterAppearance ctrlAppear = goPlayer.GetComponent<CharacterAppearance>();

        EquipCfg cfgEquip = SingletonManager.Inst.GetManager<CConfigManager>().GetConfigProvider<EquipCfgConfigProvider>().GetEquipCfg(uEquipId);
        if (cfgEquip == null)
        {
            return;
        }

        CharacterSetting playerSetting = SingletonManager.Inst.GetManager<CCharacterManager>().GetPlayerSetting(0);
        if (playerSetting == null)
        {
            return;
        }

        if (cfgEquip.eEquipType == EquipType.EquipType_RHandWeapon)
        {
            uint uCurrEquipId;
            if (!playerSetting.m_dictMountPos.TryGetValue(EquipType.EquipType_RHandWeapon, out uCurrEquipId) || uCurrEquipId != uEquipId)
            {
                ctrlAppear.ChangeWeapon(cfgEquip.uResId);
                playerSetting.m_dictMountPos[EquipType.EquipType_RHandWeapon] = uEquipId;
            } 
            else
            {
                ctrlAppear.ChangeWeapon(0);
                playerSetting.m_dictMountPos.Remove(EquipType.EquipType_RHandWeapon);
            }
        }
        else if (cfgEquip.eEquipType == EquipType.EquipType_LHandShield)
        {
            uint uCurrEquipId;
            if (!playerSetting.m_dictMountPos.TryGetValue(EquipType.EquipType_LHandShield, out uCurrEquipId) || uCurrEquipId != uEquipId)
            {
                ctrlAppear.ChangeShield(cfgEquip.uResId);
                playerSetting.m_dictMountPos[EquipType.EquipType_LHandShield] = uEquipId;
            } 
            else
            {
                ctrlAppear.ChangeShield(0);
                playerSetting.m_dictMountPos.Remove(EquipType.EquipType_LHandShield);
            }
        }
        else if (cfgEquip.eEquipType == EquipType.EquipType_LArmShield)
        {
            uint uCurrEquipId;
            if (!playerSetting.m_dictMountPos.TryGetValue(EquipType.EquipType_LArmShield, out uCurrEquipId) || uCurrEquipId != uEquipId)
            {
                ctrlAppear.ChangeShoulderArm(cfgEquip.uResId);
                playerSetting.m_dictMountPos[EquipType.EquipType_LArmShield] = uEquipId;
            } 
            else
            {
                ctrlAppear.ChangeShoulderArm(0);
                playerSetting.m_dictMountPos.Remove(EquipType.EquipType_LArmShield);
            }
        }

        psSinglePlayer.Attack01();
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
