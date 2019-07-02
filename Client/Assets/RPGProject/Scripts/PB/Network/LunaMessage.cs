using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public delegate void Response(byte[] res);
public class LunaMessage
{
    /// <summary>
    /// 消息头长度
    /// </summary>
    public const int MsgHeaderLength = 4;

    /// <summary>
    /// 消息总长度
    /// </summary>
    public const int MsgLength = 4000;



    private static Dictionary<int, Response> _msgHandlers = new Dictionary<int, Response>();

    private static Dictionary<int, Action<object>> _msgObjectHandlers = new Dictionary<int, Action<object>>();

    public static void ClearAllHandlers()
    {
        _msgHandlers.Clear();
    }

    public static void AddMsgHandler(int msgid, Response handler)
    {
        if (!_msgHandlers.ContainsKey(msgid))
            _msgHandlers.Add(msgid, handler);
    }
 

    public static Response GetMsgHandler(int msgid)
    {
        if (_msgHandlers.ContainsKey(msgid))
            return _msgHandlers[msgid];
        else
        {
            UnityEngine.Debug.LogError("Can not find msg id:{0}" +  msgid);
            return null;
        }
    }
 

    /// <summary>
    /// 获取消息体
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="msgLength"></param>
    /// <returns></returns>
    public static byte[] GetMsgBody(byte[] msg, int msgLength)
    {
        int bodyLength = msgLength - MsgHeaderLength;
        byte[] body = new byte[bodyLength];
        try
        {
            Array.Copy(msg, MsgHeaderLength, body, 0, bodyLength);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return body;
    }

    /// <summary>
    /// 获取消息体头
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] GetMsgHeader(byte[] msg)
    {
        byte[] header = new byte[MsgHeaderLength];
        try
        {
            Array.Copy(msg, 0, header, 0, MsgHeaderLength);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return header;
    }


    public static UInt16 GetMsgId(byte[] byteHeader)
    {
        byte[] byteMsgId = new byte[2];
        Array.Copy(byteHeader, 0, byteMsgId, 0, 2);
        return System.BitConverter.ToUInt16(byteMsgId, 0);
    }

    public static UInt16 GetMsgLength(byte[] byteHeader)
    {
        byte[] byteBodyLen = new byte[2];
        Array.Copy(byteHeader, 2, byteBodyLen, 0, 2);
        return System.BitConverter.ToUInt16(byteBodyLen, 0);
    }
}

