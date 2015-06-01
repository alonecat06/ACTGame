using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharaterType
{
    CharaterType_Player,
    CharaterType_Monster,
}

public class CCharaterManager : MonoBehaviour {

    //private GameObject m_uiRoot;

    //private Dictionary<int, GameObject> m_dictUIPanel = new Dictionary<int, GameObject>();
    //private Dictionary<int, uint> m_dictUIResId = new Dictionary<int, uint>();

	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GetCharaterGameObject(uint uModelId, ResourceLoaded deleg)
    {
        SingletonManager.Inst.GetManager<CModelManager>().GetModel(uModelId, deleg);
    }
}
