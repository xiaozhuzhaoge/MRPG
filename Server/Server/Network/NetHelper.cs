using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.NetworkInformation;
public class NetHelper
{

    /// <summary>
    /// 获取本机局域网地址
    /// </summary>
    /// <returns></returns>
    public static string GetLocalIpString()
    {
        string hostname = Dns.GetHostName();
        IPHostEntry localHost = Dns.GetHostEntry(hostname);
        IPAddress localAddr = localHost.AddressList[0];
        return localAddr.ToString();
    }

    public static IPAddress GetLocalIpAddress()
    {
        string hostname = Dns.GetHostName();
        IPHostEntry localHost = Dns.GetHostEntry(hostname);
        IPAddress localAddr = localHost.AddressList[2];
        return localAddr;
    }

    public static IPAddress GetEthernetIP(NetworkInterfaceType type)
    {
        //获取所有网卡信息
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in nics)
        {
            //判断是否为以太网卡
            if (adapter.NetworkInterfaceType == type && adapter.OperationalStatus == OperationalStatus.Up)
            {
                //获取以太网卡网络接口信息
                IPInterfaceProperties ip = adapter.GetIPProperties();

                //获取单播地址集
                UnicastIPAddressInformationCollection ipCollection = ip.UnicastAddresses;
                foreach (UnicastIPAddressInformation ipadd in ipCollection)
                {
                    // InterNetwork    IPV4地址      
                    // InterNetworkV6        IPV6地址
                    // Max            MAX 位址
                    if (ipadd.Address.AddressFamily == AddressFamily.InterNetwork)
                        //判断是否为ipv4
                        return ipadd.Address;//获取ip
                }
            }
        }

        return null;
    }

    //从网站"http://1111.ip138.com/ic.asp"，获取本机外网ip地址信息串
    //"<html>\r\n<head>\r\n<meta http-equiv=\"content-type\" content=\"text/html; charset=gb2312\">\r\n<title> 
    //您的IP地址 </title>\r\n</head>\r\n<body style=\"margin:0px\"><center>您的IP是：[218.104.71.178] 来自：安徽省合肥市 联通</center></body></html>"

    //获取外网ip地址
    public static string GetExtenalIpAddress()
    {
        string IP = "未获取到外网ip";
        try
        {
            //从网址中获取本机ip数据
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.Default;
            //string str = client.DownloadString("http://1111.ip138.com/ic.asp");
            //string str = client.DownloadString("http://www.ip138.com/ip2city.asp");
            string str = client.DownloadString("http://www.ip138.com/ip2city.asp");
            client.Dispose();

            //提取外网ip数据 [218.104.71.178]
            int i1 = str.IndexOf("[");
            int i2 = str.IndexOf("]");

            IP = str.Substring(i1 + 1, i2 - 1 - i1);
        }
        catch (Exception)
        {
        }

        return IP;
    }

    /// <summary>
    /// 结构体转字节数组
    /// </summary>
    /// <param name="structObj"></param>
    /// <returns></returns>
    public static byte[] StructToBytes(object structObj)
    {
        int size = Marshal.SizeOf(structObj);
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.StructureToPtr(structObj, buffer, false);
            byte[] bytes = new byte[size];
            Marshal.Copy(buffer, bytes, 0, size);
            return bytes;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    /// <summary>
    /// 字节数组转成结构体
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="structType"></param>
    /// <returns></returns>
    private static object BytesToStruct(byte[] bytes, Type structType)
    {
        int size = Marshal.SizeOf(structType);
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.Copy(bytes, 0, buffer, size);
            return Marshal.PtrToStructure(buffer, structType);
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    /// <summary>
    /// 序列化class为byte[]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static byte[] Serialize<T>(T obj) where T : class
    {

        IFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();

        //try
        //{
        //    formatter.Serialize(stream, obj);
        //}
        //catch(Exception e)
        //{
        //    Debug.LogError(e);
        //}     

        //stream.Position = 0;
        //byte[] buffer = new byte[stream.Length];
        //stream.Read(buffer, 0, buffer.Length);
        //stream.Flush();
        //stream.Close();
        //return buffer;

        formatter.Serialize(stream, obj);
        return stream.GetBuffer();
    }

    /// <summary>
    /// 序列化byte[]为class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newT"></param>
    /// <param name="buffer"></param>
    /// <returns></returns>
    //public static T Deserialize<T>(T newT, byte[] buffer) where T : class
    //{
    //    IFormatter formatter = new BinaryFormatter();
    //    MemoryStream stream = new MemoryStream(buffer);

    //    try
    //    {
    //        newT = (T)formatter.Deserialize(stream);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError(e);
    //    }

    //    stream.Flush();
    //    stream.Close();

    //    return newT;
    //}

    public static T Deserialize<T>(byte[] buffer) where T : class
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream(buffer);

        return (T)formatter.Deserialize(stream);
    }
}