using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class ConfigFile
{
    private List<List<string>> m_strFileContent = new List<List<string>>();
    private int m_iColumn;
    private int m_iRow;

    public bool ConstructConfigFile(Stream steam, string strEncode)
    {
        string s = string.Empty;
        string src = string.Empty;
        m_iRow = 0;
        SortedList sl = new SortedList();
        StreamReader fs = new StreamReader(steam, Encoding.GetEncoding(strEncode));

        while (!string.IsNullOrEmpty(s = fs.ReadLine()))
        {
            if (s[0] == '/' && s[1] == '/')
            {
                continue;
            }

            List<string> listRow = new List<string>();
            if (!string.IsNullOrEmpty(s))
            {
                src = s.Replace("\"\"", "'");
                MatchCollection col = Regex.Matches(src, ",\"([^\"]+)\",", RegexOptions.ExplicitCapture);
                IEnumerator ie = col.GetEnumerator();
                while (ie.MoveNext())
                {
                    string patn = ie.Current.ToString();
                    int key = src.Substring(0, src.IndexOf(patn)).Split(',').Length;
                    if (!sl.ContainsKey(key))
                    {
                        sl.Add(key, patn.Trim(new char[] { ',', '"' }).Replace("'", "\""));
                        src = src.Replace(patn, ",,");
                    }

                    col = Regex.Matches(src, ",\"([^\"]+)\",", RegexOptions.ExplicitCapture);
                    ie = col.GetEnumerator();
                }

                string[] arr = src.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!sl.ContainsKey(i))
                        sl.Add(i, arr[i]);
                }

                IDictionaryEnumerator ienum = sl.GetEnumerator();
                while (ienum.MoveNext())
                {
                    listRow.Add(ienum.Value.ToString().Replace("'", "\""));
                }

                if (m_iRow == 0)
                {
                    m_iColumn = sl.Count;
                }
                else
                {
                    if (sl.Count != m_iColumn)
                    {
                        fs.Close();
                        return false;
                    }
                }

                sl.Clear();
                src = string.Empty;
            }

            m_strFileContent.Add(listRow);

            m_iRow++;
        }

        fs.Close();
        return true;
    }

    public int GetRow()
    {
        return m_iRow;
    }

    public string GetContent(int iRow, int iColoumn)
    {
        if (m_iRow > iRow)
        {
            if (m_iColumn > iColoumn)
            {
                return m_strFileContent[iRow][iColoumn];
            }
        }

        return null;
    }
    public float GetFloatData(int iRow, int iColoumn)
    {
        float fOut;
        if (m_iRow > iRow)
        {
            if (m_iColumn > iColoumn)
            {
                if (float.TryParse(m_strFileContent[iRow][iColoumn], out fOut))
                {
                    return fOut;
                }
            }
        }
        return 0.0f;
    }
    public int GetIntData(int iRow, int iColoumn)
    {
        int iOut;
        if (m_iRow > iRow)
        {
            if (m_iColumn > iColoumn)
            {
                if (int.TryParse(m_strFileContent[iRow][iColoumn], out iOut))
                {
                    return iOut;
                }
            }
        }
        return 0;
    }
    public uint GetUIntData(int iRow, int iColoumn)
    {
        uint iOut;
        if (m_iRow > iRow)
        {
            if (m_iColumn > iColoumn)
            {
                if (uint.TryParse(m_strFileContent[iRow][iColoumn], out iOut))
                {
                    return iOut;
                }
            }
        }
        return 0;
    }
    public bool GetBoolData(int iRow, int iColoumn)
    {
        bool bOut;
        if (m_iRow > iRow)
        {
            if (m_iColumn > iColoumn)
            {
                if (bool.TryParse(m_strFileContent[iRow][iColoumn], out bOut))
                {
                    return bOut;
                }
            }
        }
        return false;
    }
}
