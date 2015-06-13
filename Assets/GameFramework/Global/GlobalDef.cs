using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GlobalDef
{
//    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。  
//    public static readonly string PathURL =
//#if UNITY_ANDROID   //安卓  
//        "jar:file://" + Application.dataPath + "!/assets/";  
//#elif UNITY_IPHONE  //iPhone  
//        Application.dataPath + "/Raw/";  
//#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
//        "file://" + Application.dataPath + "/StreamingAssets/";
//#else  
//        string.Empty;  
//#endif

    public static string s_FileServerIP = "127.0.0.1";
    public static string s_FileServerPort = "1234";
    public static string s_FileServerUrl = "http://" + GlobalDef.s_FileServerIP + ":" + GlobalDef.s_FileServerPort + "/";
    public static string s_ResVerName = "ResVer";
    public static string s_ResourceInfoName = "ResourceInfo";
    public static uint s_ResourceInfoResId = 0;

    public static string s_LocalFileRootPath = 
#if UNITY_ANDROID   //安卓  
        "jar:file://" + Application.streamingAssetsPath + "/";  
#elif UNITY_IPHONE  //iPhone  
        Application.streamingAssetsPath + "/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
        "file:///" + Application.streamingAssetsPath + "/";
        //Application.persistentDataPath + "/";
#else  
        string.Empty;  
#endif
}
