using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class PropInfoConfig : ConfigBase {

    string name;
    string type;
    string icon;
    string skill;
    string canCombine;

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public string Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public string Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public string Skill
    {
        get
        {
            return skill;
        }

        set
        {
           
            skill = value;
            Skills = new Dictionary<AttributeType, float>();
            JsonData data = JsonMapper.ToObject(skill);
            CreateSkillData(data);

        }
    }

    public string CanCombine
    {
        get
        {
            return canCombine;
        }

        set
        {
            canCombine = value;
        }
    }

    public Dictionary<AttributeType, float> Skills;

    public void CreateSkillData(JsonData data)
    {
        foreach (object type in Enum.GetValues(typeof(AttributeType)))
        {
            AddSkillData(data, (AttributeType)type);
        }
    }

    public void AddSkillData(JsonData data, AttributeType fieldName)
    {
        string field = fieldName.ToString();
        if (data.ContainsKey(field))
        {
            Skills.Add(fieldName, Convert.ToSingle(data[field].ToString()));
        }
        else
        {
            Skills.Add(fieldName, 0);
        }

    }
}
