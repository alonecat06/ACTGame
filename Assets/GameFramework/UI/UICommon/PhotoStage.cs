using System.Collections;
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
        SingletonManager.Inst.GetManager<CCharaterManager>().GetCharaterGameObject(uModelId, LoadedCharaterToStage);

        return true;
    }

    public GameObject GetCharaterOnStage()
    {
        return m_goCharaterModel;
    }

    private void LoadedCharaterToStage(Object obj, uint uRes)
    //private void LoadedCharaterToStage(CResource res)
    {
        //Object objCopy = Object.Instantiate(obj);
        //GameObject goCharater = objCopy as GameObject;

        GameObject goCharater = obj as GameObject;
        if (goCharater == null)
        {
            Debug.LogError("加载不了主角：" + uRes);
            //Debug.LogError("加载不了主角：" + res.ResId);
        } 
        goCharater = GameObject.Instantiate(goCharater);

        //将角色放在位子上
        goCharater.transform.SetParent(m_goCharaterPos.transform);
        goCharater.transform.localPosition = new Vector3(0, 0, 0);
        goCharater.transform.localRotation = new Quaternion(0, 0, 0, 1);
        goCharater.transform.localScale = new Vector3(1, 1, 1);

        Destroy(goCharater.GetComponent<CharacterController>());

        m_goCharaterModel = goCharater;
    }
}
