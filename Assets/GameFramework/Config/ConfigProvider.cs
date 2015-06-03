using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IConfigProvider 
{
    public virtual bool LoadBinaryFile(StreamWrapper stream)
    {
        return false;
    }

    public virtual bool LoadTextFile(ConfigFile file)
    {
        return false;
    }

    public virtual bool GenerateBinaryFile(StreamWrapper stream)
    {
        return false;
    }
}
