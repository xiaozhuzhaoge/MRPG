using System;
using System.Collections.Generic;
using proto.MyProto;

public class MsgHandler
{
    
    public static void AddMsgHandler(MessageId msgid, Action<byte[], AsyncTcpClient> data)
    {
        NetMessage.AddMsgHandler((int)msgid, data);
    }

    public static void RegisterMsg()
    {
        AddMsgHandler(MessageId.ELoginReq, OnLogin);
        AddMsgHandler(MessageId.ERegisterReq, OnRegist);
        AddMsgHandler(MessageId.ECreatePlayerInfoReq, OnCreatePlayerInfo);
        AddMsgHandler(MessageId.EGetPlayerInfosReq, OnGetPlayerInfoReq);
        AddMsgHandler(MessageId.ESelectPlayerReq, OnSelectPlayer);
        AddMsgHandler(MessageId.EBagInfoEquipReq, OnBagInfoEquip);
        AddMsgHandler(MessageId.ESignInReq, OnSignIn);
        AddMsgHandler(MessageId.EBagInfoReq, GetBagInfo);
        AddMsgHandler(MessageId.EUsePropReq, OnUseProp);
        AddMsgHandler(MessageId.EEnchancePropReq, OnChangeprop); 
        AddMsgHandler(MessageId.EEnterDungeonsReq, OnEnterDungeons);
        AddMsgHandler(MessageId.EBuyPropReq, OnBuyProp);
        
    }

    private static void OnBuyProp(byte[] data, AsyncTcpClient user)
    {
        BuyPropReq req = ProtoBufUtils.Deserialize<BuyPropReq>(data);
        BuyPropRes res = new BuyPropRes();
        res.code = new StateCode();
        ShopConfig config = JsonHelper.GetShopProp(req.storeId.ToString());
        ///修改货币数量
        int myCoin = MobaDB.GetCoin(user._roleId, Convert.ToInt32(config.Cost_Type));

        RoleList role = MobaDB.GetRoleInfo(user._roleId);
        ///货币不够
        if(myCoin < Convert.ToInt32(config.Cost_Num))
        {
            res.code.status = 1;
            res.code.message = "你的货币不够,无法购买";
            List<proto.MyProto.PropInfo> list = MobaDB.GetRoleProp(req.id);
            foreach (proto.MyProto.PropInfo item in list)
            {
                res.bagInfo.Add(item);
            }
            res.gem = role.gem;
            res.gold = role.gold;
            res.bloodStore = role.bloodstore;
            MobaNetwork.Send(user, res, (ushort)MessageId.EBuyPropRes);
            return;
        }

        myCoin -= Convert.ToInt32(config.Cost_Num);
        MobaDB.ChangeCoin(user._roleId, Convert.ToInt32(config.Cost_Type), myCoin);
        role = MobaDB.GetRoleInfo(user._roleId);
        ///添加道具
        if (MobaDB.AddProp(req.id, Convert.ToInt32(config.PropId), req.num, 0))
        {
            res.code.status = 0;
            List<proto.MyProto.PropInfo> list = MobaDB.GetRoleProp(req.id);
            foreach (proto.MyProto.PropInfo item in list)
            {
                res.bagInfo.Add(item);
            }
            res.gem = role.gem;
            res.gold = role.gold;
            res.bloodStore = role.bloodstore;
        }
        else
        {
            res.code.status = 1;
            List<proto.MyProto.PropInfo> list = MobaDB.GetRoleProp(req.id);
            foreach (proto.MyProto.PropInfo item in list)
            {
                res.bagInfo.Add(item);
            }
            res.gem = role.gem;
            res.gold = role.gold;
            res.bloodStore = role.bloodstore;
        }
        MobaNetwork.Send(user, res, (ushort)MessageId.EBuyPropRes);
    }

    private static void OnEnterDungeons(byte[] data, AsyncTcpClient user)
    {
        EnterDungeonsReq req = ProtoBufUtils.Deserialize<EnterDungeonsReq>(data);
        
    }

    private static void OnChangeprop(byte[] data, AsyncTcpClient user)
    {
        EnchancePropReq req = ProtoBufUtils.Deserialize<EnchancePropReq>(data);
        
    }

    private static void OnUseProp(byte[] data, AsyncTcpClient user)
    {
        UsePropReq req = ProtoBufUtils.Deserialize<UsePropReq>(data);
        UsePropRes res = new UsePropRes();
        res.code = new StateCode();
        if (MobaDB.UseProp(user._accountId.ToString(), req.props))
        {
            res.code.status = 0;
            List<proto.MyProto.PropInfo> propInfos = MobaDB.GetRoleProp(user._accountId);
            foreach (proto.MyProto.PropInfo item in propInfos)
            {
                res.bagInfo.Add(item);
            }
        }
        else
        {
            res.code.status = 1;
         }
        MobaNetwork.Send(user, res, (ushort)MessageId.EUsePropRes);
    }

