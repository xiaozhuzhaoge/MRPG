using System;
using System.Collections.Generic;


public enum LoginMsg
{
    /// <summary>
    /// 注册
    /// </summary>
    ReqRegist = 1001,
    RespRegist = 1002,

    /// <summary>
    /// 登录
    /// </summary>
    ReqLogin = 1003,
    RespLogin = 1004,

    /// <summary>
    /// 创建角色
    /// </summary>
    ReqCreateChar = 1005,
    RespCreateChar = 1006,

    /// <summary>
    /// 进入世界
    /// </summary>
    ReqEnterWorld = 1007,
    RespEnterWorld = 1008,

    /// <summary>
    /// 游客登录
    /// </summary>
    ReqTouristLogin = 1011,
    RespTouristLogin = 1012,
}


public enum LevelMsg
{

    /// <summary>
    /// 角色出生
    /// </summary>
    ReqRoleBorn = 2005,
    RespRoleBorn = 2006,

    NotifyRoleReborn    = 2010,
    NotifyRoleDie       = 2011,
    NotifyRoleMove      = 2012,
    NotifyRoleAttack    = 2013,
    NotifyHPChange      = 2014,
    NotifyRoleIdle      = 2015,

    ReqMove             = 2021,
    ReqAttack           = 2023,
}


public enum WorldMsg
{
    ReqPraticeLevel = 3001,
    RespPraticeLevel = 3002,

    ReqActualCombat = 3003,
    RespActualCombat = 3004,

    ReqCancelMatch = 3101,
    RespCancelMatch = 3102,

    ReqChooseHero = 3103,
    NotifyChooseHero = 3104,

    ReqReady = 3105,
    NotifyReady = 3106,

    /// <summary>
    /// 进入关卡
    /// </summary>
    ReqEnterLevel = 3107,
    NotifyEnterLevel = 3108,


    /// <summary>
    /// 退出关卡
    /// </summary>
    ReqExitLevel = 3109,
    NotifyExitLevel = 3110,
}

