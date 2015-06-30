using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CLogManager : Singletone
{
    private GameObject m_goLogPanel;
    private InputField m_infLog;

    public bool EnableLog
    {
        get;
        set;
    }
    public bool ShowLog
    {
        get;
        set;
    }

    public override bool Initialize()
    {
        EnableLog = true;
        ShowLog = true;

        //m_goLogPanel = GameObject.Find("goLogUI");
        //if (m_goLogPanel == null)
        //{
        //    Debug.LogError("goLogUI节点为空");
        //    return false;
        //}
        m_goLogPanel = GameObject.Find("infLog");
        if (m_goLogPanel == null)
        {
            Debug.LogError("infLog节点为空");
            return false;
        }
        m_infLog = m_goLogPanel.GetComponent<InputField>();
        if (m_infLog == null)
        {
            Debug.LogError("infLog节点没有挂InputField");
            return false;
        }

        if (ShowLog == false)
        {
            m_goLogPanel.SetActive(false);
        }

        return true;
    }

    public void Log(object message)
    {
        Log(message, null);
    }
    public void Log(object message, Object context)
    {
        if (EnableLog)
        {
            Debug.Log(message, context);
        }

        if (ShowLog)
        {
            m_infLog.text += message.ToString() + "\n";
        }
    }

    public void LogError(object message)
    {
        LogError(message, null);
    }
    public void LogError(object message, Object context)
    {
        if (EnableLog)
        {
            Debug.LogError(message, context);
        }

        if (ShowLog)
        {
            m_infLog.text += message.ToString() + "\n";
        }
    }

    public void LogWarning(object message)
    {
        LogWarning(message, null);
    }
    public void LogWarning(object message, Object context)
    {
        if (EnableLog)
        {
            Debug.LogWarning(message, context);
        }

        if (ShowLog)
        {
            m_infLog.text += message.ToString() + "\n";
        }
    }
}