    /// <summary>
    /// 背包
    /// </summary>
    /// <param name="data"></param>
    /// <param name="user"></param>
    private static void GetBagInfo(byte[] data, AsyncTcpClient user)
    {
       BagInfoReq req= ProtoBufUtils.Deserialize<BagInfoReq>(data);
        List<proto.MyProto.PropInfo> list = MobaDB.GetRoleProp(req.playerId);
        BagInfoRes res = new BagInfoRes();
        res.code = new StateCode();
        if(list==null)
        {
            res.code.status = 1;
        }
        else
        {
            if(list.Count>0)
            {
                foreach (proto.MyProto.PropInfo item in list)
                {
                    res.bagInfo.Add(item);
                }
            }
            res.code.status = 0;
        }
        MobaNetwork.Send(user, res, (ushort)MessageId.EBagInfoRes);
    }

    /// <summary>
    /// 玩家签到
    /// </summary>
    /// <param name="data"></param>
    /// <param name="user"></param>
    private static void OnSignIn(byte[] data, AsyncTcpClient user)
    {
        SignInReq req = ProtoBufUtils.Deserialize<SignInReq>(data);
        SignInRes res = new SignInRes();
        res.code = new StateCode();
        res.serverTime = new TimeInfo();
        
        if (MobaDB.SetSginIn(user._roleId, req.signId, req.SignInTime))
        {
            res.serverTime.time = TimeHelper.GetMSTime();
            PropInfo p = JsonHelper.GetSiginJson(req.signId);
            proto.MyProto.PropInfo propInfo= new proto.MyProto.PropInfo();
            propInfo.id = p.id;
            propInfo.num = p.num;
            propInfo.propId = p.propId;
            propInfo.slot = p.slot;
            if (propInfo == null)
                return;
            int Id = MobaDB.SetSignPropToBag(user._roleId, p);
            if (Id == -1)
            {
                res.code.status = 1;
            }
            else
            {
                propInfo.id = Id;
                res.propInfo.Add(propInfo);
                res.code.status = 0;
            }
        }
        else
        {
            res.code.status = 1;
        }
        MobaNetwork.Send(user, res, (ushort)MessageId.ESignInRes);
    }

    /// <summary>
    /// 装备道具
    /// </summary>
    /// <param name="data"></param>
    /// <param name="user"></param>
    private static void OnBagInfoEquip(byte[] data, AsyncTcpClient user)
    {
        BagInfoEquipReq req = ProtoBufUtils.Deserialize<BagInfoEquipReq>(data);
        BagInfoEquipRes res = new BagInfoEquipRes();
        
        res.code = new StateCode();
         
        for (int i = 0; i < req.equipedProps.Count; i++)
        {
            EquipPropInfo equip = req.equipedProps[i];
            Console.WriteLine(req.playerId + " " + equip.id + " " + equip.slot);
            bool flag = MobaDB.UpdatePropSlot(req.playerId, equip.id, equip.slot);

            if (flag == false)
            {
                res.code.status = 1;
                MobaNetwork.Send(user, res, (ushort)MessageId.EBagInfoEquipRes);
                return;
            }
        }
        res.code.status = 0;
        List<proto.MyProto.PropInfo> list = MobaDB.GetRoleProp(req.playerId);
        foreach (proto.MyProto.PropInfo item in list)
        {
            res.propInfos.Add(item);
        }

        MobaNetwork.Send(user, res, (ushort)MessageId.EBagInfoEquipRes);

    }

    private static void OnServer(byte[] data, AsyncTcpClient user)
    {
        ServerRes res = new ServerRes();

    }

