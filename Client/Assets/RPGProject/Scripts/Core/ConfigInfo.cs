using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.Reflection;
 
public class Config : ConfigBase
{
    string className;
    string path;
    string container;

    public string ClassName
    {
        get
        {
            return className;
        }

        set
        {
            className = value;
        }
    }

    public string Path
    {
        get
        {
            return path;
        }

        set
        {
            path = value;
        }
    }

    public string Container
    {
        get
        {
            return container;
        }

        set
        {
            container = value;
        }
    }
}

public partial class ConfigInfo : MonoSingleton<ConfigInfo> {

    //public bool IsLoadLocal;

    Dictionary<string, Config> configs = new Dictionary<string, Config>();
    public Dictionary<string, Config> Configs
    {
        get
        {
            return configs;
        }

        set
        {
            configs = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void ReadConfig()
    {
        Configs = ReadConfigs<Config>("Configs/Configs");
        Type ty = typeof(ConfigInfo);
        MethodInfo method = ty.GetMethod("ReadConfigs");

        foreach (var data in Configs.Values)
        {
            Type tragetClass = Type.GetType(data.ClassName);
         
            method = method.MakeGenericMethod(tragetClass);//创建泛型方法  Dictionary<string, ItemConfig> ReadConfigs<ItemConfig>(string path) where T : ConfigBase
            PropertyInfo pinfp = ty.GetProperty(data.Container);//找到 ItemDir 字典
            
            pinfp.SetValue(Instance, method.Invoke(Instance, new object[] { data.Path }), null); 
            
        }
    }
 

    public Dictionary<string,T> ReadConfigs<T>(string path)where T:ConfigBase
    {
        Dictionary<string, T> configs = new Dictionary<string, T>();
        TextAsset ta = ResourceMgr.Load<TextAsset>(path);
        JsonData ar = JsonMapper.ToObject(ta.text);
        foreach (var data in ar)
        {
            T config = JsonMapper.ToObject<T>(JsonMapper.ToJson(data));
            configs.Add(config.Id, config);
        }
        return configs;
    }

    //public void ReadConfigs<T>(string path,Dictionary<string,T> dir) where T : ConfigBase
    //{
         
    //    TextAsset ta = Resources.Load<TextAsset>(path);
    //    JsonData ar = JsonMapper.ToObject(ta.text);
    //    foreach (var data in ar)
    //    {
    //        T config = JsonMapper.ToObject<T>(JsonMapper.ToJson(data));
    //        dir.Add(config.Id, config);
    //    }
    //}
}
