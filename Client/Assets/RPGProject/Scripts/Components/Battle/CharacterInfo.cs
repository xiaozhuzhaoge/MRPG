using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    Role,
    Npc,
    Enemy
}

public class CharacterInfo : MonoBehaviour {

    public delegate void OnValueChange(float lastHp,float currentHp,float maxHp);
    public OnValueChange OnHpChange;
    public OnValueChange OnMpChange;
    public OnValueChange OnDead;

   
    [SerializeField]
    public float MaxHp
    {
        get {  return CaculateValue(AttributeType.MaxHp); }
    }

    public float MaxMp
    {
        get { return CaculateValue(AttributeType.MaxMp); }
    }

    [SerializeField]
    public float Atk
    {
        get { return CaculateValue(AttributeType.Atk); }
    }
    [SerializeField]
    public float Def
    {
        get { return CaculateValue(AttributeType.Def); }
    }
    [SerializeField]
    public float Cri
    {
        get { return CaculateValue(AttributeType.Cri) / 100f; }
    }
    [SerializeField]
    public float Adv
    {
        get { return CaculateValue(AttributeType.Adv) / 100f; }
    }
    [SerializeField]
    public float MoveSpeed
    {
        get { return CaculateValue(AttributeType.MoveSpeed); }
    }
    [SerializeField]
    public float AtkSpeed
    {
        get { return CaculateValue(AttributeType.AtkSpeed); }
    }

    float currentHp;
    /// <summary>
    /// 当前血量
    /// </summary>
    public float CurrentHp
    {
        get
        {
            return currentHp;
        }

        set
        {
            if (value <= 0)
                value = 0;

            if (value <= 0)
            {
                if (OnDead != null)
                    OnDead(currentHp, value, MaxHp);
            }

            if (currentHp != value)
            {
                if(OnHpChange != null)
                OnHpChange(currentHp, value, MaxHp);

            }
            currentHp = value;
           
        }
    }

    /// <summary>
    /// 当前蓝量
    /// </summary>
    public float CurrentMp
    {
        get
        {
            return currentMp;
        }

        set
        {
            if(OnMpChange != null)
               OnMpChange(currentMp, value, MaxMp);
            currentMp = value;
            if (currentMp <= 0)
                currentMp = 0;
        }
    }

    public PlayerConfig Config
    {
        get
        {
            if(config == null)
            {
                config = ConfigInfo.Instance.PlayerDir[playerConfigId];
            }
            return config;
        }

        set
        {
            config = value;
        }
    }

    float currentMp;

    /// <summary>
    /// 角色配置ID
    /// </summary>
    public string playerConfigId;
    private PlayerConfig config;
    [HideInInspector]
    public Character ccb;
    public PlayerType characterType;
    [HideInInspector]
    public TargetHPHud HPHUD;
  

    public void Awake()
    {
        ccb = GetComponent<Character>();
        if (characterType == PlayerType.Role)
            PlayerData.AddRole(this);
        if (characterType == PlayerType.Enemy)
            PlayerData.AddEnemy(this);
        if (characterType == PlayerType.Npc)
            PlayerData.AddNpc(this);

        //HPHUD = ResourceMgr.CreateUIPrefab("GUIs/HpHUD", MUIMgr.Instance.Canvas).GetComponent<TargetHPHud>();
        //HPHUD.target = this;

        //OnHpChange += (lastHp, currentHp, MaxHp) =>
        //{
        //    PopHpCost pop = ResourceMgr.CreateUIPrefab("GUIs/PopHpCost", MUIMgr.Instance.Canvas).GetComponent<PopHpCost>();
        //    pop.SetValue(lastHp - currentHp, this, ccb.IsCritical);
        //};
    }

    public void Start()
    {
        ///读取角色配置信息
        Config = ConfigInfo.Instance.PlayerDir[playerConfigId];
        ResetAllAttributes();
        CurrentHp = MaxHp;
        CurrentMp = MaxMp;
      
    }

    public void ResetAllAttributes()
    {
        ccb.SetAtkSpeedValue(AtkSpeed);
        ccb.SetMoveSpeedValue(MoveSpeed);
        MaxHp.ToString();
        MaxMp.ToString();
        Def.ToString();
        Cri.ToString();
        AtkSpeed.ToString();
        Atk.ToString();
        Adv.ToString();
        MoveSpeed.ToString();

    }

    public float CaculateValue(AttributeType attribute)
    {
        if (!PlayerData.WeaponConfigs.ContainsKey(attribute))
            PlayerData.WeaponConfigs.Add(attribute, 0);
        switch (attribute)
        {
            case AttributeType.MaxHp:
                if (characterType == PlayerType.Role)
                    return (float)Config.Hp + Config.Skills[attribute] + PlayerData.WeaponConfigs[attribute];
           
                else
                    return (float)Config.Hp + Config.Skills[attribute];

            case AttributeType.MaxMp:
                if (characterType == PlayerType.Role)
                    return Config.Skills[attribute];
                else
                    return Config.Skills[attribute];

            case AttributeType.Def:
                if (characterType == PlayerType.Role)
                    return (float)Config.Def + (float)Config.Def * Config.Skills[attribute] + PlayerData.WeaponConfigs[attribute];
                else
                    return (float)Config.Def + (float)Config.Def * Config.Skills[attribute];
            case AttributeType.Cri:
                if (characterType == PlayerType.Role)
                    return (float)Config.Cri + Config.Skills[attribute] + PlayerData.WeaponConfigs[attribute];
                else
                    return (float)Config.Cri + Config.Skills[attribute];
            case AttributeType.AtkSpeed:
                if (characterType == PlayerType.Role)
                    return (float)Config.AtkSpeed + Config.Skills[attribute] + PlayerData.WeaponConfigs[attribute];
                else
                    return (float)Config.AtkSpeed + Config.Skills[attribute];
            case AttributeType.Atk:
                if (characterType == PlayerType.Role)
                    return (float)Config.Atk + Config.Skills[attribute] + PlayerData.WeaponConfigs[attribute];
                else
                    return (float)Config.Atk + Config.Skills[attribute];
            case AttributeType.Adv:
                if (characterType == PlayerType.Role)
                    return (float)Config.Adv + Config.Skills[attribute] + PlayerData.WeaponConfigs[attribute];
                else
                    return (float)Config.Adv + Config.Skills[attribute];
            case AttributeType.MoveSpeed:
                if (characterType == PlayerType.Role)
                    return (float)Config.MoveSpeed + Config.Skills[attribute] + PlayerData.WeaponConfigs[attribute];
                else
                    return (float)Config.MoveSpeed + Config.Skills[attribute];
        }
        return 0;
    }

    public void OnDestroy()
    {
        if (characterType == PlayerType.Role)
            PlayerData.RemoveRole(this);
        if (characterType == PlayerType.Enemy)
            PlayerData.RemoveEnemy(this);
        if (characterType == PlayerType.Npc)
            PlayerData.RemoveNpc(this);
    }
}
