using UnityEngine;
using System.Collections;

public class CharacterAppearance : MonoBehaviour {

    public GameObject m_goMountRHandWeapon;
    public GameObject m_goMountLHandShield;
    public GameObject m_goMountLArmShield;

    private GameObject m_goRHandWeapon;
    private GameObject m_goLHandShield;
    private GameObject m_goLArmShield;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (m_goMountRHandWeapon == null)
            m_goMountRHandWeapon = GameObject.Find("mount_RHand_Weapon");
        if (m_goMountLHandShield == null)
            m_goMountLHandShield = GameObject.Find("mount_LHand_Shield");
        if (m_goMountLArmShield == null)
            m_goMountLArmShield = GameObject.Find("mount_LArm_Shield");

        if (m_goMountRHandWeapon == null
            || m_goMountLHandShield == null
            || m_goMountLArmShield == null)
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogError("挂载点有误");
        } 
    }

    public void ChangeWeapon(uint uWeaponId)
    {
        if (m_goRHandWeapon != null)
        {
            Destroy(m_goRHandWeapon);
        }

        if (uWeaponId != 0)
        {
            SingletonManager.Inst.GetManager<CModelManager>().GetModel(uWeaponId, LoadedWeaponRes, SetWeapon);
        }
    }
    private void LoadedWeaponRes(CResource res)
    {
        GameObject goWeapon = (GameObject)res.AssetBundle.LoadAsset(res.ResourceName, typeof(GameObject)); //res.MainAsset as GameObject;
        if (goWeapon == null)
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogError("加载不了武器：" + res.ResId);
        }
        goWeapon = GameObject.Instantiate(goWeapon);

        SetWeapon(goWeapon);
    }
    private void SetWeapon(GameObject goWeapon)
    {
        goWeapon.transform.SetParent(m_goMountRHandWeapon.transform);
        goWeapon.transform.localPosition = new Vector3(0, 0, 0);
        goWeapon.transform.localRotation = new Quaternion(0, 0, 0, 1);
        goWeapon.transform.localScale = new Vector3(1, 1, 1);

        m_goRHandWeapon = goWeapon;
    }

    public void ChangeShield(uint uShield)
    {
        if (m_goLHandShield != null)
        {
            Destroy(m_goLHandShield);
        }

        if (uShield != 0)
        {
            SingletonManager.Inst.GetManager<CModelManager>().GetModel(uShield, LoadedShieldRes, SetShield);
        }
    }
    private void LoadedShieldRes(CResource res)
    {
        GameObject goShield = (GameObject)res.AssetBundle.LoadAsset(res.ResourceName, typeof(GameObject)); //res.MainAsset as GameObject;
        if (goShield == null)
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogError("加载不了主角：" + res.ResId);
        }
        goShield = GameObject.Instantiate(goShield);
        SetShield(goShield);
    }
    private void SetShield(GameObject goShield)
    {
        goShield.transform.SetParent(m_goMountLHandShield.transform);
        goShield.transform.localPosition = new Vector3(0, 0, 0);
        goShield.transform.localRotation = new Quaternion(0, 0, 0, 1);
        goShield.transform.localScale = new Vector3(1, 1, 1);

        m_goLHandShield = goShield;
    }

    public void ChangeShoulderArm(uint uShoulderArmor)
    {
        if (m_goLArmShield != null)
        {
            Destroy(m_goLArmShield);
        }

        if (uShoulderArmor != 0)
        {
            SingletonManager.Inst.GetManager<CModelManager>().GetModel(uShoulderArmor, LoadedShoulderArmor, SetShoulderArmor);
        }
    }
    private void LoadedShoulderArmor(CResource res)
    {
        GameObject goShoulderArmor = (GameObject)res.AssetBundle.LoadAsset(res.ResourceName, typeof(GameObject)); //res.MainAsset as GameObject;
        if (goShoulderArmor == null)
        {
            SingletonManager.Inst.GetManager<CLogManager>().LogError("加载不了ShoulderArm：" + res.ResId);
        }
        goShoulderArmor = GameObject.Instantiate(goShoulderArmor);

        SetShoulderArmor(goShoulderArmor);
    }
    private void SetShoulderArmor(GameObject goShoulderArmor)
    {
        goShoulderArmor.transform.SetParent(m_goMountLArmShield.transform);
        goShoulderArmor.transform.localPosition = new Vector3(0, 0, 0);
        goShoulderArmor.transform.localRotation = new Quaternion(0, 0, 0, 1);
        goShoulderArmor.transform.localScale = new Vector3(1, 1, 1);

        m_goLArmShield = goShoulderArmor;
    }

    public void SetupCharacter(CharacterSetting setting)
    {
        uint uEquipId;
        if (setting.m_dictMountPos.TryGetValue(EquipType.EquipType_RHandWeapon, out uEquipId))
        {
            EquipCfg cfgEquip = SingletonManager.Inst.GetManager<CConfigManager>().GetConfigProvider<EquipCfgConfigProvider>().GetEquipCfg(uEquipId);
            if (cfgEquip == null)
            {
                return;
            }
            ChangeWeapon(cfgEquip.uResId);
        }
        if (setting.m_dictMountPos.TryGetValue(EquipType.EquipType_LHandShield, out uEquipId))
        {
            EquipCfg cfgEquip = SingletonManager.Inst.GetManager<CConfigManager>().GetConfigProvider<EquipCfgConfigProvider>().GetEquipCfg(uEquipId);
            if (cfgEquip == null)
            {
                return;
            }
            ChangeShield(cfgEquip.uResId);
        }
        if (setting.m_dictMountPos.TryGetValue(EquipType.EquipType_LArmShield, out uEquipId))
        {
            EquipCfg cfgEquip = SingletonManager.Inst.GetManager<CConfigManager>().GetConfigProvider<EquipCfgConfigProvider>().GetEquipCfg(uEquipId);
            if (cfgEquip == null)
            {
                return;
            }
            ChangeShoulderArm(cfgEquip.uResId);
        }
    }
}
