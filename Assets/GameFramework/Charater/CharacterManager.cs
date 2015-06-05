using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharaterType
{
    CharaterType_Player,
    CharaterType_Monster,
}

public class CCharacterManager : Singletone//MonoBehaviour 
{

    //private GameObject m_uiRoot;

    //private Dictionary<int, GameObject> m_dictUIPanel = new Dictionary<int, GameObject>();
    //private Dictionary<int, uint> m_dictUIResId = new Dictionary<int, uint>();

    //void Start () 
    //{
	
    //}
	
    //// Update is called once per frame
    //void Update () {
	
    //}

    public void GetCharacterGameObject(uint uModelId, ResourceLoaded delegResLoad, ModelLoaded delegModelLoad)
    {
        SingletonManager.Inst.GetManager<CModelManager>().GetModel(uModelId, delegResLoad, delegModelLoad);
    }
}
