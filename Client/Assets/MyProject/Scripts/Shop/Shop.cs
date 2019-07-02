using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using proto.MyProto;
using System;

public class Shop : MUIBase {
    public enum GroupType
    {
        药材 = 90001,
        武器 = 90002,
        防御 = 90003,
        道具 = 90004
    }

    public GroupType currentSelectGroup = GroupType.药材;
    Transform content;
    public GameObject shopItem;
    List<GameObject> items = new List<GameObject>();
    public Button drug;
    public Button weapon;
    public Button defense;
    public Button prop;
    public Button exit;
    public override void OnAwake()
    {
        base.OnAwake();
        content = Utility.FindChild<Transform>(transform, "Content");
        drug = Utility.FindChild<Button>(transform, "Button_Drug");
        weapon = Utility.FindChild<Button>(transform, "Button_Weapon");
        defense = Utility.FindChild<Button>(transform, "Button_Defense");
        prop = Utility.FindChild<Button>(transform, "Button_Prop");
        exit = Utility.FindChild<Button>(transform, "Btn_Exit");
        drug.onClick.AddListener(() => {
            currentSelectGroup = GroupType.药材; Refresh();
        });
        weapon.onClick.AddListener(() =>
        {
            currentSelectGroup = GroupType.武器; Refresh();
        });
        defense.onClick.AddListener(() => {
            currentSelectGroup = GroupType.防御; Refresh();
        });
        prop.onClick.AddListener(() => {
            currentSelectGroup = GroupType.道具; Refresh();
        });
        exit.onClick.AddListener(() =>
        {
            Close();
        });

        LunaMessage.AddMsgHandler((int)MessageId.EBuyPropRes, OnBuyItem);
    }

    private void OnBuyItem(byte[] res)
    {
        BuyPropRes response = ProtoBufUtils.Deserialize<BuyPropRes>(res);
        PlayerData.BagInfos.Clear();
        foreach(var data in response.bagInfo){
            PlayerData.BagInfos.Add(data.id,data);
        }
        PlayerData.Gem = response.gem;
        PlayerData.Gold= response.gold;
        PlayerData.BloodStore = response.bloodStore;
    }

    /// <summary>
    /// 当前要现实的道具类型
    /// </summary>
    List<ShopConfig> props;
    public void Start()
    {
        Refresh();
    }

    public override void Open(params object[] parms)
    {
        base.Open(parms);
    }

    public override void Close(params object[] parms)
    {
        base.Close(parms);
    }
    /// <summary>
    /// 刷新商店道具UI的功能
    /// </summary>
    /// <param name="parms"></param>
    public override void Refresh(params object[] parms)
    {
        base.Refresh(parms);
        props = ConfigInfo.Instance.ShopConfigs[((int)currentSelectGroup).ToString()];
        //每次刷新UI之前把之前的UI清空掉
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i]);
        }
        items.Clear();

        for(int i =0; i < props.Count; i++)
        {
            //go每一个道具的UI预制体
            var go = ResourceMgr.CreateUIPrefab("GUIs/Shop/ShopItem",content);
            go.name = props[i].Id;
            items.Add(go);
            ShopConfig shopConfig = props[i];
            PropInfoConfig propConfig = ConfigInfo.Instance.Props[shopConfig.PropId];
            Text nameText = Utility.FindChild<Text>(go.transform,"Text_itemName");
            Image icon = Utility.FindChild<Image>(go.transform,"Icon");
            Text costText = Utility.FindChild<Text>(go.transform,"cost");
            nameText.text = propConfig.Name;
            icon.sprite = ResourceMgr.Load<Sprite>("Sprites/Props/" + propConfig.Icon);
            icon.gameObject.AddComponent<Button>();
            icon.gameObject.GetComponent<Button>().onClick.AddListener(() => 
            {
                BuyPropReq req = new BuyPropReq();
                req.id = PlayerData.Me.info.id;
                req.num = 1;
                req.storeId = Convert.ToInt32(go.name);
                MobaNetwork.Send((ushort)MessageId.EBuyPropReq, req);
            });
            Image coin = Utility.FindChild<Image>(go.transform, "coin");
            coin.sprite = ResourceMgr.LoadSpriteFromAtals(Utility.CTInt(shopConfig.Cost_Type),"Sprites/Coins");
            
            costText.text = shopConfig.Cost_Num;
        }
    }
}
