using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharaterType
{
    CharaterType_Player,
    CharaterType_Monster,
}

public class CharacterSetting
{
    public uint m_uCharaterId;
    public CharaterType m_eCharType;
    public Dictionary<EquipType, uint> m_dictMountPos = new Dictionary<EquipType, uint>();

    public CharacterSetting()
    {
        m_uCharaterId = 0;
        m_eCharType = CharaterType.CharaterType_Player;
    }
    public CharacterSetting(uint uCharaterId, CharaterType eCharType)
    {
        m_uCharaterId = uCharaterId;
        m_eCharType = eCharType;
    }
    public CharacterSetting(uint uCharaterId, CharaterType eCharType, Dictionary<EquipType, uint> dictMountPos)
    {
        m_uCharaterId = uCharaterId;
        m_eCharType = eCharType;
        m_dictMountPos = dictMountPos;
    }
}

public delegate void EntityLoaded(CCharacter entity);

public class CCharacter
{
    public event EntityLoaded OnEntityLoaded;

    public uint m_Handle;

    public CharacterSetting m_Setting;

    public CharacterAppearance m_Appearance;
    public GameObject m_goCharacter;
    public Transform m_trans;

    public CCharacter(CharacterSetting setting)
    {
        m_Setting = setting;
    }

    public void Initialize(GameObject goEntity)
    {
        m_goCharacter = goEntity;
        m_Appearance = goEntity.GetComponent<CharacterAppearance>();
        m_Appearance.SetupCharacter(m_Setting);
        m_trans = goEntity.transform;
    }

    public void FinishLoadingPlayer(CResource res)
    {
        GameObject goCharacter = (GameObject)res.AssetBundle.LoadAsset(res.ResourceName, typeof(GameObject));// res.MainAsset as GameObject;   

        if (goCharacter == null)
        {
            Debug.LogError("加载不了主角：" + res.ResId);
        }
        goCharacter = GameObject.Instantiate(goCharacter);

        SetPlayerToScene(goCharacter);
    }
    public void SetPlayerToScene(GameObject goCharacter)
    {
        Initialize(goCharacter);
        OnEntityLoaded(this);
    }
}
