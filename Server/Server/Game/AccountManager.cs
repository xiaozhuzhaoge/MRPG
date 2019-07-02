using System;
using System.Collections.Generic;

/// <summary>
/// 账号
/// </summary>
public class Account
{
    /// <summary>
    /// 唯一ID
    /// </summary>
    private uint _uid;

    // 账号名
    private string _accountName;

    // 密码
    private string _password;

    // 连接对象
    private AsyncTcpClient _client;


    public string accountName
    {
        get
        {
            return _accountName;
        }
    }
    public AsyncTcpClient client
    {
        get
        {
            return _client;
        }
    }

    public Account(uint uid, AsyncTcpClient client)
    {
        _uid = uid;
        _client = client;
    }


    public Account(uint uid, string accountid, string password, AsyncTcpClient client)
    {
        _uid = uid;

        _accountName = accountid;
        _password = password;
        _client = client;
    }

}
public class AccountManager
{
    // 已经登录的玩家信息容器
    private static Dictionary<uint, Account> _Accounts;
    public static Dictionary<uint, Account> Accounts
    {
        get { return _Accounts; }
    }

    private static uint __uid = 10000001;


    public AccountManager()
    {
        _Accounts = new Dictionary<uint, Account>();
        MobaLog.Log("AccountManager Initialized...");
    }

    /// <summary>
    /// 从容器中移除一个账户
    /// </summary>
    /// <param name="uid"></param>
    public static void RemoveAccount(uint uid)
    {
        if (_Accounts.ContainsKey(uid))
        {
            _Accounts.Remove(uid);
            MobaLog.Log("{0} logout..." + uid);
        }
    }

    public static Account GetAccount(uint uid)
    {
        if (_Accounts.ContainsKey(uid))
            return _Accounts[uid];

        return null;
    }

    /// <summary>
    /// 客户端连接时创建账号
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    public static Account CreateAccount(AsyncTcpClient client)
    {
        Account acc = new Account(__uid, client);
        client.uid = __uid;

        if (!_Accounts.ContainsKey(__uid))
        {
            _Accounts.Add(__uid, acc);
            MobaLog.Log(string.Format("Account {0} Connect", __uid));
        }

        __uid++;

        return acc;
    }

 
}