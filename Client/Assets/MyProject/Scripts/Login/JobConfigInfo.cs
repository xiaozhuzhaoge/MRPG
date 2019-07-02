using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConfigInfo{

    Dictionary<string, JobConfig> jobs = new Dictionary<string, JobConfig>();

    public Dictionary<string, JobConfig> Jobs
    {
        get
        {
            return jobs;
        }

        set
        {
            jobs = value;
        }
    }

    /// <summary>
    /// 获取指定职业的图标
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetJobIcon(string type)
    {
        if (Jobs.ContainsKey(type))
        {
            return Jobs[type].Icon;
        }
        return null;
    }
}
