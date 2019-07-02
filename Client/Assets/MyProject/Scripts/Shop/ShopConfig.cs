using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 储存商店道具信息的类
/// </summary>
public class ShopConfig : ConfigBase
{
    string propId;
    string group_Id;
    string cost_Type;
    string cost_Num;

    public string PropId
    {
        get
        {
            return propId;
        }

        set
        {
            propId = value;
        }
    }

    public string Group_Id
    {
        get
        {
            return group_Id;
        }

        set
        {
            group_Id = value;
        }
    }

    public string Cost_Type
    {
        get
        {
            return cost_Type;
        }

        set
        {
            cost_Type = value;
        }
    }

    public string Cost_Num
    {
        get
        {
            return cost_Num;
        }

        set
        {
            cost_Num = value;
        }
    }
}
