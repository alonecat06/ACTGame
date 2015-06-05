using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class IConfigProvider
{
    public abstract string ConfigProvidePath
    {
        get;
    }
    public abstract string ConfigProvideName
    {
        get;
    }
    public abstract uint ResId
    {
        get;
    }

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
