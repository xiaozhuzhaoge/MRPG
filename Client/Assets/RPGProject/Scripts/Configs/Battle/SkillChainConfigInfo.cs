using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConfigInfo : MonoSingleton<ConfigInfo>
{
    private Dictionary<string, SkillChain> skill;

    private Dictionary<string, List<SkillChain>> skillgrounps;

    public Dictionary<string, SkillChain> Skill
    {
        get
        {
            return skill;
        }
        set
        {
            skill = value;
            Skillgrounps = new Dictionary<string, List<SkillChain>>();
            foreach (var data in value.Values)
            {
                if (!Skillgrounps.ContainsKey(data.GroupId))
                    Skillgrounps.Add(data.GroupId, new List<SkillChain>());
                Skillgrounps[data.GroupId].Add(data);
            }
        }
    }

    public Dictionary<string, List<SkillChain>> Skillgrounps
    {
        get
        {
            return skillgrounps;
        }

        set
        {
            skillgrounps = value;
        }
    }

    /// <summary>
    /// 获取指定技能组信息
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public static List<SkillChain> GetSkillChains(string groupId)
    {
        Debug.Log(groupId);
        return Instance.Skillgrounps[groupId];
    }
}
