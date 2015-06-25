//**********************************************************************
//                         CInputManager
//负责功能:
//  1. 玩家输入数据获取，包括键盘/鼠标/手柄/触屏，根据对应关系翻译为按键设置
//  2. 玩家输入与按键设置的对应关系
//  3. 按键设置配置管理
//**********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public enum InputFlag
//{
//    Keyboard_Horizontal,
//    Keyboard_Vertical,
//    Keyboard_Fire3,
//}

public class CInputManager : Singletone
{
    //public Dictionary<InputFlag, string> m_dictInputSetting = new Dictionary<InputFlag, string>();
    //public Dictionary<InputFlag, KeyCode> m_dictKeyboardSetting = new Dictionary<InputFlag, KeyCode>();
    public ActionCommandInput m_cmdInput;
    public bool m_bMasterSetup;

    public override bool Initialize()
    {
        m_bMasterSetup = false;
        return true;
    }
    public override bool InitializeData()
    {
        ////初始化玩家输入与按键设置的对应关系
        //m_dictInputSetting.Add(InputFlag.Keyboard_Horizontal, "Horizontal");
        //m_dictInputSetting.Add(InputFlag.Keyboard_Vertical, "Vertical");
        //m_dictInputSetting.Add(InputFlag.Keyboard_Fire3, "Fire3");
        return true;
    }
    public override bool Uninitialize()
    {
        return base.Uninitialize();
    }

    public override bool Update()
    {
        if (m_bMasterSetup)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool bFire3 = Input.GetButton("Fire3");

            m_cmdInput.Move(h, v, bFire3);
        }

        return true;
    }

    public void SetMaster(CCharacter master)
    {
        m_cmdInput = new ActionCommandInput(master);
        m_bMasterSetup = true;
    }

    //public float GetAxis(InputFlag eInput)
    //{
    //    string strName;
    //    float v = 0.0f;
    //    if (m_dictInputSetting.TryGetValue(eInput, out strName))
    //    {
    //        v = Input.GetAxis(strName);
    //    }
    //    return v;
    //}
    //public bool GetButton(InputFlag eInput)
    //{
    //    string strName;
    //    bool v = false;
    //    if (m_dictInputSetting.TryGetValue(eInput, out strName))
    //    {
    //        v = Input.GetButton(strName);
    //    }
    //    return v;
    //}
    //public bool GetKey(InputFlag eInput)
    //{
    //    KeyCode eKeyCode;
    //    bool bPress = false;
    //    if (m_dictKeyboardSetting.TryGetValue(eInput, out eKeyCode))
    //    {
    //        bPress = Input.GetKey(eKeyCode);
    //    }
    //    return bPress;
    //}
}
