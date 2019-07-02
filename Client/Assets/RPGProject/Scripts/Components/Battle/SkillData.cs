using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

public class SkillData {
    
    public enum AttackType
    {
        Near,
        Throw
    }

    public AttackType atkType;
    public string Type;
    public NearSkillData nearSkillData;
    public ThrowSkillData throwSkillData;
    public float SkillAttack;
    
    public void SetJsonData(string data)
    {
        JsonData skillData = JsonMapper.ToObject(data);
        if (skillData.ContainsKey("Type"))
        {
            Type = Utility.CTString(skillData["Type"]);
        }
        if (skillData.ContainsKey("NearAttackType"))
        {
            atkType = AttackType.Near;
            JsonData nearAttack = skillData["NearAttackType"];
            nearSkillData = new NearSkillData(nearAttack);
        }
        if (skillData.ContainsKey("ThrowAttackType"))
        {
            atkType = AttackType.Throw;
            JsonData throwAttack = skillData["ThrowAttackType"];
            throwSkillData = new ThrowSkillData(throwAttack);
        }
        if (skillData.ContainsKey("SkillAttack"))
        {
            SkillAttack = Utility.CTFloat(skillData["SkillAttack"].ToString());
        }
    }
	
}

/// <summary>
/// 近距离技能数据
/// </summary>
public class NearSkillData
{
    /// <summary>
    /// 相对位置
    /// </summary>
    public Vector3 LocalPosition;
    /// <summary>
    /// 相对方向
    /// </summary>
    public Vector3 LocalDir;
    /// <summary>
    /// 半径
    /// </summary>
    public float Radius;
    /// <summary>
    /// 角度
    /// </summary>
    public float Angle;
    /// <summary>
    /// 射线数量
    /// </summary>
    public int RayNum;
    /// <summary>
    /// 飞翔距离 (如果是球形射线则有效)
    /// </summary>
    public float Distance;
    /// <summary>
    /// 受击特效名
    /// </summary>
    public string HitEffect;

    public AppearEffectData EffectData;

    public bool ReParent = true;

    public NearSkillData(JsonData nearAttack)
    {
        
        if (nearAttack.ContainsKey("LocalPosition"))
        {
            LocalPosition = Utility.GetPosV3(nearAttack["LocalPosition"].ToString());
        }
        if (nearAttack.ContainsKey("LocalDir"))
        {
            LocalDir = Utility.GetPosV3(nearAttack["LocalDir"].ToString());
        }
        if (nearAttack.ContainsKey("Radius"))
        {
            Radius = Utility.CTFloat(nearAttack["Radius"].ToString());
        }
        if (nearAttack.ContainsKey("Angle"))
        {
            Angle = Utility.CTFloat(nearAttack["Angle"].ToString());
        }
        if (nearAttack.ContainsKey("RayNum"))
        {
            RayNum = Utility.CTInt(nearAttack["RayNum"].ToString());
        }
        if (nearAttack.ContainsKey("Distance"))
        {
            Distance = Utility.CTFloat(nearAttack["Distance"].ToString());
        }
        if (nearAttack.ContainsKey("HitEffect"))
        {
            HitEffect = Utility.CTString(nearAttack["HitEffect"].ToString());
        }
         
        if (nearAttack.ContainsKey("AppearEffect"))
        {
            JsonData appearEffect = nearAttack["AppearEffect"];
            EffectData = new AppearEffectData(appearEffect);
        }
        if (nearAttack.ContainsKey("ReParent"))
        {
            ReParent = Utility.CTBoolean(nearAttack["ReParent"].ToString());
        }
    }
}

public class AppearEffectData
{
    /// <summary>
    /// 相对坐标
    /// </summary>
    public Vector3 LocalPosition;
    /// <summary>
    /// 相对位置
    /// </summary>
    public Vector3 LocalDir;
    /// <summary>
    /// 持续时间
    /// </summary>
    public float Duration;
    /// <summary>
    /// 特效资源路径
    /// </summary>
    public string EffectName;

    /// <summary>
    /// 重置特效至子类
    /// </summary>
    public bool Reparent;

    public AppearEffectData(JsonData data)
    {
        if (data.ContainsKey("LocalPosition"))
        {
            LocalPosition = Utility.GetPosV3(data["LocalPosition"].ToString());
        }
        if (data.ContainsKey("LocalDir"))
        {
            LocalDir = Utility.GetPosV3(data["LocalDir"].ToString());
        }
        if (data.ContainsKey("Duration"))
        {
            Duration = Utility.CTFloat(data["Duration"].ToString());
        }
        if (data.ContainsKey("EffectName"))
        {
            EffectName = Utility.CTString(data["EffectName"].ToString());
        }
        if (data.ContainsKey("ReParent"))
        {
            Reparent = Utility.CTBoolean(data["ReParent"].ToString());
        }
    }
}

public class ThrowSkillData
{

    /// <summary>
    /// 相对位置
    /// </summary>
    public Vector3 LocalPosition;
    /// <summary>
    /// 相对方向
    /// </summary>
    public Vector3 LocalDir;
    /// <summary>
    /// 飞翔距离 (如果是球形射线则有效)
    /// </summary>
    public float Distance;
    /// <summary>
    /// 受击特效名
    /// </summary>
    public string HitEffect;
    /// <summary>
    /// 持续时间
    /// </summary>
    public float Duration;
    /// <summary>
    /// 飞翔速度
    /// </summary>
    public float Speed;
    /// <summary>
    /// 特效外观
    /// </summary>
    public string Appearance;

    /// <summary>
    /// 更改父类
    /// </summary>
    public bool ReParent;

    public ThrowSkillData(JsonData data)
    {
      
        if (data.ContainsKey("LocalPosition"))
        {
            LocalPosition = Utility.GetPosV3(data["LocalPosition"].ToString());
        }
        if (data.ContainsKey("LocalDir"))
        {
            LocalDir = Utility.GetPosV3(data["LocalDir"].ToString());
        }
        if (data.ContainsKey("Distance"))
        {
            Distance = Utility.CTFloat(data["Distance"].ToString());
        }
        if (data.ContainsKey("HitEffect"))
        {
            HitEffect = Utility.CTString(data["HitEffect"].ToString());
        }
        if (data.ContainsKey("Duration"))
        {
            Duration = Utility.CTFloat(data["Duration"].ToString());
        }
        if (data.ContainsKey("Speed"))
        {
            Speed = Utility.CTFloat(data["Speed"].ToString());
        }
        if (data.ContainsKey("Duration"))
        {
            Duration = Utility.CTFloat(data["Duration"].ToString());
        }
        if (data.ContainsKey("Appearance"))
        {
            Appearance = Utility.CTString(data["Appearance"].ToString());
        }
        if (data.ContainsKey("ReParent"))
        {
            ReParent = Utility.CTBoolean(data["ReParent"].ToString());
        }
    }
    
}
