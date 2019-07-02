using System;
using System.Collections.Generic;
using proto.MyProto;

public class DBAccount
{
    public int accountId;       //唯一ID
    public string username;     //用户名
    public string password;     //密码
    public string deviceID;     //设备ID
    public string channel;      //渠道
}

public class RoleList
{
    public int userId;          //角色ID
    public int accountId;       //账号ID
    public string name;         //昵称
    public int jobId;           //职业
    public int exp;             //经验值
    public int level;           //关卡ID
    public string localtion;    //玩家位置
    public string signIn;       //玩家签到信息
    public int gold;            //金子
    public int bloodstore;      //血石
    public int gem;             //宝石
}

public class PropInfo
{
    public int id;          //角色ID
    public int propId;
    public int num;
    public int slot;
}

public class MobaDB
{
    private static MysqlDB _mysql;

    private string _dbName = "Game";
    private string _dataSource = "127.0.0.1";
    private string _userid = "root";
    private string _pwd = "root";

    public void Init()
    {
        _mysql = new MysqlDB();
        _mysql.Init(_dbName, _dataSource, _userid, _pwd);
        MobaLog.Log("Mysql DB Init...");
    }

    /// <summary>
    ///  从数据库中查询账号名为accountID的账号信息
    /// </summary>
    /// <param name="accountID"></param>
    /// <returns></returns>
    public static DBAccount ContainAccount(string userName)
    {
        string sql = "select * from userMsgTable";
        List<object> objList = _mysql.ExecQuery<DBAccount>(sql);
        for (int i = 0; i < objList.Count; i++)
        {
            DBAccount acc = objList[i] as DBAccount;
            if (acc.username == userName)
            {
                return acc;
            }
        }
        return null;
    }

    public static int GetPropNum(int roleId,int instanceId)
    {
        List<object> props = GetPropbyInstanceId(roleId, instanceId);
        if (props.Count > 0)
        {
            return (props[0] as PropInfo).num;
        }
        else
            return 0;
    }

