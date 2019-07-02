using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using LitJson;

[XLua.LuaCallCSharp]
public class LuaConfigInfo : Singleton<LuaConfigInfo> {

    public void LoadLuas()
    {
        JsonData jsonArray = JsonMapper.ToObject(ResourceMgr.Load<TextAsset>("LuaConfigs/LuaConfigs").text) ;
        foreach(JsonData config in jsonArray)
        {
            LoadLoadTable(config["name"].ToString());
        }
    }

	public void LoadLoadTable(string luaTableName)
    {
        LuaSumlator.MyLuaEnv.DoString(ResourceMgr.Load<TextAsset>("LuaConfigs/" +luaTableName).text,"LuaConfigInfo", LuaSumlator.MyLuaEnv.Global);
    }
}
