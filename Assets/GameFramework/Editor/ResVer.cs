using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Runtime.Serialization;

public class ResVerConfigProvider : IConfigProvider
{
    private Dictionary<uint, uint> m_dictData = new Dictionary<uint, uint>();

    public override string ConfigProvidePath
    {
        get { return ""; }
    }
    //public override string ConfigProvideName
    //{
    //    get { return "ResVer"; }
    //}
    public override uint ResId
    {
        get { return 5000; }
    }

    public override bool LoadTextFile(ConfigFile file)
    {
        int iRowCount = file.GetRow();
        for (int iRow = 0; iRow < iRowCount; ++iRow )
        {
            uint uResId = file.GetUIntData(iRow, file.GetColumnIdxByName("ResId"));
            uint uVerId = file.GetUIntData(iRow, file.GetColumnIdxByName("VerId"));

            m_dictData.Add(uResId, uVerId);
        }

        return true;
    }

    public override bool GenerateBinaryFile(StreamWrapper stream)
    {
        BinaryFormatter binFormat = new BinaryFormatter();
        binFormat.Serialize(stream.GetStream(), m_dictData);

        return true;
    }
}