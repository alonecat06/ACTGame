using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;  

public class PackAssetBundle : Editor
{
    ////打包单个  
    //[MenuItem("Custom Editor/Create AssetBunldes Selected")]
    //static void CreateAssetBunldesSelected()
    //{
    //    //获取在Project视图中选择的所有游戏对象  
    //    Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
    //    //遍历所有的游戏对象  
    //    foreach (Object obj in SelectedAsset)
    //    {
    //        //本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径  
    //        //StreamingAssets是只读路径，不能写入  
    //        //服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。  
    //        string targetPath = Application.dataPath + "/ExportedAssets/UI/" + obj.name + ".unity3d";
    //        if (BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.UncompressedAssetBundle))
    //        {
    //            Debug.Log(obj.name + "资源打包成功");
    //        }
    //        else
    //        {
    //            Debug.Log(obj.name + "资源打包失败");
    //        }
    //    }
    //    //刷新编辑器  
    //    AssetDatabase.Refresh();
    //}

    #region 公共函数
    static void DeleteFile(string Path)
    {
        File.Delete(Path);
    }
    static void DeleteDirectory(string strPath, bool bKeepDirectory) 
    {
        //删除这个目录下的所有子目录
        if (Directory.GetDirectories(strPath).Length > 0)
        {
            foreach (string var in Directory.GetDirectories(strPath))
            {
                DeleteDirectory(var, bKeepDirectory);
            }
        }
        //删除这个目录下的所有文件
        if (Directory.GetFiles(strPath).Length > 0)
        {
            foreach (string var in Directory.GetFiles(strPath))
            {
                File.Delete(var);
            }
        }

        if (!bKeepDirectory)
        {
            Directory.Delete(strPath);
        }
    }

    static bool PacketConfigAssetsBundle(string strConfigName, string strConfigPath, IConfigProvider cp, BuildTarget buildTarget, string strExportPath)
    {
        using (FileStream fs = new FileStream(Application.dataPath + "/GameAssets/Config/" + strConfigPath + strConfigName + ".csv", FileMode.Open))
        {
            ConfigFile cf = new ConfigFile();
            if (cf.ConstructConfigFile(fs, "gb2312"))
            {
                //编译为二进制文件
                cp.LoadTextFile(cf);

                FileStream bs = new FileStream(Application.dataPath + "/GameAssets/Config/Binary/" + strConfigName + ".bytes", FileMode.Create);
                StreamWrapper sw = new StreamWrapper(bs);
                cp.GenerateBinaryFile(sw);
                sw.Close();

                //加入到资产数据库
                AssetDatabase.Refresh();
                Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/GameAssets/Config/Binary/" + strConfigName + ".bytes");
                if (obj == null)
                {
                    Debug.LogWarning("Cann't Find File--Assets/GameAssets/Config/Binary/" + strConfigName + ".bytes");
                    return false;
                }
                //进行资产打包
                AssetBundleBuild[] arrABB = new AssetBundleBuild[1];
                AssetBundleBuild abb = new AssetBundleBuild();
                abb.assetBundleName = strConfigName;
                abb.assetNames = new string[] { "Assets/GameAssets/Config/Binary/" + strConfigName + ".bytes" };
                arrABB[0] = abb;
                BuildPipeline.BuildAssetBundles(Application.dataPath + strExportPath + "Config/" + strConfigPath
                                                , arrABB
                                                , BuildAssetBundleOptions.UncompressedAssetBundle
                                                , buildTarget);

                Debug.Log(strConfigName + "打包完成");
                fs.Close();
                return true;
            }
            else
            {
                Debug.LogError(strConfigName + "打包失败，请检查表格");
                fs.Close();
                return false;
            }
        }
    }