    public static bool AddProp(int roleId,int propId,int num,int slot)
    {
        List<object> props = GetProps(roleId,propId);
        if (props.Count == 0)
        {
            string sql = string.Format("insert into {0}_prop (propId,num,slot) values('{1}','{2}','{3}')", roleId, propId, num, slot);
            try
            {
                _mysql.Insert(sql);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        else
        {
            for(int i = 0; i < props.Count; i++)
            {
                PropInfo temp = props[i] as PropInfo;
                if(temp != null)
                {
                    PropConfig info =  JsonHelper.GetProp(temp.propId.ToString());
                    if (info.CanCombine.Equals("0"))//不能合并
                    {
                        string sql = string.Format("insert into {0}_prop (propId,num,slot) values('{1}','{2}','{3}')", roleId, propId, num, slot);
                        try
                        {
                            _mysql.Insert(sql);
                            return true;
                        }
                        catch (Exception e)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        int currentNum = GetPropNum(roleId,temp.id);//可以合并
                        string sql = string.Format("update {0} set num='{1}' where id='{2}'", (roleId + "_prop"), currentNum += num , temp.id);
                        try
                        {
                            _mysql.Insert(sql);
                            return true;
                        }
                        catch (Exception e)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 获取指定道具id的道具实例
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="propId"></param>
    /// <returns></returns>
    public static List<object> GetProps(int roleId,int propId)
    {
        string sql = string.Format("select * from {0}_prop where propId = {1}", roleId, propId);
        List<object> props = _mysql.ExecQuery<PropInfo>(sql);
        return props;
    }

    public static List<object> GetPropbyInstanceId(int roleId,int instanceId)
    {
        string sql = string.Format("select * from {0}_prop where id = {1}", roleId, instanceId);
        List<object> props = _mysql.ExecQuery<PropInfo>(sql);
        return props;
    }

    public static bool UpdatePropSlot(int roleId,int propInstanceId,int slot)
    {
        Console.WriteLine(roleId + " "+ propInstanceId  + "" + slot);
        string sql = string.Format("update  {0}_prop set slot = {1} where id = {2}", roleId, slot, propInstanceId);
        Console.WriteLine(sql);
        try
        {
            _mysql.ExecNonQuery(sql);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }

    }

    public static bool UseProp(string accountId, List<UserPropInfo> propInfos)
    {
        string sql = string.Format("select * from {0}", (accountId + "_prop"));
        List<object> infos = _mysql.ExecQuery<PropInfo>(sql);


        if (infos.Count > 0)
        {
            Dictionary<int, PropInfo> dic = new Dictionary<int, PropInfo>();
            foreach (PropInfo item in infos)
            {
                dic.Add(item.id, item);
            }
            foreach (UserPropInfo item in propInfos)
            {
                if (dic.ContainsKey(item.id))
                {
                    if(dic[item.id].num>=item.num)
                    {
                        
                        sql = string.Format("update {0} set num='{0}' where id='{1}'", (accountId + "_prop"), (dic[item.id].num - item.num), item.id);
                        _mysql.ExecNonQuery(sql);
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }


   public static List<SignInInfo> GetSignInInfo(int userId)
    {
        List<SignInInfo> signIns = new List<SignInInfo>();
        string sql = string.Format("select* from role where userId='{0}'", userId);
        List<object> roles= _mysql.ExecQuery<RoleList>(sql);
        if (roles.Count > 0)
        {
            SignInInfo s = new SignInInfo();
            s.time = new TimeInfo();
            string signin = (roles[0] as RoleList).signIn;
            string[] strs = signin.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in strs)
            {
                s.signId = Convert.ToInt32(item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                s.time.time = Convert.ToInt64(item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                signIns.Add(s);
            }
        }
        return signIns;
    }


    /// <summary>
    /// 获取角色列表信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static List<PlayerInfo> GetRoleList(int userId)
    {
        string sql = "select* from role";
        List<PlayerInfo> playerInfos = new List<PlayerInfo>();
        List<object> objList = _mysql.ExecQuery<RoleList>(sql);
        for (int i = 0; i < objList.Count; i++)
        {
            RoleList acc = objList[i] as RoleList;
            if (acc.accountId == userId)
            {
                PlayerInfo playerInfo = new PlayerInfo();
                playerInfo.id = acc.userId;
                playerInfo.name = acc.name;
                playerInfo.type = acc.jobId;
                
                playerInfos.Add(playerInfo);
            }
        }
        return playerInfos;
    }
    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="accountId">账户ID</param>
    /// <param name="name">玩家昵称</param>
    /// <param name="job">职业</param>
    /// <returns></returns>
    public static bool CreateRole(string accountId, string name, int job)
    {
        List<object> roleLists = GetRoleID(name);
        if (roleLists.Count == 0)
        {
            string sql = string.Format("insert into role (accountId,name,JobId,exp) values('{0}','{1}','{2}',0)", accountId, name, job);
            try
            {
                _mysql.Insert(sql);
                roleLists = GetRoleID(name);
                if (roleLists.Count == 0)
                {
                    return false;
                }
                else
                {
                    CreateRoleProp((roleLists[0] as RoleList).userId);
                    return true;
                }

            }
            catch (Exception e)
            {
                return false;
            }
        }
        else
        {
            return false;
        }


    }

    /// <summary>
    /// 用户签到
    /// </summary>
    public static bool SetSginIn(int accountId,int signId,TimeInfo time)
    {
        try
        {
            string sql = string.Format("select * from role where userId='{0}'", accountId);
            List<object> list = _mysql.ExecQuery<RoleList>(sql);
            string signIn = null;
            if (list.Count > 0)
            {
                signIn = (list[0] as RoleList).signIn;
                signIn += string.Format("|{0} {1}", signId, time.time);
            }
            sql = string.Format("update role set signIn='{0}' where userId='{1}'", signIn, accountId);
            _mysql.ExecNonQuery(sql);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }


    public static bool GetSignPropFromBag(int accountId,PropInfo propInfo)
    {
        string sql = string.Format("select * from {0} where propId='{1}'", (accountId + "_prop"), propInfo.propId);
        try
        {
            List<object> list = _mysql.ExecQuery<PropInfo>(sql);
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }


    public static int SetSignPropToBag(int accountId, PropInfo propInfo)
    {
        int nums = -1;
        string sql = string.Format("select * from {0} where propId='{1}'", (accountId + "_prop"), propInfo.propId);
        List<object> list = _mysql.ExecQuery<PropInfo>(sql);
        if (list.Count > 0)
        {
            sql = string.Format("update {0} set num='{0}' where propId='{1}'", (accountId + "_prop"), (propInfo.num + (list[0] as PropInfo).num), propInfo.propId);
            nums = (list[0] as PropInfo).id;
            _mysql.ExecNonQuery(sql);
        }
        else
        {
            sql = string.Format("insert into {0} ( propId, num, slot) values('{1}', '{2}' , '{3}')", (accountId + "_prop"), propInfo.propId, propInfo.num, propInfo.slot);
            nums = propInfo.num;
            _mysql.Insert(sql);

            sql = string.Format("select * from {0} where propId='{1}'", (accountId + "_prop"), propInfo.propId);
            list = _mysql.ExecQuery<PropInfo>(sql);
            nums = (list[0] as PropInfo).id;
        }

        return nums;
    }



    public static List<object> GetRoleID(string name)
    {
        string sql = string.Format("select * from role where name='{0}'", name);
        List<object> roleLists = _mysql.ExecQuery<RoleList>(sql);
        return roleLists;
    }


    /// <summary>
    /// 往数据库的Account表中插入一条记录
    /// </summary>
    /// <param name="accountName"></param>
    /// <param name="password"></param>
    public static void CreateAccount(string userName, string password, string deviceID, string channel)
    {
        string sql = string.Format("insert into userMsgTable ( username, password, deviceID, channel) values('{0}','{1}', '{2}' , '{3}')", userName, password, deviceID, channel);
        _mysql.Insert(sql);


    }

    public static int GetUserID(string userName)
    {
        DBAccount db = ContainAccount(userName);
        return db.accountId;
    }

    /// <summary>
    /// 创建角色物品表
    /// </summary>
    /// <param name="userID"></param>
    public static void CreateRoleProp(int userID)
    {
        string sql = string.Format("create table " + userID + "_prop" + " like test_prop");
        _mysql.ExecNonQuery(sql);
    }

    /// <summary>
    /// 获取角色背包信息
    /// </summary>
    /// <param name="userID">角色ID</param>
    /// <returns></returns>
    public static List<proto.MyProto.PropInfo> GetRoleProp(int userID)
    {
        string tableName = userID + "_prop";
        string sql = string.Format("select * from {0}", tableName);
        List<object> list = ExecQuery<PropInfo>(sql);
        List<proto.MyProto.PropInfo> p = new List<proto.MyProto.PropInfo>();
        foreach (PropInfo item in list)
        {
            proto.MyProto.PropInfo prop = new proto.MyProto.PropInfo();
            prop.id = item.id;
            prop.num = item.num;
            prop.slot = item.slot;
            prop.propId = item.propId;
            p.Add(prop);
        }

        return p;
    }

    /// <summary>
    /// 选择角色
    /// </summary>
    /// <param name="accountId"></param>
    public static List<object> SelectRole(int roleId)
    {
        string sql = string.Format("select * from role where userId='{0}'", roleId);
        List<object> list = ExecQuery<RoleList>(sql);
        if (list.Count == 1)
        {
            return list;
        }
        else
        {
            return null;
        }

    }



    public static List<object> ExecQuery<T>(string sql)
    {
        return _mysql.ExecQuery<T>(sql);
    }
}