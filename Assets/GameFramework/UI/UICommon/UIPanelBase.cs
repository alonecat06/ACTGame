using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase: MonoBehaviour
{
    public virtual void Start()
    {
        if (!InitializeUI())
        {
            Debug.LogError("界面初始化失败");
        }
    }

    public virtual bool InitializeUI()
    {
        return true;
    }
}
