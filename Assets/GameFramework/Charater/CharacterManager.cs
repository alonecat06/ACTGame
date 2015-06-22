using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CCharacterManager : Singletone
{
    public List<CharacterSetting> listPlayerSetting = new List<CharacterSetting>();

    public Dictionary<uint, CCharacter> dictCharacterInScene = new Dictionary<uint, CCharacter>();
    //public uint m_uMasterHandle;

    private uint m_uHandleCount;

    public override bool InitializeData()
    {
        listPlayerSetting.Add(new CharacterSetting());
        m_uHandleCount = 0;
        return true;
    }

    public void GetCharacterGameObject(uint uModelId, ResourceLoaded delegResLoad, ModelLoaded delegModelLoad)
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

    //public CCharacter CreateCharacter()
    //{
    //    dictCharacterInScene.Add
    //}
    //public CCharacter CreatePlayer()
    //{
    //    return dictCharacterInScene[0];
    //}

    public uint CreateOrGetCharaterToScene(CharacterSetting settingChar, EntityLoaded entityLoaded, out IEnumerator corotineLoading)
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

    //private void FinishLoadingPlayer(CResource res)
    //{
    //    GameObject goCharacter = (GameObject)res.AssetBundle.LoadAsset(res.ResourceName, typeof(GameObject));// res.MainAsset as GameObject;   

    //    if (goCharacter == null)
    //    {
    //        Debug.LogError("加载不了主角：" + res.ResId);
    //    }
    //    goCharacter = GameObject.Instantiate(goCharacter);

    //    CCharacter character;
    //    if (dictCharacterInScene.TryGetValue(0, out character))
    //    {
    //        character.m_goCharacter = goCharacter;
    //        character.m_Appearance = goCharacter.GetComponent<CharacterAppearance>();
    //        character.m_Appearance.SetupCharacter(character.m_Setting);
    //        character.m_trans = goCharacter.transform;
    //    }

    //    SetPlayerToScene(goCharacter);
    //}
    //private void SetPlayerToScene(GameObject goCharacter)
    //{
    //    GameObject goSceneRoot = SingletonManager.Inst.GetManager<CSceneManager>().SceneRoot;
    //    SceneCfg cfgScene = SingletonManager.Inst.GetManager<CSceneManager>().SceneConfig;

    //    goCharacter.transform.SetParent(goSceneRoot.transform);
    //    goCharacter.transform.localPosition = new Vector3(cfgScene.fEnterX, cfgScene.fEnterY, cfgScene.fEnterZ);
    //    goCharacter.transform.localRotation = new Quaternion(0, 0, 0, 1);

    //    SingletonManager.Inst.GetManager<CCharacterManager>().AddCharacterToScene(goCharacter);
    //}
}
