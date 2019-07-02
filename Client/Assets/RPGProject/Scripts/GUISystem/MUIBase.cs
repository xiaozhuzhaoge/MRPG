using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

public abstract class MUIBase : MonoBehaviour {

    /// <summary>
    /// 映射委托
    /// </summary>
    /// <param name="objs"></param>
    public delegate void Objects (object[] objs);
    public string path;
    public TextAsset luaScript;
    private Action luaStart;
    private Action luaUpdate;
    private Objects luaOpen;
    private Objects luaClose;
    private Objects luaRefresh;
  

    private LuaTable scriptEnv;

    /// <summary>
    /// 静态窗口 默认在场景中需要勾上
    /// </summary>
    public bool StaticWindow;

    private void Awake()
    {
        OnAwake();
        if(StaticWindow)
         MUIMgr.Instance.PushUI(this);
    }

    void Start()
    {
        if (luaStart != null)
        {
            luaStart();
        }
    }

    void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate();
        }

    }

    public bool NoClose;
 
    public virtual void OnAwake()
    {
        MUIMgr.Instance.AddUI(this);
        transform.SetParent(MUIMgr.Instance.Canvas, false);

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

        //LuaSumlator.Instance.LoadScript(path,ref scriptEnv);
        LuaSumlator.MyLuaEnv.DoString(luaScript.text, "MUIBase" + gameObject.name, scriptEnv);
        Action luaAwake = scriptEnv.Get<Action>("Awake");
        scriptEnv.Get("Start", out luaStart);
        scriptEnv.Get("Update", out luaUpdate);
        scriptEnv.Get("Open", out luaOpen);
        scriptEnv.Get("Close", out luaClose);
        scriptEnv.Get("Refresh", out luaRefresh);
        if (luaAwake != null)
        {
            luaAwake();
        }
    }

    public string UiName;
	public virtual void Open(params object[] parms)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        if (!NoClose)
            MUIMgr.Instance.PushUI(this);

        if(luaOpen != null)
        {
            luaOpen(parms);
        }
    }

    public virtual void Close(params object[] parms)
    {
        if (NoClose)
            return;

        gameObject.SetActive(false);
        transform.SetAsFirstSibling();

        if(luaClose != null)
        {
            luaClose(parms);
        }
    }

    public virtual void Refresh(params object[] parms)
    {
        Debug.Log(gameObject.name + "Refresh");
        if(luaRefresh != null)
        {
            luaRefresh(parms);
        }
    }

    public virtual void OnDestroy()
    {
        MUIMgr.Instance.RemoveUI(UiName);
        luaStart = null;
        luaUpdate = null;
        luaOpen = null;
        luaClose = null;
        luaRefresh = null;
        if(scriptEnv != null)
            scriptEnv.Dispose();
    }

    public void Back()
    {
        MUIMgr.Instance.PopUI();
    }
}
