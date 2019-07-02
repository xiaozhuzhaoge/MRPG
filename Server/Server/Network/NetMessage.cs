using System;
using System.Collections.Generic;

public class NetMessage
{
    /// <summary>
    /// 消息头长度
    /// </summary>
    public const int MsgHeaderLength = 4;

    /// <summary>
    /// 消息总长度
    /// </summary>
    public const int MsgLength = 4000;

    // 消息ID和消息响应事件容器
    private static Dictionary<int, Action<byte[], AsyncTcpClient>> _msgHandlers = new Dictionary<int, Action<byte[], AsyncTcpClient>>();

    /// <summary>
    /// 往消息响应事件容器中添加一个元素
    /// </summary>
    /// <param name="msgid"></param>
    /// <param name="handler"></param>
    public static void AddMsgHandler(int msgid, Action<byte[], AsyncTcpClient> handler)
    {
        if (!_msgHandlers.ContainsKey(msgid))
            _msgHandlers.Add(msgid, handler);
    }


    public static Action<byte[], AsyncTcpClient> GetMsgHandler(int msgid)
    {
        if (_msgHandlers.ContainsKey(msgid))
            return _msgHandlers[msgid];
        else
        {
            Console.WriteLine("Can not find msg id:{0}", msgid);
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
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
        
        return body;
    }

    /// <summary>
    /// 获取消息头
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
        catch(Exception e)
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

