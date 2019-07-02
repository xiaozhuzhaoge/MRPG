using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.MyProto;
using System;

public class PlayerData
{

    private static int accountId;

    public static int AccountId
    {
        get
        {
            return accountId;
        }

        set
        {
            accountId = value;
        }
    }

    public static List<PlayerInfo> MyPlayers
    {
        get
        {
            return myPlayers;
        }

        set
        {
            myPlayers = value;
        }
    }

    public static PlayerInfo SelectPlayer
    {
        get
        {
            return selectPlayer;
        }

        set
        {
            selectPlayer = value;
        }
    }

    public static PlayerRoleInfo Me
    {
        get
        {
            return me;
        }

        set
        {
            me = value;
            SetPropInfo(me.bagInfo);
        }
    }

    /// <summary>
    /// 背包变动委托
    /// </summary>
    public static Action<List<PropInfo>> OnBagChange;

    /// <summary>
    /// 设置道具
    /// </summary>
    /// <param name="props"></param>
    public static void SetPropInfo(List<PropInfo> props)
    {
        BagInfos.Clear();
        props.ForEach((item) => {
            if (BagInfos == null)
                BagInfos = new Dictionary<int, PropInfo>();
            if (BagInfos.ContainsKey(item.id))
                BagInfos[item.id] = item;
            else
                BagInfos.Add(item.id, item);
        });
        if(OnBagChange != null)
        {
            OnBagChange(props);
        }
 
        WeaponConfigs.Clear();
        ReCaculateWeaponInfo(BagInfos);
        if(myRole != null)
        myRole.ResetAllAttributes();
    }

    /// <summary>
    /// 重新计算装备加成
    /// </summary>
    /// <param name="props"></param>
    public static void ReCaculateWeaponInfo(Dictionary<int,PropInfo> props)
    {
        foreach (var data in props)
        {
            Debug.Log(data.Value.propId.ToString());
            PropInfoConfig config = ConfigInfo.GetProp(data.Value.propId.ToString());
            if(data.Value.slot >= 1200 && data.Value.slot < 1208)
            {
                foreach (var skilldata in config.Skills)
                {
                    if (!WeaponConfigs.ContainsKey(skilldata.Key))
                        WeaponConfigs.Add(skilldata.Key, skilldata.Value);

                    WeaponConfigs[skilldata.Key] += skilldata.Value;
                }
            }
           
        }
    }
    /// <summary>
    /// 装备添加的属性
    /// </summary>
    static Dictionary<AttributeType, float> weaponConfigs = new Dictionary<AttributeType, float>();

    public static Dictionary<AttributeType, float> WeaponConfigs
    {
        get
        {
            return weaponConfigs;
        }

        set
        {
            weaponConfigs = value;
        }
    }
    /// <summary>
    /// 背包信息
    /// </summary>
    private static Dictionary<int, PropInfo> bagInfos = new Dictionary<int, PropInfo>();

    public static Dictionary<int, PropInfo> BagInfos
    {
        get
        {
            return bagInfos;
        }

        set
        {
            bagInfos = value;
        }
    }


    public static PlayerStateInfo State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
        }
    }

    public static long LoginTime
    {
        get
        {
            return loginTime;
        }

        set
        {
            loginTime = value;
            TimeSystem.ServerTime = value;

        }
    }

    public static List<SignInInfo> Signs
    {
        get
        {
            return signs;
        }

        set
        {

            signs = value;

        }
    }

    public static Action<int, int> OnGoldChanged;

    public static int Gold
    {
        get
        {
            return gold;
        }

        set
        {

            if (OnGoldChanged != null)
                OnGoldChanged(gold, value);

            gold = value;
        }
    }

    public static Action<int, int> OnGemChanged;

    public static int Gem
    {
        get
        {
            return gem;
        }

        set
        {

            if (OnGemChanged != null)
                OnGemChanged(gem, value);

            gem = value;
        }
    }

    public static Action<int, int> OnBloodStoreChanged;

    public static int BloodStore
    {
        get
        {
            return bloodStore;
        }

        set
        {
            if (OnBloodStoreChanged != null)
                OnBloodStoreChanged(bloodStore, value);
            bloodStore = value;
        }
    }


    private static List<PlayerInfo> myPlayers;

    private static PlayerInfo selectPlayer;

    private static PlayerRoleInfo me;

    private static PlayerStateInfo state;

    private static long loginTime;

    private static List<SignInInfo> signs;

    public static CharacterInfo myRole;
    public static Dictionary<int, CharacterInfo> enemys = new Dictionary<int, CharacterInfo>();
    public static Dictionary<int, CharacterInfo> npcs = new Dictionary<int, CharacterInfo>();


    public static Action<CharacterInfo> OnRoleChange;
    public static Action<CharacterInfo> OnNpcChange;
    public static Action<CharacterInfo> OnEnemyChange;

    public static void AddRole(CharacterInfo role)
    {
        myRole = role;
        if (OnRoleChange != null)
            OnRoleChange(role);
    }
    public static void AddEnemy(CharacterInfo role)
    {
        enemys.Add(role.gameObject.GetInstanceID(), role);
        if (OnEnemyChange != null)
            OnEnemyChange(role);
    }
    public static void AddNpc(CharacterInfo role)
    {
        npcs.Add(role.gameObject.GetInstanceID(), role);
        if (OnNpcChange != null)
            OnNpcChange(role);
    }

    public static void RemoveRole(CharacterInfo role)
    {
        myRole = null;
        if (OnRoleChange != null)
            OnRoleChange(role);
    }

    public static void RemoveEnemy(CharacterInfo role)
    {
        enemys.Remove(role.gameObject.GetInstanceID());
        if (OnEnemyChange != null)
            OnEnemyChange(role);
    }

    public static void RemoveNpc(CharacterInfo role)
    {
        npcs.Remove(role.gameObject.GetInstanceID());
        if (OnNpcChange != null)
            OnNpcChange(role);
    }

    private static int gold;
    private static int gem;
    private static int bloodStore;
}
