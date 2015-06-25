//**********************************************************************
//                         CCharacterManager
//负责功能:
//  1. 场景中的人物管理
//**********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CCharacterManager : Singletone
{
    public List<CharacterSetting> listPlayerSetting = new List<CharacterSetting>();
    public Dictionary<uint, CCharacter> dictCharacterInScene = new Dictionary<uint, CCharacter>();

    private uint m_uHandleCount;
    private uint m_uMasterHandle;

    public uint MasterHandle
    {
        get { return m_uMasterHandle; }
        set { m_uMasterHandle = value; }
    }

    public override bool InitializeData()
    {
        listPlayerSetting.Add(new CharacterSetting());
        m_uHandleCount = 0;
        return true;
    }

    public void GetCharacterGameObject(uint uModelId
                                        , ResourceLoaded delegResLoad
                                        , ModelLoaded delegModelLoad)
    {
        SingletonManager.Inst.GetManager<CModelManager>().GetModel(uModelId, delegResLoad, delegModelLoad);
    }


    public CharacterSetting GetPlayerSetting(int iPlayerIdx)
    {
        if (listPlayerSetting.Count <= iPlayerIdx || iPlayerIdx < 0)
        {
            return null;
        }
        return listPlayerSetting[iPlayerIdx];
    }

    public CCharacter GetCharacterByHandle(uint uCharHandle)
    {
        CCharacter character;
        if (dictCharacterInScene.TryGetValue(uCharHandle, out character))
        {
            return character;
        }
        return null;
    }

    public CCharacter GetMaster()
    {
        return GetCharacterByHandle(MasterHandle);
    }

    public uint CreateCharaterToScene(CharacterSetting settingChar
                                        , EntityLoaded entityLoaded
                                        , out IEnumerator corotineLoading)
    {        
        CCharacter character = new CCharacter(settingChar);
        character.m_Handle = m_uHandleCount++;
        dictCharacterInScene.Add(character.m_Handle, character);
        character.OnEntityLoaded += entityLoaded;

        //加载人物
        uint iModelId = SingletonManager.Inst.GetManager<CConfigManager>().GetConfigProvider<CharacterCfgConfigProvider>().GetEquipCfg(settingChar.m_uCharaterId).uResId;
        SingletonManager.Inst.GetManager<CModelManager>().GetModel(iModelId
            , character.FinishLoadingPlayer
            , character.SetPlayerToScene
            , out corotineLoading);

        return character.m_Handle;
    }
}
