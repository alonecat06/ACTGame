using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;

public class CConfigManager : Singletone//MonoBehaviour 
{
    Dictionary<Type, uint> m_dictCPType = new Dictionary<Type, uint>();
    Dictionary<uint, IConfigProvider> m_dictConfigProvider = new Dictionary<uint, IConfigProvider>();

    public override bool Initialize()
    {
        IConfigProvider cp = new SceneCfgConfigProvider();
        m_dictCPType.Add(typeof(SceneCfgConfigProvider), cp.ResId);
        m_dictConfigProvider.Add(cp.ResId, cp);

        //LoadAllBinaryFile();

        return true;
    }

    public T GetConfigProvider<T>() where T : IConfigProvider
    {
        return m_dictConfigProvider[m_dictCPType[typeof(T)]] as T;
    }

    public Dictionary<uint, IConfigProvider>.Enumerator GetConfigProvidersIter()
    {
        return m_dictConfigProvider.GetEnumerator();
    }

    //public void PackConfigFile()
    //{
    //    Dictionary<uint, IConfigProvider>.Enumerator iter = m_dictConfigProvider.GetEnumerator();
    //    while (iter.MoveNext())
    //    {
    //        IConfigProvider cp = iter.Current.Value;
    //        using (FileStream fs = new FileStream(Application.dataPath + "/GameAssets/Config/" + cp.ConfigProvidePath + cp.ConfigProvideName + ".csv", FileMode.Open))
    //        {
    //            ConfigFile cf = new ConfigFile();
    //            if (cf.ConstructConfigFile(fs, "gb2312"))
    //            {
    //                cp.LoadTextFile(cf);

    //                FileStream bs = new FileStream(Application.dataPath + "/GameAssets/Config/Binary/" + cp.ConfigProvideName + ".bytes", FileMode.Create);
    //                StreamWrapper sw = new StreamWrapper(bs);
    //                cp.GenerateBinaryFile(sw);
    //                sw.Close();
    //                Debug.Log("配置表" + cp.ConfigProvideName + "打包成功");
    //            }
    //            else
    //            {
    //                Debug.LogError("配置表" + cp.ConfigProvideName + "打包不成功，请查看配置表格式");
    //            }

    //            fs.Close();

    //            //加入到资产数据库
    //            UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/GameAssets/Config/Binary/" + cp.ConfigProvideName + ".bytes");
    //            if (obj == null)
    //            {
    //                Debug.LogWarning("Cann't Find File--Assets/GameAssets/Config/Binary/" + cp.ConfigProvideName + ".bytes");
    //                return;
    //            }
    //            //进行资产打包
    //            AssetBundleBuild[] arrABB = new AssetBundleBuild[1];
    //            AssetBundleBuild abb = new AssetBundleBuild();
    //            abb.assetBundleName = cp.ConfigProvideName;
    //            abb.assetNames = new string[] { "Assets/GameAssets/Config/Binary/" + cp.ConfigProvideName + ".bytes" };
    //            arrABB[0] = abb;
    //            BuildPipeline.BuildAssetBundles(Application.dataPath + "/ExportedAssets/Config/" + cp.ConfigProvidePath, arrABB);
    //        }
    //    }
    //}

    public void LoadAllBinaryFile()
    {
        Dictionary<uint, IConfigProvider>.Enumerator iter = m_dictConfigProvider.GetEnumerator();
        while (iter.MoveNext())
        {
            IConfigProvider cp = iter.Current.Value;

            SingletonManager.Inst.GetManager<CResourceManager>().LoadResource(cp.ResId
                    , FinishLoadingConfig);
        }
    }

    public void FinishLoadingConfig(CResource res)
    {
        object[] txt = res.AssetBundle.LoadAllAssets(typeof(TextAsset));
        if (null != txt)
        {
            if (txt.Length == 1)
            {
                foreach (object obj in txt)
                {
                    if (obj.GetType() == typeof(TextAsset))
                    {
                        TextAsset tt = obj as TextAsset;
                        StreamWrapper sw = new StreamWrapper(tt.bytes);
                        
                        IConfigProvider cp;
                        if (m_dictConfigProvider.TryGetValue(res.ResId, out cp))
                        {
                            cp.LoadBinaryFile(sw);
                        }
                    }
                }
            }
        }
    }
}
