using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

public class C_Lua_Bridge  {

    private Action luaEnter;
    private Action luaUpdate;
    private Action luaExit;
    private Action luaLateUpdate;

    public GameObject gameObject;
    public string Prefix;
    private LuaTable scriptEnv;
    /// <summary>
    /// 唯一识别ID
    /// </summary>
    public string BirdageId;
    public C_Lua_Bridge(string id,GameObject gameObject,string prefix) {

        this.Prefix = prefix;
        this.BirdageId = id;
        this.gameObject = gameObject;
        scriptEnv = LuaSumlator.MyLuaEnv.NewTable();//创建一个LUA表
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = LuaSumlator.MyLuaEnv.NewTable();//创一个元表
        meta.Set("__index", LuaSumlator.MyLuaEnv.Global);//__index = Gloabl
        scriptEnv.SetMetaTable(meta);//setmetatable
        meta.Dispose();

        scriptEnv.Set("self", this);
        TextAsset asset = ResourceMgr.Load<TextAsset>( "Luas/" + prefix + BirdageId + ".lua");
        if (asset == null)
            return;

        LuaSumlator.MyLuaEnv.DoString(asset.text,"报错来自: " + gameObject.name + "报错信息来自脚本: " + BirdageId, scriptEnv);
        //LuaSumlator.Instance.LoadScript(prefix + BirdageId, scriptEnv);

        Action luaAwake = scriptEnv.Get<Action>("Awake");
        scriptEnv.Get("OnEnter", out luaEnter);
        scriptEnv.Get("OnUpdate", out luaUpdate);
        scriptEnv.Get("OnExit", out luaExit);
        scriptEnv.Get("OnLateUpdate", out luaLateUpdate);

        if (luaAwake != null)
        {
            luaAwake();
        }

    }

    public virtual void OnEnter() { if (luaEnter != null) { luaEnter(); } }
    public virtual void OnUpdate() { if (luaUpdate != null) { luaUpdate(); } }
    public virtual void OnExit() { if (luaExit != null) { luaExit(); } }
     
	public virtual void OnLateUpdate() { if (luaLateUpdate != null) { luaLateUpdate(); } }

    public virtual void Destory()
    {
        luaEnter = null;
        luaUpdate = null;
        luaExit = null;
        luaLateUpdate = null;
        scriptEnv.Dispose();
    }
}
