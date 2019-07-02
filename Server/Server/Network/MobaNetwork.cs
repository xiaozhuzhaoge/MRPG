
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Collections.Generic;

public class MobaNetwork
{
    private const int _port = 10010;

    private static AsyncTcpServer _asyncTcpServer;

    private static Action<AsyncTcpClient> _onClientConnect;

    private static Action<AsyncTcpClient> _onClientDisconnect;


    public void Init(Action<AsyncTcpClient> onClientConnect, Action<AsyncTcpClient> onClientDisconnect)
    {
        // 获取以太网有线连接IP
        IPAddress ip = NetHelper.GetEthernetIP(NetworkInterfaceType.Ethernet);

        if (ip == null)
        {
            // 获取以太网无线连接IP
            ip = NetHelper.GetEthernetIP(NetworkInterfaceType.Wireless80211);
        }


        _asyncTcpServer = new AsyncTcpServer();
        _asyncTcpServer.Init(ip, _port);
        _asyncTcpServer.Start();

        _onClientConnect = onClientConnect;
        _onClientDisconnect = onClientDisconnect;

        MobaLog.Log("Server Start, ip = " + ip.ToString() + ", port:" + _port);
    }

    /// <summary>
    /// 客户端连接
    /// </summary>
    /// <param name="client"></param>
    public static void ClientConnect(AsyncTcpClient client)
    {
        EndPoint endpoint = null;

        endpoint = (client as AsyncTcpClient).TcpClient.Client.RemoteEndPoint;

        string ip = (endpoint as IPEndPoint).Address.ToString();

        _onClientConnect(client);

        Console.WriteLine("{0} Connected...", ip);
    }

    /// <summary>
    /// 客户端断开连接
    /// </summary>
    /// <param name="client"></param>
    public static void ClientDisConnect(AsyncTcpClient client)
    {
        EndPoint endpoint = (client as AsyncTcpClient).TcpClient.Client.RemoteEndPoint;

        string ip = (endpoint as IPEndPoint).Address.ToString();

        _onClientDisconnect(client);
        Console.WriteLine("{0} DisConnected...", ip);
    }

    /// <summary>
    /// 收到客户端消息
    /// </summary>
    /// <param name="client"></param>
    /// <param name="buffer"></param>
    public static void Recv(AsyncTcpClient client, byte[] buffer)
    {
        byte[] byteHeader = new byte[NetMessage.MsgHeaderLength];
        int index = 0;
        while (index < buffer.Length)
        {
            Array.Copy(buffer, index, byteHeader, 0, NetMessage.MsgHeaderLength);
            index += NetMessage.MsgHeaderLength;

            UInt16 msgid = NetMessage.GetMsgId(byteHeader);
            UInt16 bodyLen = NetMessage.GetMsgLength(byteHeader);

            Action<byte[], AsyncTcpClient> handler;

            try
            {
                if (bodyLen == 0)
                {
                    handler = NetMessage.GetMsgHandler(msgid);
                    handler(new byte[0], client);
                    //MobaNetwork.Log(AccountID, msgid);
                }
                else
                {
                    byte[] byteBody = new byte[bodyLen];
                    Array.Copy(buffer, index, byteBody, 0, bodyLen);
                    index += bodyLen;

                    handler = NetMessage.GetMsgHandler(msgid);
                    handler(byteBody, client);
                }
            }
            catch (Exception ex)
            {
                MobaLog.Log(ex);
            }
        }
    }

    /// <summary>
    /// 向客户端发送消息
    /// </summary>
    /// <param name="client"></param>
    /// <param name="msgid"></param>
    /// <param name="body"></param>
    public static void Send(AsyncTcpClient client, object msgobj, ushort msgid)
    {
        byte[] byteBody = ProtoBufUtils.Serialize(msgobj);

        byte[] byteHeader = new byte[NetMessage.MsgHeaderLength];

        // 消息ID
        byte[] byteMsgId = System.BitConverter.GetBytes(msgid);
        byteMsgId.CopyTo(byteHeader, 0);

        // 消息体长度
        UInt16 bodyLength = (UInt16)byteBody.Length;
        byte[] byteBodyLength = System.BitConverter.GetBytes(bodyLength);
        byteBodyLength.CopyTo(byteHeader, byteMsgId.Length);

        // 消息体
        byte[] msg = new byte[byteHeader.Length + byteBody.Length];
        byteHeader.CopyTo(msg, 0);
        byteBody.CopyTo(msg, byteHeader.Length);

        TcpClient tcpClient = (client as AsyncTcpClient).TcpClient;
        if (tcpClient.Connected)
            _asyncTcpServer.Send(tcpClient, msg);
    }

    /// <summary>
    /// 通知所有账号
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="msgid"></param>
    /// <param name="accounts"></param>
    public static void SendToAllAccount(object msg, ushort msgid, Dictionary<string, Account> accounts)
    {
        foreach (Account acc in accounts.Values)
        {
            MobaNetwork.Send(acc.client, msg, msgid);
        }
    }

    /// <summary>
    /// 通知所有其他账号
    /// </summary>
    /// <param name="msg">消息对象</param>
    /// <param name="msgid">消息ID</param>
    /// <param name="excludeUID">排除的账号ID</param>
    /// <param name="accounts">所有账号</param>
    public static void SendToOtherAccount(object msg, ushort msgid, uint excludeUID, Dictionary<string, Account> accounts)
    {
        foreach (Account acc in accounts.Values)
        {
            if (acc.client.uid != excludeUID)
            {
                MobaNetwork.Send(acc.client, msg, msgid);
            }
        }
    }

}