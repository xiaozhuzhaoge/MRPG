using System;
using System.Collections.Generic;
using System.Timers;


public class TimeHelper
{
    public static uint AppStartTime = 0;

    public static uint deltaTime;

    private static long nowTime = 0;
    public   static int Month
    {
        get
        {
            return DateTime.Now.Month;
        }
    }


    public static void Init()
    {
        AppStartTime = (uint)Environment.TickCount;
        MobaLog.Log("Timer Init...");
    }

    /// <summary>
    /// 获取当前时间
    /// </summary>
    /// <returns></returns>
    public static long GetMSTime()
    {
       nowTime = DateTime.UtcNow.Ticks / 10000;
        return nowTime;
    }

    /// <summary>
    /// 获取时间增量
    /// </summary>
    /// <param name="oldTime"></param>
    /// <param name="newTime"></param>
    /// <returns></returns>
    public static uint GetMSTimeDiff(uint oldTime, uint newTime)
    {
        return newTime - oldTime;
    }
}
