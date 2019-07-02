using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public enum AttributeType
{
    MaxHp,
    MaxMp,
    Atk,
    Def,
    Cri,
    Adv,
    AtkSpeed,
    MoveSpeed,
}

public class PlayerConfig:ConfigBase {

     
    string name;
     
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

    public double Atk
    {
        get
        {
            return atk;
        }

        set
        {
            atk = value;
        }
    }

    public double Def
    {
        get
        {
            return def;
        }

        set
        {
            def = value;
        }
    }

    public double Cri
    {
        get
        {
            return cri;
        }

        set
        {
            cri = value;
        }
    }

    public double Adv
    {
        get
        {
            return adv;
        }

        set
        {
            adv = value;
        }
    }

    public double Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
        }
    }

    public string SkillGroup
    {
        get
        {
            return skillGroup;
        }

        set
        {
            skillGroup = value;
            Skills = new Dictionary<AttributeType, float>();
            JsonData data = JsonMapper.ToObject(skillGroup);
            
            CreateSkillData(data);
        }
    }

    public double AtkSpeed
    {
        get
        {
            return atkSpeed;
        }

        set
        {
            atkSpeed = value;
        }
    }

    public double MoveSpeed
    {
        get
        {
            return moveSpeed;
        }

        set
        {
            moveSpeed = value;
        }
    }

    double atk;
    double def;
    double cri;
    double adv;
    double hp;
    double atkSpeed;
    double moveSpeed;
    string skillGroup;

    public Dictionary<AttributeType, float> Skills;

    public void CreateSkillData(JsonData data)
    {
        foreach(object type in Enum.GetValues(typeof(AttributeType)))
        {
            AddSkillData(data, (AttributeType)type);
        }
    }
   
    public void AddSkillData(JsonData data,AttributeType fieldName)
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
