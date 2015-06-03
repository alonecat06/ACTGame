using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMain : MonoBehaviour {

    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。  
    public static readonly string PathURL =
#if UNITY_ANDROID   //安卓  
        "jar:file://" + Application.dataPath + "!/assets/";  
#elif UNITY_IPHONE  //iPhone  
        Application.dataPath + "/Raw/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
        "file://" + Application.dataPath + "/StreamingAssets/";
#else  
        string.Empty;  
#endif 

    public GameObject m_uiRoot;

    // Use this for initialization
    void Start()
    {
        if (m_uiRoot == null)
        {
            Debug.LogError("UI摄像机为空，严重错误");
        }

        //添加各种管理类
        gameObject.AddComponent<CResourceManager>();
        gameObject.AddComponent<CConfigManager>();
        gameObject.AddComponent<CUIManager>().SetUIRoot(m_uiRoot);
        gameObject.AddComponent<CModelManager>();
        gameObject.AddComponent<CCharacterManager>();
        gameObject.AddComponent<CSceneManager>();
        gameObject.AddComponent<CInputManager>();

        //加载第一个界面
        SingletonManager.Inst.GetManager<CUIManager>().LoadUI(0);
    }
}
