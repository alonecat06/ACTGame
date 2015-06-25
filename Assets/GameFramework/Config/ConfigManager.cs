//**********************************************************************
//                         CConfigManager
//负责功能:
//  1. 二进制配置文件加载
//  2. 配置数据管理
//**********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class CConfigManager : Singletone
{
    Dictionary<Type, uint> m_dictCPType = new Dictionary<Type, uint>();
    Dictionary<uint, IConfigProvider> m_dictConfigProvider = new Dictionary<uint, IConfigProvider>();

    public override bool Initialize()
    {
        IConfigProvider cp = new SceneCfgConfigProvider();
        m_dictCPType.Add(typeof(SceneCfgConfigProvider), cp.ResId);
        m_dictConfigProvider.Add(cp.ResId, cp);

        cp = new EquipCfgConfigProvider();
        m_dictCPType.Add(typeof(EquipCfgConfigProvider), cp.ResId);
        m_dictConfigProvider.Add(cp.ResId, cp);

        cp = new CharacterCfgConfigProvider();
        m_dictCPType.Add(typeof(CharacterCfgConfigProvider), cp.ResId);
        m_dictConfigProvider.Add(cp.ResId, cp);

        return true;
    }
    public override bool InitializeData()
    {
        LoadAllBinaryFile();
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
