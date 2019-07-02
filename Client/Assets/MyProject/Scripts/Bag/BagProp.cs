using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using proto.MyProto;
using UnityEngine.EventSystems;
using System;

public class BagProp : PropItem,IBeginDragHandler,IEndDragHandler,IDragHandler {

    public Image icon;
    public Text num;
    public PropInfo info;
    public Transform father;
    
    public void Awake()
    {
        icon = transform.GetComponent<Image>();
        num = Utility.FindChild<Text>(transform,"num");
        LunaMessage.AddMsgHandler((int)MessageId.EBagInfoEquipRes, OnEquipRes);
    }

    private void OnEquipRes(byte[] res)
    {
       BagInfoEquipRes response = ProtoBufUtils.Deserialize<BagInfoEquipRes>(res);
        PlayerData.SetPropInfo(response.propInfos);
        Debug.LogError("收到装备回调");
        MessageCenter.Instance.BoradCastMessage("ItemExChange");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        father = transform.parent;
        icon.raycastTarget = false;
        transform.SetParent(MUIMgr.Instance.Canvas);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
        int instanceId = Convert.ToInt32(gameObject.name);
        int propId = PlayerData.BagInfos[instanceId].propId;
        Debug.LogError(hitObject);

        if (hitObject == null)
        {
            ResetParent(father.gameObject);
        }
        else
        {
            BagBlock block = hitObject.GetComponent<BagBlock>();
         
            if (block != null)
            {
                if(block.type == BagBlock.BlockType.CharacterBag)
                {
                    if (hitObject.name == ConfigInfo.GetProp(info.propId.ToString()).Type)
                    {
                        ResetParent(hitObject, true);
                        SendPropEquipInfo(instanceId, propId, Convert.ToInt32(block.name));
                    }
                    else
                    {
                        ResetParent(father.gameObject);
                    }
                }
                else if(block.type == BagBlock.BlockType.Bag)
                {
                    ResetParent(hitObject,true);
                    SendPropEquipInfo(instanceId, propId, Convert.ToInt32(block.name));
                }
                else
                {
                    ResetParent(father.gameObject);
                }
            }
            else
            {
                BagProp bp = hitObject.GetComponent<BagProp>();
                if(bp != null)
                { ///替换装备
                    if (bp.transform.parent.GetComponent<BagBlock>().type == BagBlock.BlockType.CharacterBag)
                    {
                        ResetParent(hitObject.transform.parent.gameObject, true);
                        BagInfoEquipReq request = new BagInfoEquipReq();

                        PropInfo swapedInfo = PlayerData.BagInfos[Convert.ToInt32(hitObject.name)];
                        PropInfo dragInfo = PlayerData.BagInfos[Convert.ToInt32(gameObject.name)];

                        EquipPropInfo reprop = new EquipPropInfo();
                        reprop.id = swapedInfo.id;
                        reprop.propId = swapedInfo.propId;


                        EquipPropInfo prop = new EquipPropInfo();
                        prop.id = dragInfo.id;
                        prop.propId = dragInfo.propId;

                        reprop.slot = dragInfo.slot;
                        int swapSlot = swapedInfo.slot;
                        prop.slot = swapSlot;


                        request.playerId = PlayerData.Me.info.id;
                        request.equipedProps.Add(reprop);
                        request.equipedProps.Add(prop);

                        MobaNetwork.Send((ushort)MessageId.EBagInfoEquipReq, request);
                    }
                    else
                    {
                        ResetParent(father.gameObject);
                    }
                }
                else
                {
                    ResetParent(father.gameObject);
                } 
            }
        }
        icon.raycastTarget = true;
    }

    /// <summary>
    /// 设置道具信息
    /// </summary>
    /// <param name="info"></param>
    public void SetPropInfo(PropInfo info)
    {
        this.info = info;
        SetView();
    }

    public void SetView()
    {
        propInfo = ConfigInfo.GetProp(info.propId.ToString());
        icon.sprite = ResourceMgr.Load<Sprite>("Sprites/Props/" + propInfo.Icon);
        num.text = "x"+info.num.ToString();
    }
 
    /// <summary>
    /// 重置父类对象
    /// </summary>
    /// <param name="go"></param>
    /// <param name="setFatherValue"></param>
    public void ResetParent(GameObject go,bool setFatherValue = false)
    {
        transform.SetParent(go.transform);
        transform.localPosition = Vector3.one;
        if(father == true)
            father = transform.parent;
    }

    /// <summary>
    /// 发送道具装备数据
    /// </summary>
    /// <param name="instanceId"></param>
    /// <param name="propId"></param>
    /// <param name="slot"></param>
    public void SendPropEquipInfo(int instanceId,int propId,int slot)
    {
        BagInfoEquipReq request = new BagInfoEquipReq();
        EquipPropInfo prop = new EquipPropInfo();
        prop.id = instanceId;
        prop.propId = propId;
        prop.slot = slot;
        request.equipedProps.Add(prop);
        request.playerId = PlayerData.Me.info.id;
        Debug.Log("SendPropEquipInfo" + prop.propId + "  " + prop.slot);
        MobaNetwork.Send((ushort)MessageId.EBagInfoEquipReq, request);
    }
}
