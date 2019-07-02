using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;


public class LuaSumlator : MonoSingleton<LuaSumlator> {

    internal static float lastGCTime = 0;
    public const float GCInterval = 1;//1 second 
    public const string hotfixMainScriptName = "XLua.HotfixMain";
    public const string commonMainScriptName = "Common.Main";
    public const string gameMainScriptName = "GameMain";
    public const string luaScriptsFolder = "Assets";

    /// <summary>
    /// 全局唯一的lua虚拟机
    /// </summary>
    static LuaEnv myLuaEnv;
    /// <summary>
    /// Lua虚拟机
    /// </summary>
    public static LuaEnv MyLuaEnv
    {
        get
        {
            if (myLuaEnv == null)
            {
                myLuaEnv = new LuaEnv();
            }
            return myLuaEnv;
        }
    }

    protected override void Initialize()
    {
        base.Initialize();
        //MyLuaEnv.AddLoader(CustomLoader);
        //MyLuaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadPb);
    }

    public void Update()
    {
        if (Time.time - lastGCTime > GCInterval)
        {
            MyLuaEnv.Tick();
            lastGCTime = Time.time;
        }
    }

    public void SafeDoString(string scriptContent)
    {
        if (myLuaEnv != null)
        {
            try
            {
                myLuaEnv.DoString(scriptContent);
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }
    public void SafeDoString(string scriptContent,string cunk,ref LuaTable table)
    {
        if (MyLuaEnv != null)
        {
            try
            {
                MyLuaEnv.DoString(scriptContent,cunk,table);
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }

    public void StartHotfix(bool restart = false)
    {
        if (myLuaEnv == null)
        {
            return;
        }

        if (restart)
        {
            StopHotfix();
        }
        else
        {
            LoadScript(hotfixMainScriptName);
        }
        SafeDoString("HotfixMain.Start()");
    }

    public void StopHotfix()
    {
        SafeDoString("HotfixMain.Stop()");
    }

    public void LoadScript(string scriptName)
    {
        SafeDoString(string.Format("require('{0}')", scriptName));
    }

    public void LoadScript(string scriptName,ref LuaTable table)
    {
        SafeDoString(string.Format("require('{0}')", scriptName),scriptName,ref table);
    }
    /// <summary>
    /// 暂时只能读本地数据
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public static byte[] CustomLoader(ref string filepath)
    {
        Debug.LogError("Luas/" + filepath);
        filepath = filepath.Replace(".", "/");
        TextAsset asset = ResourceMgr.Load<TextAsset>("Luas/" + filepath + ".lua");
        Debug.LogError(asset.text);
        if (asset != null)
        {
            return System.Text.Encoding.UTF8.GetBytes(asset.text);
        }
        Logger.LogError("Load lua script failed : " + filepath + ", You should preload lua assetbundle first!!!");
        return null;
    }
}