    /// <summary>
    /// 选择角色
    /// </summary>
    /// <param name="data"></param>
    /// <param name="user"></param>
    private static void OnSelectPlayer(byte[] data, AsyncTcpClient user)
    {
        SelectPlayerReq req = ProtoBufUtils.Deserialize<SelectPlayerReq>(data);
        SelectPlayerRes res = new SelectPlayerRes();
        res.roleInfo = new PlayerRoleInfo();
        res.stateInfo = new PlayerStateInfo();
        res.code = new StateCode();
        res.roleInfo.info = new PlayerInfo();
        res.time = new TimeInfo();
        List<object> list = MobaDB.SelectRole(req.id);
        if (list == null)
        {
            res.code.status = 1;
        }
        else
        {
            res.roleInfo.info.id = (list[0] as RoleList).userId;
            res.roleInfo.info.name= (list[0] as RoleList).name;
            res.roleInfo.info.type = (list[0] as RoleList).jobId;
            res.roleInfo.gem = (list[0] as RoleList).gem;
            res.roleInfo.bloodStore = (list[0] as RoleList).bloodstore;
            res.roleInfo.gold = (list[0] as RoleList).gold;

            List<proto.MyProto.PropInfo> rolePropList = MobaDB.GetRoleProp(req.id);
            if (rolePropList != null && rolePropList.Count >= 0)
            {
                foreach (proto.MyProto.PropInfo item in rolePropList)
                {
                    res.roleInfo.bagInfo.Add(item);
                }
            }
            res.stateInfo.level = (list[0] as RoleList).level;
            string[] str = (list[0] as RoleList).localtion.Split(new char[] { ',' });
            for (int i = 0; i < str.Length; i++)
            {
                res.stateInfo.localtion.Add(str[i]);
            }
            res.time.time = TimeHelper.GetMSTime();


            List<SignInInfo> infos = MobaDB.GetSignInInfo(req.id);
            foreach (SignInInfo item in infos)
            {
                res.infos.Add(item);
            }

            /////////////////////////签到记录返回

            res.code.status = 0;
            user._roleId = req.id;
        }
        MobaNetwork.Send(user, res, (ushort)MessageId.ESelectPlayerRes);
    }

    

    /// <summary>
    /// 获取玩家角色列表
    /// </summary>
    /// <param name="data"></param>
    /// <param name="user"></param>
    private static void OnGetPlayerInfoReq(byte[] data, AsyncTcpClient user)
    {
        GetPlayerInfosReq req = ProtoBufUtils.Deserialize<GetPlayerInfosReq>(data);
        GetPlayerInfosRes res = new GetPlayerInfosRes();
        res.code = new StateCode();
        foreach (PlayerInfo item in MobaDB.GetRoleList(req.accountId))
        {
            res.infos.Add(item);
        }
        res.code.status = 0;
        MobaNetwork.Send(user, res, (ushort)MessageId.EGetPlayerInfosRes);
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="data"></param>
    /// <param name="user"></param>
    private static void OnCreatePlayerInfo(byte[] data, AsyncTcpClient user)
    {
        CreatePlayerInfoReq req = ProtoBufUtils.Deserialize<CreatePlayerInfoReq>(data);
        CreatePlayerInfoRes res = new CreatePlayerInfoRes();
        res.code = new StateCode();
        if(MobaDB.CreateRole(req.accountId, req.name, req.type))
        {
            res.code.status = 0;
        }
        else
        {
            res.code.status = 1;
        }
        MobaNetwork.Send(user, res, (ushort)MessageId.ECreatePlayerInfoRes);
        
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="data"></param>
    /// <param name="user"></param>
    private static void OnRegist(byte[] data, AsyncTcpClient user)
    {
        RegisterReq req = ProtoBufUtils.Deserialize<RegisterReq>(data);
        RegisterRes res = new RegisterRes();
        res.code = new StateCode();
        DBAccount acc = MobaDB.ContainAccount(req.username);

        if (acc != null)
        {
            res.code.status = 1;
        }
        else
        {
            MobaDB.CreateAccount(req.username, req.password, req.deviceID, req.channel);
            res.code.status = 0;
        }
        MobaNetwork.Send(user, res, (ushort)MessageId.ERegisterRes);
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="data"></param>
    /// <param name="user"></param>
    private static void OnLogin(byte[] data, AsyncTcpClient user)
    {
        LoginReq req = ProtoBufUtils.Deserialize<LoginReq>(data);
        Console.WriteLine(req.username+"  "+req.password);
        LoginRes loginRes = new LoginRes();
        loginRes.code = new StateCode();
        // 从数据库中查询是否包含该账号
        DBAccount dbAcc = MobaDB.ContainAccount(req.username);
        if (dbAcc == null)
        {
            loginRes.accountId = -1;
            loginRes.code.status = 1;
        }
        else
        {
            if (dbAcc.username == req.username && dbAcc.password == req.password)
            {
                loginRes.accountId = dbAcc.accountId;
                loginRes.code.status = 0;
                user._roleId = dbAcc.accountId;
            }
        }

        // 从账号容器中查询是否包含该账号
        //Account acc = AccountManager.GetAccountByName(req.Login.AccountName);
        //if (acc != null)
        //{
        //    resp.ErrorCode = (uint)ErrorCode.AccountHasLogin;
        //}
        //else
        //{
        //    AccountManager.AccountLogin(user.uid, req.Login.AccountName, req.Login.Password);
        //    resp.Login.Account = req.Login.AccountName;
        //    resp.Login.Password = req.Login.Password;
        //}

        MobaNetwork.Send(user, loginRes,(ushort)MessageId.ELoginRes);
    }
}