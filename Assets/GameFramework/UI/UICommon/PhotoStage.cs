﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PhotoStage : MonoBehaviour
{
    private GameObject m_goCharaterPos;
    private GameObject m_goCharaterModel;

    void Start()
    {
        m_goCharaterPos = GameObject.Find("goPlayerStage");
        if (m_goCharaterPos == null)
        {
            Debug.LogError("没有找到拍照点站位");
        }
    }

    public bool LoadCharaterToStage(CharaterType eCharType, uint uModelId)
    {
        //卸载现有
        if (m_goCharaterModel != null)
            Destroy(m_goCharaterModel);

        //从角色管理器异步获得引用
        SingletonManager.Inst.GetManager<CCharacterManager>().GetCharacterGameObject(uModelId, LoadedCharaterRes, SetCharacterToStage);

        return true;
    }

    public GameObject GetCharaterOnStage()
    {
        return m_goCharaterModel;
    }

    private void LoadedCharaterRes(CResource res)
    {
        //Object objCopy = Object.Instantiate(res.MainAsset);
        //GameObject goCharacter = objCopy as GameObject;
        GameObject goCharacter = (GameObject)res.AssetBundle.LoadAsset(res.ResourceName, typeof(GameObject));// res.MainAsset as GameObject;   

        if (goCharacter == null)
        {
            Debug.LogError("加载不了主角：" + res.ResId);
        } 
        goCharacter = GameObject.Instantiate(goCharacter);

        SetCharacterToStage(goCharacter);
    }

    private void SetCharacterToStage(GameObject goCharacter)
    {
        goCharacter.transform.SetParent(m_goCharaterPos.transform);
        goCharacter.transform.localPosition = new Vector3(0, 0, 0);
        goCharacter.transform.localRotation = new Quaternion(0, 0, 0, 1);
        goCharacter.transform.localScale = new Vector3(1, 1, 1);

        Destroy(goCharacter.GetComponent<CharacterController>());

        m_goCharaterModel = goCharacter;
    }
}