    static bool PackResVer()
    {
        IConfigProvider cp = new ResVerConfigProvider();
        using (FileStream fs = new FileStream(Application.dataPath + "/GameAssets/Config/ResVer.csv", FileMode.Open))
        {
            ConfigFile cf = new ConfigFile();
            if (cf.ConstructConfigFile(fs, "gb2312"))
            {
                //编译为二进制文件
                cp.LoadTextFile(cf);

                FileStream bs = new FileStream(Application.dataPath + "/ExportedAssets/ResVer", FileMode.Create);
                StreamWrapper sw = new StreamWrapper(bs);
                cp.GenerateBinaryFile(sw);
                sw.Close();

                Debug.Log("资源总表打包完成");
                fs.Close();
            }
            else
            {
                Debug.LogError("资源总表打包失败，请检查表格");
                fs.Close();
            }
        }

        return true;
    }
    static bool PackConfig(BuildTarget buildTarget, string strExportPaht)
    {
        ResourceInfoConfigProvider cpResInfo = new ResourceInfoConfigProvider();
        bool bResInfoLoad = false;
        using (FileStream fs = new FileStream(Application.dataPath + "/GameAssets/Config/ResourceInfo.csv", FileMode.Open))
        {
            ConfigFile cf = new ConfigFile();
            if (cf.ConstructConfigFile(fs, "gb2312"))
            {
                bResInfoLoad = cpResInfo.LoadTextFile(cf);
            }
            fs.Close();
        }

        if (bResInfoLoad)
        {
            CConfigManager mgrConfig = new CConfigManager();
            mgrConfig.Initialize();
            Dictionary<uint, IConfigProvider>.Enumerator iter = mgrConfig.GetConfigProvidersIter();
            while (iter.MoveNext())
            {
                IConfigProvider cp = iter.Current.Value;
                CResourceInfo resInfo = cpResInfo.GetResourceInfo(cp.ResId);
                if (resInfo == null)
                {
                    Debug.LogError("没有找到配置表的资源信息，ResId" + cp.ResId);
                    continue;
                }

                if (PacketConfigAssetsBundle(resInfo.strResName, cp.ConfigProvidePath, cp, buildTarget, strExportPaht) == false)
                {
                    Debug.LogError(string.Format("打包配置文件错误，{0}，{1}", resInfo.strResName, cp.ConfigProvidePath));
                    return false;
                }
            }
        }

        return true;
    }
    static bool PackResource(BuildTarget buildTarget, string strExportPath)
    {
        //得到资源配置表
        ResourceInfoConfigProvider cpResInfo = new ResourceInfoConfigProvider();
        bool bResInfoLoad = false;
        using (FileStream fs = new FileStream(Application.dataPath + "/GameAssets/Config/ResourceInfo.csv", FileMode.Open))
        {
            ConfigFile cf = new ConfigFile();
            if (cf.ConstructConfigFile(fs, "gb2312"))
            {
                bResInfoLoad = cpResInfo.LoadTextFile(cf);
            }
            fs.Close();
        }

        if (bResInfoLoad)
        {
            //刷新资产数据库
            AssetDatabase.Refresh();

            List<AssetBundleBuild> listUI = new List<AssetBundleBuild>();
            List<AssetBundleBuild> listCharacter = new List<AssetBundleBuild>();
            List<AssetBundleBuild> listItem = new List<AssetBundleBuild>();
            List<AssetBundleBuild> listScene = new List<AssetBundleBuild>();

            //根据资源配置表进行打包
            Dictionary<uint, CResourceInfo>.Enumerator iter = cpResInfo.GetEnumerator();
            while (iter.MoveNext())
            {
                CResourceInfo resInfo = iter.Current.Value;

                AssetBundleBuild abb = new AssetBundleBuild();
                abb.assetBundleName = resInfo.strResName;
                string strSuffix = resInfo.eResourceType == ResourceType.ResourceType_Material ? ".mat" : ".prefab";
                abb.assetNames = new string[] { "Assets/GameAssets/Prefab/" + resInfo.strResName + strSuffix };

                if (resInfo.strResPath.Contains("UI"))
                {
                    listUI.Add(abb);
                }
                else if (resInfo.strResPath.Contains("Character"))
                {
                    listCharacter.Add(abb);
                }
                else if (resInfo.strResPath.Contains("Item"))
                {
                    listItem.Add(abb);
                }
                else if (resInfo.strResPath.Contains("Scene"))
                {
                    listScene.Add(abb);
                }
                else//Config的不处理
                {
                    continue;
                }

                Debug.Log(string.Format("打包资源{0}到路径{1}", resInfo.strResName, strExportPath + resInfo.strResPath));
            }

            BuildPipeline.BuildAssetBundles(Application.dataPath + strExportPath + "UI", listUI.ToArray(), BuildAssetBundleOptions.None, buildTarget);
            BuildPipeline.BuildAssetBundles(Application.dataPath + strExportPath + "Character", listCharacter.ToArray(), BuildAssetBundleOptions.None, buildTarget);
            BuildPipeline.BuildAssetBundles(Application.dataPath + strExportPath + "Item", listItem.ToArray(), BuildAssetBundleOptions.None, buildTarget);
            BuildPipeline.BuildAssetBundles(Application.dataPath + strExportPath + "Scene", listScene.ToArray(), BuildAssetBundleOptions.None, buildTarget);
        }

        return true;
    }
    #endregion

