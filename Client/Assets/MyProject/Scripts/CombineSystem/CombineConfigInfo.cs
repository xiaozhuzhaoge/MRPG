using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public partial class ConfigInfo {

    Dictionary<string, CombineConfig> combine = new Dictionary<string, CombineConfig>();
    public Dictionary<string, CombineConfig> Combine合成公式 = new Dictionary<string, CombineConfig>();

    public Dictionary<string, CombineConfig> Combine
    {
        get
        {
            return combine;
        }

        set
        {
            combine = value;
            foreach(var 每一行数据 in combine.Values)
            {
                Combine合成公式.Add(每一行数据.Combine, 每一行数据);
            }
        }
    }
}
