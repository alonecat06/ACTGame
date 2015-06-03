using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public class StreamWrapper
{
    public static int s_IntSize = 4;
    public static int s_UIntSize = 4;
    public static int s_LongSize = 8;
    public static int s_FloatSize = 4;
    public static int s_Float2Size = 8;
    public static int s_Float3Size = 12;
    public static int s_Float4Size = 16;
    public static int s_DoubleSize = 8;
    public static int s_CharSize = 1;
    public static int s_ByteSize = 1;
    public static int s_BoolSize = 1;
    public static int s_ShortSize = 2;

    private Stream m_stream = null;
    private byte[] m_byte = null;

    public StreamWrapper(Stream stream)
    {
        m_stream = stream;
    }
    public StreamWrapper(byte[] udata)
    {
        m_byte = udata;
        m_stream = new MemoryStream(udata);
    }

    public Stream GetStream()
    {
        return m_stream;
    }
    public void Close()
    {
        if (null != m_stream)
        {
            m_stream.Close();
        }
    }
    public byte[] GetBuffer()
    {
        return m_byte;
    }
    public void Seek(int iOff)
    {
        if (iOff < m_stream.Length)
        {
            m_stream.Position = iOff;
        }
    }
    public long Lenght()
    {
        return null != m_stream ? m_stream.Length : null != m_byte ? m_byte.Length : 0;
    }
    public int GetOffset()
    {
        if (m_stream != null)
            return (int)m_stream.Position;
        else
            return 0;

    }
    public void Reset()
    {
        m_stream.Position = 0;
        if (null != m_byte)
        {
            //Array.Clear(m_byte, 0, m_byte.Length);
        }
    }
    public int GetRest()
    {
        if (null == m_stream)
        {
            return -1;
        }
        return (int)m_stream.Length - (int)m_stream.Position;
    }

    public int Read(ref byte[] uDate)
    {
        if (m_stream == null || null == uDate)
        {
            return -1;
        }
        int iRef = m_stream.Read(uDate, 0, uDate.Length);
        return iRef;
    }
    public void Write(ref byte[] uDate)
    {
        if (m_stream == null || null == uDate)
        {
            return;
        }

        m_stream.Write(uDate, 0, uDate.Length);
    }

    public int ReadInt(ref int uDate)
    {
        if (m_stream == null)
        {
            return -1;
        }
        byte[] bit = new byte[s_IntSize];
        int iRef = m_stream.Read(bit, 0, s_IntSize);
        uDate = BitConverter.ToInt32(bit, 0);
        return iRef;
    }
    public int ReadInt()
    {
        byte[] bit = new byte[s_IntSize];
        m_stream.Read(bit, 0, s_IntSize);
        return BitConverter.ToInt32(bit, 0);
    }
    public void WriteInt(int uDate)
    {
        if (m_stream == null)
        {
            return;
        }
        byte[] bit = BitConverter.GetBytes(uDate);
        m_stream.Write(bit, 0, s_IntSize);
    }

    public int ReadUInt(ref uint uDate)
    {
        if (m_stream == null)
        {
            return -1;
        }
        byte[] bit = new byte[s_IntSize];
        int iRef = m_stream.Read(bit, 0, s_IntSize);
        uDate = BitConverter.ToUInt32(bit, 0);
        return iRef;
    }
    public uint ReadUInt()
    {
        if (m_stream == null)
        {
            return 0;
        }
        byte[] bit = new byte[s_IntSize];
        m_stream.Read(bit, 0, s_IntSize);
        return BitConverter.ToUInt32(bit, 0);
    }
    public void WriteUInt(uint uDate)
    {
        if (m_stream == null)
        {
            return;
        }
        byte[] bit = BitConverter.GetBytes(uDate);
        m_stream.Write(bit, 0, s_IntSize);
    }

    public int ReadUShort(ref ushort uDate)
    {
        if (m_stream == null)
        {
            return -1;
        }
        byte[] bit = new byte[s_ShortSize];
        int iRef = m_stream.Read(bit, 0, s_ShortSize);
        uDate = BitConverter.ToUInt16(bit, 0);
        return iRef;
    }
    public ushort ReadUShort()
    {
        if (m_stream == null)
        {
            return 0;
        }
        byte[] bit = new byte[s_ShortSize];
        m_stream.Read(bit, 0, s_ShortSize);
        return BitConverter.ToUInt16(bit, 0);
    }
    public void WriteUShort(ushort uDate)
    {
        if (m_stream == null)
        {
            return;
        }

        m_stream.Write(BitConverter.GetBytes(uDate), 0, s_ShortSize);
    }

    public int ReadLong(ref long uDate)
    {
        if (m_stream == null)
        {
            return -1;
        }
        byte[] bit = new byte[s_LongSize];
        int iRef = m_stream.Read(bit, 0, s_LongSize);
        uDate = BitConverter.ToInt64(bit, 0);
        return iRef;
    }
    public long ReadLong()
    {
        if (m_stream == null)
        {
            return -1;
        }
        byte[] bit = new byte[s_LongSize];
        m_stream.Read(bit, 0, s_LongSize);
        return BitConverter.ToInt64(bit, 0);
    }
    public void WriteLong(long uDate)
    {
        if (m_stream == null)
        {
            return;
        }
        byte[] bit = BitConverter.GetBytes(uDate);
        m_stream.Write(bit, 0, s_LongSize);
    }

    public int ReadBool(ref bool uDate)
    {
        if (m_stream == null)
        {
            return -1;
        }
        byte[] bit = new byte[s_BoolSize];
        int iRef = m_stream.Read(bit, 0, s_BoolSize);
        uDate = BitConverter.ToBoolean(bit, 0);
        return iRef;
    }
    public bool ReadBool()
    {
        if (m_stream == null)
        {
            return false;
        }
        byte[] bit = new byte[s_BoolSize];
        m_stream.Read(bit, 0, s_BoolSize);
        return BitConverter.ToBoolean(bit, 0);
    }
    public void WriteBool(bool uDate)
    {
        if (m_stream == null)
        {
            return;
        }
        byte[] bit = BitConverter.GetBytes(uDate);
        m_stream.Write(bit, 0, s_BoolSize);
    }

    public int ReadFloat(ref float uDate)
    {
        if (m_stream == null)
        {
            return -1;
        }
        byte[] bit = new byte[s_FloatSize];
        int iRef = m_stream.Read(bit, 0, s_FloatSize);
        uDate = BitConverter.ToSingle(bit, 0);
        return iRef;
    }
    public void WriteFloat(float uDate)
    {
        if (m_stream == null)
        {
            return;
        }
        byte[] bit = BitConverter.GetBytes(uDate);
        m_stream.Write(bit, 0, s_FloatSize);
    }

    public int ReadDouble(ref double uDate)
    {
        if (m_stream == null)
        {
            return -1;
        }
        byte[] bit = new byte[s_DoubleSize];
        int iRef = m_stream.Read(bit, 0, s_DoubleSize);
        uDate = BitConverter.ToDouble(bit, 0);
        return iRef;
    }
    public void WriteDouble(double uDate)
    {
        if (m_stream == null)
        {
            return;
        }
        byte[] bit = BitConverter.GetBytes(uDate);
        m_stream.Write(bit, 0, s_DoubleSize);
    }

    public bool ReadString(out string strOut)
    {
        strOut = null;
        if (null == m_stream)
        {
            return false;
        }
        int iLenght = 0;
        ReadInt(ref iLenght);
        if (iLenght <= 0)
        {
            strOut = null;
            return true;
        }
        byte[] uData = new byte[iLenght];
        Read(ref uData);
        //读出来的都是unicode
        strOut = Encoding.UTF8.GetString(uData);
        return true;
    }
    public bool ReadString(out string strOut, int size)
    {
        strOut = null;
        if (null == m_stream)
        {
            return false;
        }
        byte[] uData = new byte[size];
        Read(ref uData);
        //读出来的都是unicode
        strOut = Encoding.UTF8.GetString(uData);
        return true;
    }
    public bool WriteString(ref string strOut)
    {
        if (m_stream == null)
        {
            return false;
        }

        byte[] uData = null;
        int iLenght = 0;
        if (!string.IsNullOrEmpty(strOut))
        {
            uData = Encoding.UTF8.GetBytes(strOut);
            iLenght = uData.Length;
            WriteInt(iLenght);
            Write(ref uData);
        }
        else
        {
            WriteInt(iLenght);
        }
        return true;
    }
    //public static uint StringInFileSize(ref string strOut)
    //{
    //    if (string.IsNullOrEmpty(strOut))
    //    {
    //        return (uint)StringHelper.s_UIntSize;
    //    }
    //    byte[] uData = Encoding.UTF8.GetBytes(strOut);
    //    return (uint)StringHelper.s_UIntSize + (uint)uData.Length;
    //}

    //public void WriteVector2(float x, float y)
    //{
    //    WriteFloat(x);
    //    WriteFloat(y);
    //}
    //public int ReadVector2(ref float x, ref float y)
    //{
    //    int iRef = ReadFloat(ref x);
    //    iRef = ReadFloat(ref y);
    //    return iRef;
    //}

    //public void WriteVector3(float x, float y, float z)
    //{
    //    WriteFloat(x);
    //    WriteFloat(y);
    //    WriteFloat(z);
    //}
    //public int ReadVector3(ref float x, ref float y, ref float z)
    //{
    //    int iRef = ReadFloat(ref x);
    //    iRef = ReadFloat(ref y);
    //    iRef = ReadFloat(ref z);
    //    return iRef;
    //}

    //#if UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN || UNITY_EDITOR
    //    public void ReadColor(ref Color col)
    //    {
    //        ReadFloat(ref col.r);
    //        ReadFloat(ref col.g);
    //        ReadFloat(ref col.b);
    //        ReadFloat(ref col.a);
    //    }
    //    public void WriteColor(ref Color col)
    //    {
    //        WriteFloat(col.r);
    //        WriteFloat(col.g);
    //        WriteFloat(col.b);
    //        WriteFloat(col.a);
    //    }

    //    public void WriteVector2(ref Vector2 vec)
    //    {
    //        WriteFloat(vec.x);
    //        WriteFloat(vec.y);
    //    }
    //    public int ReadVector2(ref Vector2 vec)
    //    {
    //        int iRef = ReadFloat(ref vec.x);
    //        iRef = ReadFloat(ref vec.y);
    //        return iRef;
    //    }

    //    public void WriteVector3(ref Vector3 vec)
    //    {
    //        WriteFloat(vec.x);
    //        WriteFloat(vec.y);
    //        WriteFloat(vec.z);
    //    }
    //    public int ReadVector3(ref Vector3 vec)
    //    {
    //        int iRef = ReadFloat(ref vec.x);
    //        iRef = ReadFloat(ref vec.y);
    //        iRef = ReadFloat(ref vec.z);
    //        return iRef;
    //    }

    //    public void ReadVector4(ref Vector4 vec)
    //    {
    //        ReadFloat(ref vec.x);
    //        ReadFloat(ref vec.y);
    //        ReadFloat(ref vec.z);
    //        ReadFloat(ref vec.w);
    //    }
    //    public void WriteVector4(ref Vector4 vec)
    //    {
    //        WriteFloat(vec.x);
    //        WriteFloat(vec.y);
    //        WriteFloat(vec.z);
    //        WriteFloat(vec.w);
    //    }
    //#endif
    //public void WriteFloat4(float x, float y, float z, float w)
    //{
    //    WriteFloat(x);
    //    WriteFloat(y);
    //    WriteFloat(z);
    //    WriteFloat(w);
    //}
    //public int ReadFloat4(ref float x, ref float y, ref float z, ref float w)
    //{
    //    int iRef = ReadFloat(ref x);
    //    iRef = ReadFloat(ref y);
    //    iRef = ReadFloat(ref z);
    //    iRef = ReadFloat(ref w);
    //    return iRef;
    //}
}