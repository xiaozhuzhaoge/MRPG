using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Globalization;
using System.Threading;
using UnityEngine;

/// <summary>
/// 异步TCP客户端
/// </summary>
public class AsyncTcpClient : IDisposable
{
    private TcpClient tcpClient;
    private bool disposed = false;

    // 重试连接的次数
    private int retries = 0;

    private string _ip;


    /// <summary>
    /// 是否已与服务器建立连接
    /// </summary>
    public bool Connected
    {
        get
        {
            return tcpClient.Client.Connected;
        }
    }

    /// <summary>
    /// 远端服务器的IP地址列表
    /// </summary>
    public IPAddress[] Addresses { get; private set; }

    /// <summary>
    /// 远端服务器的端口
    /// </summary>
    public int _port { get; private set; }

    /// <summary>
    /// 连接重试次数
    /// </summary>
    public int Retries { get; set; }

    /// <summary>
    /// 连接重试间隔
    /// </summary>
    public int RetryInterval { get; set; }


    /// <summary>
    /// 本地客户端终结点
    /// </summary>
    protected IPEndPoint LocalIPEndPoint { get; private set; }

    /// <summary>
    /// 通信所使用的编码
    /// </summary>
    public Encoding Encoding { get; set; }


    public void Init(string ip, int port)
    {
        _ip = ip;
        _port = port;
        this.tcpClient = new TcpClient();

        Connect();
    }

    private void Connect()
    {
        if (!Connected)
        {
            try
            {
                tcpClient.BeginConnect(_ip, _port, HandleTcpServerConnected, tcpClient);
            }
            catch (System.Exception ex)
            {
                Debug.Log(string.Format("BeginConnect Exception:{0}", ex));
            }
        }
    }

    /// <summary>
    /// 关闭与服务器的连接
    /// </summary>
    /// <returns>异步TCP客户端</returns>
    public AsyncTcpClient Close()
    {
        if (Connected)
        {
            retries = 0;
            tcpClient.Close();
            MobaNetwork.OnDisConnectServer(_ip, _port);
        }

        return this;
    }


    private void HandleTcpServerConnected(IAsyncResult ar)
    {
        try
        {
            tcpClient.EndConnect(ar);
            MobaNetwork.OnConnectServer(_ip, _port);

            retries = 0;
        }
        catch (Exception ex)
        {
            MobaLog.Log(ex);

            if (retries > 0)
            {
                MobaLog.Log(string.Format(CultureInfo.InvariantCulture, "Connect to server with retry {0} failed.", retries));
            }

            retries++;
            if (retries > Retries)
            {
                MobaNetwork.OnServerException(_ip, _port, ex);
                return;
            }
            else
            {
                MobaLog.Log(string.Format(CultureInfo.InvariantCulture, "Waiting {0} seconds before retrying to connect to server.", RetryInterval));
                Thread.Sleep(TimeSpan.FromSeconds(RetryInterval));
                Connect();
                return;
            }
        }

        byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
        tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, HandleDatagramReceived, buffer);
    }

    private void HandleDatagramReceived(IAsyncResult ar)
    {
        NetworkStream stream = tcpClient.GetStream();

        int numberOfReadBytes = 0;
        try
        {
            numberOfReadBytes = stream.EndRead(ar);
        }
        catch
        {
            numberOfReadBytes = 0;
        }

        if (numberOfReadBytes == 0)
        {
            Close();
            return;
        }

        byte[] buffer = (byte[])ar.AsyncState;
        byte[] receivedBytes = new byte[numberOfReadBytes];
        Buffer.BlockCopy(buffer, 0, receivedBytes, 0, numberOfReadBytes);

        MobaNetwork.Recv(receivedBytes);

        stream.BeginRead(buffer, 0, buffer.Length, HandleDatagramReceived, buffer);
    }



    /// <summary>
    /// 发送报文
    /// </summary>
    /// <param name="datagram">报文</param>
    public void Send(byte[] datagram)
    {
        if (datagram == null)
            throw new ArgumentNullException("datagram");

        if (!Connected)
        {
            MobaNetwork.OnDisConnectServer(_ip, _port);
            throw new InvalidProgramException("This client has not connected to server.");
        }

        tcpClient.GetStream().BeginWrite(datagram, 0, datagram.Length, HandleDatagramWritten, tcpClient);
    }

    private void HandleDatagramWritten(IAsyncResult ar)
    {
        ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);
    }

    /// <summary>
    /// 发送报文
    /// </summary>
    /// <param name="datagram">报文</param>
    public void Send(string datagram)
    {
        Send(this.Encoding.GetBytes(datagram));
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                try
                {
                    Close();

                    if (tcpClient != null)
                    {
                        tcpClient = null;
                    }
                }
                catch (SocketException ex)
                {
                    //ExceptionHandler.Handle(ex);
                }
            }

            disposed = true;
        }
    }
}