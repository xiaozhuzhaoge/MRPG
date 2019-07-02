using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class ConfigInfo {

    private Dictionary<string, SignInConfig> signIn = new Dictionary<string, SignInConfig>();

    public Dictionary<string, SignInConfig> SignIn
    {
        get
        {
            return signIn;
        }

        set
        {
            signIn = value;
        }
    }
 
    /// <summary>
    /// 获取指定天数和ID对应的道具数量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="days"></param>
    /// <returns></returns>
    public int GetNum(string id,int days)
    {
        if (days == 28)
            return Convert.ToInt32(SignIn[id].num28);
        if (days == 29)
            return Convert.ToInt32(SignIn[id].num29);
        if (days == 30)
            return Convert.ToInt32(SignIn[id].num30);
        if (days == 31)
            return Convert.ToInt32(SignIn[id].num31);
        return 0;
    }

    /// <summary>
    /// 获取指定天数和ID对应的道具数量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="days"></param>
    /// <returns></returns>
    public string GetPropId(string id, int days)
    {
        if (days == 28)
            return SignIn[id].Prop28;
        if (days == 29)
            return SignIn[id].Prop29;
        if (days == 30)
            return SignIn[id].Prop30;
        if (days == 31)
            return SignIn[id].Prop31;
        return "0";
    }
}
