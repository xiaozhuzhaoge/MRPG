using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using proto.MyProto;
using System;
using UnityEngine.SceneManagement;

public class CharacterSelectSystem : MUIBase {

    Button addCharacter;
    Button enterGame;
    Button close;
    Transform layout;
    List<GameObject> caches = new List<GameObject>();
    

    public override void OnAwake()
    {
        base.OnAwake();
        addCharacter = transform.Find("Layout/Add").GetComponent<Button>();
        enterGame = transform.Find("EnterGame").GetComponent<Button>();
        close = transform.Find("Close").GetComponent<Button>();
        layout = transform.Find("Layout");
        addCharacter.onClick.AddListener(() => { MUIMgr.Instance.OpenUI("Systems/CharacterCreate"); });
        close.onClick.AddListener(Back);
        enterGame.onClick.AddListener(EnterGameReq);
    }


    public override void Open(params object[] parms)
    {
        base.Open(parms);
        Refresh();
    }

    public override void Refresh(params object[] parms)
    {
        base.Refresh(parms);
        RequestCharacterInfos();
    }

    private void Start()
    {
        LunaMessage.AddMsgHandler((int)MessageId.EGetPlayerInfosRes, OnGetPlayerInfosRes);
        LunaMessage.AddMsgHandler((int)MessageId.ESelectPlayerRes, OnPlayerSelect);

        MessageCenter.Instance.RegiseterMessage("OnCharacterCreate", gameObject, Do);
        MessageCenter.Instance.RegiseterMessage("RoleSelect", gameObject, OnRoleSelect);
    }
    private void EnterGameReq()
    {
        if(PlayerData.SelectPlayer == null)
        {
            MUIMgr.Instance.ShowAlert(OpenType.Long, null, null, "错误", "请选择角色或者创建角色再进入游戏", "确定");
            return;
        }
        SelectPlayerReq req = new SelectPlayerReq();
        req.gameServerIp = "";
        req.id = PlayerData.SelectPlayer.id;
        MobaNetwork.Send((ushort)MessageId.ESelectPlayerReq, req);
    }

    private void OnPlayerSelect(byte[] res)
    {
        SelectPlayerRes response = ProtoBufUtils.Deserialize<SelectPlayerRes>(res);
        if(response.code.status == 0)
        {
            PlayerData.WeaponConfigs.Clear();
            foreach(AttributeType data in Enum.GetValues(typeof(AttributeType)))
            {
                PlayerData.WeaponConfigs.Add(data, 0);
            }
            PlayerData.Me = response.roleInfo;
            PlayerData.State = response.stateInfo;
            PlayerData.LoginTime = response.time.time;
            PlayerData.Signs = response.infos;
            PlayerData.BloodStore = response.roleInfo.bloodStore;
            PlayerData.Gem = response.roleInfo.gem;
            PlayerData.Gold = response.roleInfo.gold;
            PlayerData.SetPropInfo(response.roleInfo.bagInfo);
            SceneMgr.Instance.LoadSceneAsync("Main");
        }
        else if(response.code.status == 1)
        {
            Debug.LogError("角色选择失败");
        }
    }

    private void OnRoleSelect(object[] parms)
    {
        PlayerData.SelectPlayer = parms[0] as PlayerInfo;
    }

    private void Do(object[] parms)
    {
        Refresh();
    }

    private void OnGetPlayerInfosRes(byte[] res)
    {
        ///清除上一次生成的游戏物体
        for(int i = 0; i < caches.Count; i++)
        {
            Destroy(caches[i]);
        }
        caches.Clear();

        ///基于反馈数据生成新的游戏物体
        GetPlayerInfosRes response = ProtoBufUtils.Deserialize<GetPlayerInfosRes>(res);
        int stat = response.code.status;
        if(stat == 0)
        {
            PlayerData.MyPlayers = response.infos;
            PlayerInfo temp;
            for(int i = 0; i < PlayerData.MyPlayers.Count; i++)
            {
                temp = PlayerData.MyPlayers[i];
                var go = GameObject.Instantiate<GameObject>(ResourceMgr.Load<GameObject>("GUIs/RolePlayer"));
                RolePlayer role = go.GetComponent<RolePlayer>();
                role.SetUI(temp);
                go.transform.SetParent(layout, false);
                caches.Add(go);
            }

            addCharacter.transform.SetAsLastSibling();
        }
        else
        {
            RequestCharacterInfos();
        }
    }

    /// <summary>
    /// 请求当前账户内的角色信息
    /// </summary>
    public void RequestCharacterInfos()
    {
        GetPlayerInfosReq req = new GetPlayerInfosReq();
        req.accountId = PlayerData.AccountId;
        MobaNetwork.Send((ushort)MessageId.EGetPlayerInfosReq, req);
    }
}
