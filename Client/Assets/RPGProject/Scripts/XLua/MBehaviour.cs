using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

[LuaCallCSharp]
public class MBehaviour : MonoBehaviour {

    public string path;
    public TextAsset luaScript;
    private Action luaStart;
    private Action luaUpdate;
    private Action luaFixedUpdate;
    private Action luaLateUpdate;
    private Action<Collider> luaOnTriggerEnter;
    private Action<Collision> luaOnCollisionEnter;

    private LuaTable scriptEnv;

    private void Awake()
    {
        luaScript = ResourceMgr.Load<TextAsset>("Luas/" + path + ".lua");
        if (luaScript == null)
            return;

        scriptEnv = LuaSumlator.MyLuaEnv.NewTable();//创建一个LUA表
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = LuaSumlator.MyLuaEnv.NewTable();//创一个元表
        meta.Set("__index", LuaSumlator.MyLuaEnv.Global);//__index = Gloabl
        scriptEnv.SetMetaTable(meta);//setmetatable
        meta.Dispose();

        scriptEnv.Set("self", this);
          
        LuaSumlator.MyLuaEnv.DoString(luaScript.text, "MBehaviour" + gameObject.name, scriptEnv);

        Action luaAwake = scriptEnv.Get<Action>("awake");
        scriptEnv.Get("Start", out luaStart);
        scriptEnv.Get("Update", out luaUpdate);

        scriptEnv.Get("FixedUpdate", out luaFixedUpdate);
        scriptEnv.Get("LateUpdate", out luaLateUpdate);
        scriptEnv.Get("OnTriggerEnter", out luaOnTriggerEnter);
        scriptEnv.Get("OnCollsionEnter", out luaOnCollisionEnter);

        if (luaAwake != null)
        {
            luaAwake();
        }
    }

    // Use this for initialization
    void Start () {
        if (luaStart != null)
        {
            luaStart();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (luaUpdate != null)
        {
            luaUpdate();
        }
       
    }

    private void LateUpdate()
    {
        if(luaLateUpdate != null)
        {
            luaLateUpdate();
        }
    }

    private void FixedUpdate()
    {
        if(luaFixedUpdate != null)
        {
            luaFixedUpdate();
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (luaOnTriggerEnter != null)
        {
            luaOnTriggerEnter(other);
        }
    }

    void OnDestroy()
    {
        luaStart = null;
        luaUpdate = null;
        luaFixedUpdate = null;
        luaLateUpdate = null;
        luaOnTriggerEnter = null;
        luaOnCollisionEnter = null;
        scriptEnv.Dispose();
    }
}
