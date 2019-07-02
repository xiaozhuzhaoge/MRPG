using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ConfigInfo拆分类中的商店配置信息部分
/// </summary>
public partial class ConfigInfo {
    //存储所有道具信息的字典
    private Dictionary<string, ShopConfig> shops = new Dictionary<string, ShopConfig>();
    //将道具信息通过GroupId进行分组的字典
    public Dictionary<string, List<ShopConfig>> ShopConfigs = new Dictionary<string, List<ShopConfig>>();

    public Dictionary<string, ShopConfig> Shops
    {
        get
        {
            return shops;
        }

        set
        {
            shops = value;

            foreach(var data in shops)
            {
                if (!ShopConfigs.ContainsKey(data.Value.Group_Id))
                {
                    ShopConfigs.Add(data.Value.Group_Id, new List<ShopConfig>());
                }
                ShopConfigs[data.Value.Group_Id].Add(data.Value);
            }
        }
    }
}
