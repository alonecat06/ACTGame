using UnityEngine;
using System.Collections;

public class PlayerAppearanceController : MonoBehaviour {

    public GameObject m_goMountRHandWeapon;
    public GameObject m_goMountLHandShield;
    public GameObject m_goMountLArmShield;

    private GameObject m_goRHandWeapon;
    private GameObject m_goLHandShield;
    private GameObject m_goLArmShield;

    //// Use this for initialization
    //void Start ()
    //{
    //    Debug.Log("运行Start");       
    //}
	
    //// Update is called once per frame
    //void Update () {
	
    //}

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
            Debug.LogError("挂载点有误");
        } 
    }

    public void ChangeWeapon(uint uWeaponId)
    {
        if (m_goRHandWeapon != null)
        {
            Destroy(m_goRHandWeapon);
        }

        SingletonManager.Inst.GetManager<CModelManager>().GetModel(uWeaponId, LoadedWeapon);
    }
    private void LoadedWeapon(Object obj, uint uRes)
    {
        GameObject goCharater = obj as GameObject;
        if (goCharater == null)
        {
            Debug.LogError("加载不了主角：" + uRes);
        }
        goCharater = GameObject.Instantiate(goCharater);

        //将角色放在位子上
        goCharater.transform.SetParent(m_goMountRHandWeapon.transform);
        goCharater.transform.localPosition = new Vector3(0, 0, 0);
        goCharater.transform.localRotation = new Quaternion(0, 0, 0, 1);
        goCharater.transform.localScale = new Vector3(1, 1, 1);

        m_goRHandWeapon = goCharater;
    }

    public void ChangeShield(uint uShield)
    {
        if (m_goLHandShield != null)
        {
            Destroy(m_goLHandShield);
        }

        SingletonManager.Inst.GetManager<CModelManager>().GetModel(uShield, LoadedShield);
    }
    private void LoadedShield(Object obj, uint uRes)
    {
        GameObject goShield = obj as GameObject;
        if (goShield == null)
        {
            Debug.LogError("加载不了主角：" + uRes);
        }
        goShield = GameObject.Instantiate(goShield);

        //将角色放在位子上
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

        SingletonManager.Inst.GetManager<CModelManager>().GetModel(uShoulderArmor, LoadedShoulderArmor);
    }
    private void LoadedShoulderArmor(Object obj, uint uRes)
    {
        GameObject goShoulderArmor = obj as GameObject;
        if (goShoulderArmor == null)
        {
            Debug.LogError("加载不了ShoulderArm：" + uRes);
        }
        goShoulderArmor = GameObject.Instantiate(goShoulderArmor);

        //将角色放在位子上
        goShoulderArmor.transform.SetParent(m_goMountLArmShield.transform);
        goShoulderArmor.transform.localPosition = new Vector3(0, 0, 0);
        goShoulderArmor.transform.localRotation = new Quaternion(0, 0, 0, 1);
        goShoulderArmor.transform.localScale = new Vector3(1, 1, 1);

        m_goLArmShield = goShoulderArmor;
    }
}
