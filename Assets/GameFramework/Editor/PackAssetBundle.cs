using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;  

public class PackAssetBundle : Editor
{
    //打包单个  
    [MenuItem("Custom Editor/Create AssetBunldes Selected")]
    static void CreateAssetBunldesSelected()
    {
        //获取在Project视图中选择的所有游戏对象  
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //遍历所有的游戏对象  
        foreach (Object obj in SelectedAsset)
        {
            //本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径  
            //StreamingAssets是只读路径，不能写入  
            //服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。  
            string targetPath = Application.dataPath + "/Asset/" + obj.name + ".unity3d";
            if (BuildPipeline.BuildAssetBundle(obj, null, targetPath))
            {
                Debug.Log(obj.name + "资源打包成功");
            }
            else
            {
                Debug.Log(obj.name + "资源打包失败");
            }
        }
        //刷新编辑器  
        AssetDatabase.Refresh();
    }

    //打包资源总表
    [MenuItem("Custom Editor/Create ResVer AssetBunlde")]
    static void CreateResVerAssetBunldes()
    {
        Debug.Log("打包资源总表");
    }

    //打包配置表
    [MenuItem("Custom Editor/Create Config AssetBunlde")]
    static void CreateConfigAssetBunldes()
    {
        Debug.Log("打包配置表");
    }

    //打包资源
    [MenuItem("Custom Editor/Create Resource AssetBunlde")]
    static void CreateResourceAssetBunldes()
    {
        Debug.Log("打包资源");
    }

//    [MenuItem("Custom Editor/Create AssetBunldes ALL")]
//    static void CreateAssetBunldesALL()
//    {
//        Caching.CleanCache();
//        string Path = Application.dataPath + "/StreamingAssets/ALL.assetbundle";
//        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

//        foreach (Object obj in SelectedAsset)
//        {
//            Debug.Log("Create AssetBunldes name :" + obj);
//        }

//        //这里注意第二个参数就行  
//        if (BuildPipeline.BuildAssetBundle(null, SelectedAsset, Path))
//        {
//            AssetDatabase.Refresh();
//        }
//    }

//    [MenuItem("Custom Editor/Create Scene")]
//    static void CreateSceneALL()
//    {
//        //清空一下缓存  
//        Caching.CleanCache();
//        string Path = Application.dataPath + "/MyScene.unity3d";
//        string[] levels = { "Assets/Level.unity" };
//        //打包场景  
//        BuildPipeline.BuildPlayer(levels, Path, BuildTarget.WebPlayer, BuildOptions.BuildAdditionalStreamedScenes);
//        AssetDatabase.Refresh();
//    }

//    [MenuItem("Custom Editor/Build Assetbundle")]
//    static private void BuildAssetBundle()
//    {
//        string dir = Application.dataPath + "/StreamingAssets";

//        if (!Directory.Exists(dir))
//        {
//            Directory.CreateDirectory(dir);
//        }
//        DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas");
//        foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
//        {
//            List<Sprite> assets = new List<Sprite>();
//            string path = dir + "/" + dirInfo.Name + ".assetbundle";
//            foreach (FileInfo pngFile in dirInfo.GetFiles("*.png", SearchOption.AllDirectories))
//            {
//                string allPath = pngFile.FullName;
//                string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
//                assets.Add(Resources.LoadAssetAtPath<Sprite>(assetPath));
//            }
//            if (BuildPipeline.BuildAssetBundle(null
//                                                , assets.ToArray()
//                                                , path
//                                                , BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.CollectDependencies
//                                                , GetBuildTarget()))
//            {
//            }
//        }
//    }

//    static private BuildTarget GetBuildTarget()
//    {
//        BuildTarget target = BuildTarget.WebPlayer;
//#if UNITY_STANDALONE
//            target = BuildTarget.StandaloneWindows;
//#elif UNITY_IPHONE
//            target = BuildTarget.iPhone;
//#elif UNITY_ANDROID
//            target = BuildTarget.Android;
//#endif
//        return target;
//    }

//    [MenuItem("Custom Editor/Build AssetBundle From Selection - Track dependencies")]
//    static void ExportResource()
//    {
//        // Bring up save panel
//        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
//        if (path.Length != 0)
//        {
//            // Build the resource file from the active selection.
//            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
//            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, 0);
//            Selection.objects = selection;
//        }
//    }
//    [MenuItem("Custom Editor/Build AssetBundle From Selection - No dependency tracking")]
//    static void ExportResourceNoTrack()
//    {
//        // Bring up save panel
//        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
//        if (path.Length != 0)
//        {
//            // Build the resource file from the active selection.
//            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
//        }
//    }
}