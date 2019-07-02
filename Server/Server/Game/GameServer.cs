using System;
using System.Collections.Generic;
using System.Threading;

public class GameServer
{

    // 游戏循环线程
    private Thread _gameloopThread;

    private bool _gameloopRunning = false;

    private const int World_Sleep_Const = 50;

    private TimeHelper _timerHelper;

    private MobaLog _log;

    private MobaDB _db;

    private MobaNetwork _net;

    private AccountManager _accountManager;


    public void Startup()
    {
        _log = new MobaLog();
        _log.Init();

        _timerHelper = new TimeHelper();
        TimeHelper.Init();

        _db = new MobaDB();
        _db.Init();

        _accountManager = new AccountManager();


        MsgHandler.RegisterMsg();

        _net = new MobaNetwork();
        _net.Init(OnClientConnect, OnClientDisConnect);

        _gameloopRunning = true;

        _gameloopThread = new Thread(Run);
        _gameloopThread.Start();
    }


    private void Run()
    {
        // 当前时间
        uint realCurrTime = 0;

        // 上一次更新开始的时间
        uint realPrevTime = (uint)TimeHelper.GetMSTime();

        // 上一次更新Sleep的时间
        uint prevSleepTime = 0;

        while (_gameloopRunning)
        {
            // 获取当前时间
            realCurrTime = (uint)TimeHelper.GetMSTime();

            // 更新增量时间
            uint diff = TimeHelper.GetMSTimeDiff(realPrevTime, realCurrTime);
            TimeHelper.deltaTime = diff;

            float framedt = diff / 1000f;
            realPrevTime = realCurrTime;

            if (diff <= World_Sleep_Const + prevSleepTime)
            {
                prevSleepTime = World_Sleep_Const + prevSleepTime - diff;
                Thread.Sleep((int)prevSleepTime);
            }
            else
            {
                prevSleepTime = 0;
            }
        }
    }


    private void OnClientConnect(AsyncTcpClient client)
    {
        Account acc = AccountManager.GetAccount(client.uid);
        if (acc == null)
        {
            acc = AccountManager.CreateAccount(client);
        }
        else
        {
            MobaLog.Log(string.Format("{0} has connected...", client.uid));
        }
    }

    private void OnClientDisConnect(AsyncTcpClient client)
    {
        Account acc = AccountManager.GetAccount(client.uid);
        if (acc == null)
        {
            MobaLog.Log("Cannot find account:" + client.uid);
            return;
        }
        else
        {
            AccountManager.RemoveAccount(client.uid);
        }
    }

    public void Shutdown()
    {

    }

}