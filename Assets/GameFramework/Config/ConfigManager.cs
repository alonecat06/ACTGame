using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class CConfigManager : MonoBehaviour 
{
    Dictionary<Type, IConfigProvider> m_dictConfigProvider = new Dictionary<Type, IConfigProvider>();

    //void Start()
    //{
    //    Initialize();
    //}

    public bool Initialize()
    {
        m_dictConfigProvider.Add(typeof(ResourceInfoConfigProvider), new ResourceInfoConfigProvider());
        return true;
    }

    public T GetConfigProvider<T>() where T : IConfigProvider
    {
        return m_dictConfigProvider[typeof(T)] as T;
    }

    public void PackConfigFile()
    {
        Dictionary<Type, IConfigProvider>.Enumerator iter = m_dictConfigProvider.GetEnumerator();
        while (iter.MoveNext())
        {
            IConfigProvider cp = iter.Current.Value;
            using (FileStream fs = new FileStream(Application.dataPath + "/GameAssets/Config/" + cp.ConfigProvidePath + cp.ConfigProvideName + ".csv", FileMode.Open))
            {
                ConfigFile cf = new ConfigFile();
                if (cf.ConstructConfigFile(fs, "gb2312"))
                {
                    cp.LoadTextFile(cf);

                    FileStream bs = new FileStream(Application.dataPath + "/ExportedAssets/Config/" + cp.ConfigProvidePath + cp.ConfigProvideName + ".cfg", FileMode.OpenOrCreate);
                    StreamWrapper sw = new StreamWrapper(bs);
                    cp.GenerateBinaryFile(sw);
                    sw.Close();
                    Debug.Log("配置表" + cp.ConfigProvideName + "打包成功");
                }
                else
                {
                    Debug.LogError("配置表" + cp.ConfigProvideName + "打包不成功，请查看配置表格式");
                }

                fs.Close();
            }
        }
    }

    public void LoadAllBinaryFile()
    {

    }
}