    //打包资源总表与资源信息表
    [MenuItem("Custom Editor/Create Resource Config (pc)")]
    static void CreateResourceConfigAssetBunldes()
    {
        #region 打包资源总表
        DeleteFile(Application.dataPath + "/ExportedAssets/ResVer");
        PackResVer();
        #endregion

        DeleteDirectory(Application.dataPath + "/ExportedAssets/Config/", true);
        PacketConfigAssetsBundle("ResourceInfo", "", new ResourceInfoConfigProvider(), BuildTarget.StandaloneWindows, "/ExportedAssets/");
    }    
    //打包配置表
    [MenuItem("Custom Editor/Create Config AssetBunlde (pc)")]
    static void CreateConfigAssetBunldes()
    {
        if (PackConfig(BuildTarget.StandaloneWindows, "/ExportedAssets/"))
        {
            //清理游戏本地数据
            DeleteDirectory(Application.dataPath + "/StreamingAssets/Config/", true);
            Debug.Log("完成所有配表打包");
        }
    }

    //打包资源
    [MenuItem("Custom Editor/Create Resource AssetBunlde (pc)")]
    static void CreateResourceAssetBunldes()
    {
        DeleteDirectory(Application.dataPath + "/ExportedAssets/Character/", true);
        DeleteDirectory(Application.dataPath + "/ExportedAssets/Item/", true);
        DeleteDirectory(Application.dataPath + "/ExportedAssets/Scene/", true);
        DeleteDirectory(Application.dataPath + "/ExportedAssets/UI/", true);

        if (PackResource(BuildTarget.StandaloneWindows, "/ExportedAssets/"))
        {
            #region 清理游戏本地数据
            DeleteDirectory(Application.dataPath + "/StreamingAssets/Character/", true);
            DeleteDirectory(Application.dataPath + "/StreamingAssets/Item/", true);
            DeleteDirectory(Application.dataPath + "/StreamingAssets/Scene/", true);
            DeleteDirectory(Application.dataPath + "/StreamingAssets/UI/", true);
            #endregion

            Debug.Log("完成打包资源");
        }
    }

    //打包android平台
    [MenuItem("Custom Editor/Pack all Android")]
    static void PackAllAndroid()
    {
        #region 打包资源总表
        DeleteFile(Application.dataPath + "/StreamingAssets/ResVer");
        PackResVer();
        #endregion

        DeleteDirectory(Application.dataPath + "/StreamingAssets/Config/", true);
        PacketConfigAssetsBundle("ResourceInfo", "", new ResourceInfoConfigProvider(), BuildTarget.Android, "/StreamingAssets/");

        #region 打包配置表
        if (PackConfig(BuildTarget.Android, "/StreamingAssets/"))
        {
            Debug.Log("完成所有配表打包");
        }
        #endregion

        #region 打包资源
        DeleteDirectory(Application.dataPath + "/StreamingAssets/Character/", true);
        DeleteDirectory(Application.dataPath + "/StreamingAssets/Item/", true);
        DeleteDirectory(Application.dataPath + "/StreamingAssets/Scene/", true);
        DeleteDirectory(Application.dataPath + "/StreamingAssets/UI/", true);

        if (PackResource(BuildTarget.Android, "/StreamingAssets/"))
        {
            Debug.Log("完成打包资源");
        }
        #endregion
    }


    //// 设置assetbundle的名字(修改meta文件)
    //[MenuItem("Tools/SetAssetBundleName")]
    //static void OnSetAssetBundleName()
    //{

    //    UnityEngine.Object obj = Selection.activeObject;
    //    string path = AssetDatabase.GetAssetPath(Selection.activeObject);

    //    string[] extList = new string[] { ".prefab.meta", ".png.meta", ".jpg.meta", ".tga.meta" };
    //    EditorUtil.Walk(path, extList, DoSetAssetBundleName);

    //    //刷新编辑器
    //    AssetDatabase.Refresh();
    //    Debug.Log("AssetBundleName修改完毕");
    //}
    //static void DoSetAssetBundleName(string path)
    //{
    //    path = path.Replace("\\", "/");
    //    int index = path.IndexOf(EditorConfig.PREFAB_PATH);
    //    string relativePath = path.Substring(path.IndexOf(EditorConfig.PREFAB_PATH) + EditorConfig.PREFAB_PATH.Length);
    //    string prefabName = relativePath.Substring(0, relativePath.IndexOf('.')) + EditorConfig.ASSETBUNDLE;
    //    StreamReader fs = new StreamReader(path);
    //    List<string> ret = new List<string>();
    //    string line;
    //    while ((line = fs.ReadLine()) != null)
    //    {
    //        line = line.Replace("\n", "");
    //        if (line.IndexOf("assetBundleName:") != -1)
    //        {
    //            line = "  assetBundleName: " + prefabName.ToLower();

    //        }
    //        ret.Add(line);
    //    }
    //    fs.Close();

    //    File.Delete(path);

    //    StreamWriter writer = new StreamWriter(path + ".tmp");
    //    foreach (var each in ret)
    //    {
    //        writer.WriteLine(each);
    //    }
    //    writer.Close();

    //    File.Copy(path + ".tmp", path);
    //    File.Delete(path + ".tmp");
    //}
}