using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.MyProto;
using System;
using UnityEngine.UI;

public class BagSystem : MUIBase {

    public int num;
    public Transform layout;
    public List<GameObject> blocks = new List<GameObject>();
    public List<BagProp> props = new List<BagProp>();
    public Button close;

    public override void OnAwake()
    {
        base.OnAwake();
        layout = Utility.FindChild<Transform>(transform,"Content");
        CreateBlocks();
        LunaMessage.AddMsgHandler((int)MessageId.EBagInfoRes, OnBagInfo);
        close = Utility.FindChild<Button>(transform, "Close");
        close.onClick.AddListener(Back);
        MessageCenter.Instance.RegiseterMessage("ItemExChange", gameObject, Refresh);
    }

    private void OnBagInfo(byte[] res)
    {
        BagInfoRes response = ProtoBufUtils.Deserialize<BagInfoRes>(res);
        PlayerData.SetPropInfo(response.bagInfo);
        Debug.Log(response.bagInfo.Count);
        Refresh();
    }

    public override void Open(params object[] parms)
    {
        base.Open(parms);
        BagInfoReq req = new BagInfoReq();
        req.playerId = PlayerData.Me.info.id;
        MobaNetwork.Send((ushort)MessageId.EBagInfoReq, req);
    }


    public override void Refresh(params object[] parms)
    {
        base.Refresh(parms);
        RemoveAllItem();
        ShowItem();
    }

    void CreateBlocks()
    {
        for(int i = 0; i < num; i++)
        {
            var block = ResourceMgr.CreateObj("GUIs/Bag/Block");
            block.transform.SetParent(layout, false);
            block.name = i.ToString();
            blocks.Add(block);
        }
    }

    void RemoveAllItem()
    {
        for(int i = 0; i < props.Count; i++)
        {
            Debug.Log("???");
            Destroy(props[i].gameObject);
        }
        props.Clear();
    }

    void ShowItem()
    {
        if (PlayerData.BagInfos == null)
            return;
        int count = 0;
        foreach (var item in PlayerData.BagInfos)
        {
            
            if(item.Value.slot < 1201 && item.Value.num > 0)
            {
                var prop = ResourceMgr.CreateObj("GUIs/Bag/Prop");
                BagProp bp = prop.GetComponent<BagProp>();
                bp.SetPropInfo(item.Value);
                prop.transform.SetParent(blocks[count].transform, false);
                props.Add(bp);
                bp.gameObject.name = item.Value.id.ToString();
                count++;
            }
           
        }
 
    }

}
