using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;


public class AsyncTcpClient
{
    public uint uid;
    public int _roleId;
    public int _accountId;

    public AsyncTcpClient(TcpClient tcpClient, byte[] buffer)
    {
        this.TcpClient = tcpClient;
        this.Buffer = buffer;
    }

    public TcpClient TcpClient { get; private set; }

    public byte[] Buffer { get; private set; }

    public NetworkStream NetworkStream
    {
        get { return TcpClient.GetStream(); }
    }
}



/// <summary>
/// 异步TCP服务器
/// </summary>
public class AsyncTcpServer
{

    private TcpListener listener;

    private List<AsyncTcpClient> clients;


    public IPAddress Address { get; private set; }

    public int Port { get; private set; }


    public void Init(IPAddress localIPAddress, int listenPort)
    {
        Address = localIPAddress;
        Port = listenPort;

        clients = new List<AsyncTcpClient>();

        listener = new TcpListener(Address, Port);
        listener.AllowNatTraversal(true);
    }


    /// <summary>
    /// 启动服务器
    /// </summary>
    /// <returns>异步TCP服务器</returns>
    public void Start()
    {
        listener.Start();
        listener.BeginAcceptTcpClient(new AsyncCallback(HandleTcpClientAccepted), listener);
    }

    /// <summary>
    /// 停止服务器
    /// </summary>
    /// <returns>异步TCP服务器</returns>
    public AsyncTcpServer Stop()
    {
        listener.Stop();

        lock (this.clients)
        {
            for (int i = 0; i < this.clients.Count; i++)
            {
                this.clients[i].TcpClient.Client.Disconnect(false);
            }
            this.clients.Clear();
        }

        return this;
    }

    /// <summary>
    /// 客户端连接回调
    /// </summary>
    /// <param name="ar"></param>
    private void HandleTcpClientAccepted(IAsyncResult ar)
    {
        TcpListener tcpListener = (TcpListener)ar.AsyncState;

        TcpClient tcpClient = tcpListener.EndAcceptTcpClient(ar);
        byte[] buffer = new byte[tcpClient.ReceiveBufferSize];

        AsyncTcpClient internalClient = new AsyncTcpClient(tcpClient, buffer);

        lock (this.clients)
        {
            this.clients.Add(internalClient);
            MobaNetwork.ClientConnect(internalClient);
        }

        NetworkStream networkStream = internalClient.NetworkStream;
        networkStream.BeginRead(internalClient.Buffer, 0, internalClient.Buffer.Length, HandleDatagramReceived, internalClient);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleTcpClientAccepted), ar.AsyncState);
    }

    /// <summary>
    /// 收到报文回调
    /// </summary>
    /// <param name="ar"></param>
    private void HandleDatagramReceived(IAsyncResult ar)
    {
        AsyncTcpClient internalClient = (AsyncTcpClient)ar.AsyncState;
        NetworkStream networkStream = internalClient.NetworkStream;

        int numberOfReadBytes = 0;

        try
        {
            numberOfReadBytes = networkStream.EndRead(ar);
        }
        catch
        {
            numberOfReadBytes = 0;
        }

        if (numberOfReadBytes == 0)
        {
            lock (this.clients)
            {
                MobaNetwork.ClientDisConnect(internalClient);
                this.clients.Remove(internalClient);

                return;
            }
        }

        byte[] receivedBytes = new byte[numberOfReadBytes];
        Buffer.BlockCopy(internalClient.Buffer, 0, receivedBytes, 0, numberOfReadBytes);

        MobaNetwork.Recv(internalClient, receivedBytes);

        networkStream.BeginRead(internalClient.Buffer, 0, internalClient.Buffer.Length, HandleDatagramReceived, internalClient);
    }

    /// <summary>
    /// 发送报文至指定的客户端
    /// </summary>
    /// <param name="tcpClient">客户端</param>
    /// <param name="datagram">报文</param>
    public void Send(TcpClient tcpClient, byte[] datagram)
    {
        try
        {
            tcpClient.GetStream().BeginWrite(datagram, 0, datagram.Length, HandleDatagramWritten, tcpClient);
        }
        catch (Exception ex)
        {
            MobaLog.Log("Client Has Disconnect...");
        }
    }

    private void HandleDatagramWritten(IAsyncResult ar)
    {
        ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);
    }

    /// <summary>
    /// 发送报文至所有客户端
    /// </summary>
    /// <param name="datagram">报文</param>
    public void SendAll(byte[] datagram)
    {
        for (int i = 0; i < this.clients.Count; i++)
        {
            Send(this.clients[i].TcpClient, datagram);
        }
    }
}