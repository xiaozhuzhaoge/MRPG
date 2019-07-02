
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetMessage
{
    public NetMessage(byte[] header, byte[] body)
    {
        _header = header;
        _body = body;
    }

    public byte[] _header { get; private set; }

    public byte[] _body { get; private set; }
}

public class MobaNetwork
{
    private static int _port = 10010;

    private static string _ip = "192.168.12.168";

    private static AsyncTcpClient _asyncTcpClient;

    private static Queue<NetMessage> _msgQueue;


    public static void ConnectServer(string ip, int port = 10010)
    {
        _msgQueue = null;
        _asyncTcpClient = null;
        _port = port;
        _ip = ip;
        _msgQueue = new Queue<NetMessage>();
        _asyncTcpClient = new AsyncTcpClient();
        _asyncTcpClient.Init(_ip, _port);
    }

    public static void End()
    {
        _asyncTcpClient.Close();
    }


    public static void OnConnectServer(string ip, int port)
    {
        string str = string.Format("Connect Server {0}:{1}", ip, port);
        MobaLog.Log(str);
    }

    public static void OnDisConnectServer(string ip, int port)
    {
        string str = string.Format("DisConnect Server {0}:{1}", ip, port);
        MobaLog.Log(str);
    }

    public static void OnServerException(string ip, int port, Exception ex)
    {
        MobaLog.Log("OnServerException");
        MobaLog.Log(ex);
    }

    public static void Recv(byte[] buffer)
    {
        Debug.Log(buffer.Length);

        byte[] byteHeader = new byte[LunaMessage.MsgHeaderLength];
        int index = 0;
        while (index < buffer.Length)
        {
            Array.Copy(buffer, index, byteHeader, 0, LunaMessage.MsgHeaderLength);
            index += LunaMessage.MsgHeaderLength;

            UInt16 bodyLen = LunaMessage.GetMsgLength(byteHeader);

            // 客户端从缓冲区读取消息之后，首先将消息放到消息队列中，在主线程更新时，再执行消息响应事件
            // 因为只有在主线程中才能访问Unity的对象，组件和方法
            if (bodyLen == 0)
            {
                lock (_msgQueue)
                {
                    NetMessage msg = new NetMessage(byteHeader, null);
                    _msgQueue.Enqueue(msg);
                }
            }
            else
            {
                byte[] byteBody = new byte[bodyLen];
                Array.Copy(buffer, index, byteBody, 0, bodyLen);
                index += bodyLen;

                lock (_msgQueue)
                {
                    NetMessage msg = new NetMessage(byteHeader, byteBody);
                    _msgQueue.Enqueue(msg);
   
                }
            }
        }
    }

    /// <summary>
    /// 发送文本信息
    /// </summary>
    /// <param name="msg"></param>
    public static void Send(string msg)
    {
        _asyncTcpClient.Send(msg);
    }

    /// <summary>
    /// 发送消息ID以及消息对象 ProtoBuffer 数据
    /// </summary>
    /// <param name="msgid"></param>
    /// <param name="obj"></param>
    public static void Send(ushort msgid, object obj)
    {
        byte[] byteBody = ProtoBufUtils.Serialize(obj);
        byte[] byteHeader = new byte[LunaMessage.MsgHeaderLength];

        byte[] byteMsgId = System.BitConverter.GetBytes(msgid);
        byteMsgId.CopyTo(byteHeader, 0);

        ushort bodyLength = (ushort)byteBody.Length;
        byte[] byteBodyLength = System.BitConverter.GetBytes(bodyLength);
        byteBodyLength.CopyTo(byteHeader, byteMsgId.Length);

        //Console.WriteLine("Server Send Msg : ModID : " + ModID + " FunID: " + FunID);

        byte[] msg = new byte[byteHeader.Length + byteBody.Length];
        byteHeader.CopyTo(msg, 0);
        byteBody.CopyTo(msg, byteHeader.Length);

        _asyncTcpClient.Send(msg);

        Log(msgid, true);
    }

    public static void Log(ushort msgid, bool send = false)
    {
        Type msgType = null;

        //string text = Enum.GetName(msgType, msgid);
        //if (send)
        //    MobaLog.Log("Send: " + text);
        //else
        //    MobaLog.Log("Recv: " + text);
    }

    public static void Update()
    {
        while (_msgQueue.Count > 0)
        {
            NetMessage msg = null;
            lock (_msgQueue)
            {
                msg = _msgQueue.Dequeue();
            }

            UInt16 msgid = LunaMessage.GetMsgId(msg._header);
            Debug.Log(msgid);
            Response handler = LunaMessage.GetMsgHandler(msgid);
             
            try
            {
                handler(msg._body);
            }
            catch (Exception ex)
            {
                MobaLog.Log(ex);
            }


            MobaNetwork.Log(msgid);
        }
    }
}