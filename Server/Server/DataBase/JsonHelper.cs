using System;
using System.Collections.Generic;
using proto.MyProto;
using LitJson;
using System.IO;

public class PropConfig
{
    private string id;
    private string name;
    private string type;
    private string icon;
    private string skill;
    private string canCombine;

    public string Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public string Type { get => type; set => type = value; }
    public string Icon { get => icon; set => icon = value; }
    public string Skill { get => skill; set => skill = value; }
    public string CanCombine { get => canCombine; set => canCombine = value; }
}


public class SiginInJson
{
    private string id;
    private string prop28;
    private string prop29;
    private string prop30;
    private string prop31;
    private string nm28;
    private string nm29;
    private string nm30;
    private string nm31;

    public string Id { get => id; set => id = value; }
    public string Prop28 { get => prop28; set => prop28 = value; }
    public string Prop29 { get => prop29; set => prop29 = value; }
    public string Prop30 { get => prop30; set => prop30 = value; }
    public string Prop31 { get => prop31; set => prop31 = value; }
    public string num28 { get => nm28; set => nm28 = value; }
    public string num29 { get => nm29; set => nm29 = value; }
    public string num30 { get => nm30; set => nm30 = value; }
    public string num31 { get => nm31; set => nm31 = value; }
}

public class ShopConfig {

    string id;
    string propId;
    string group_Id;
    string cost_Type;
    string cost_Num;
 

    public string Id { get => id; set => id = value; }
    public string PropId { get => propId; set => propId = value; }
    public string Group_Id { get => group_Id; set => group_Id = value; }
    public string Cost_Type { get => cost_Type; set => cost_Type = value; }
    public string Cost_Num { get => cost_Num; set => cost_Num = value; }
}


public static class JsonHelper
{
    private static string signInJosnPath= "Config/Signin.json";
    private static string propInfoJsonPath = "Config/Props.json";
    private static string shopInfoJsonPath = "Config/Shop.json";

 
    private static Dictionary<string, SiginInJson> signInJsons = new Dictionary<string, SiginInJson>();
    private static Dictionary<string, ShopConfig> shops = new Dictionary<string, ShopConfig>();
    private static Dictionary<string, PropConfig> props = new Dictionary<string, PropConfig>();

    static JsonHelper()
    {
        Init();
    }

    static void ReadConfig(string path,Action<JsonData> callback)
    {
        Console.WriteLine(path);
        StreamReader sr = new StreamReader(path);
        string cache = "";
        try
        {
            cache = sr.ReadToEnd();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            sr.Dispose();
        }
        JsonData jsonData = JsonMapper.ToObject(cache);
        if(callback != null)
        {
            callback(jsonData);
        }
    }

    static void Init()
    {
        ReadConfig(signInJosnPath, (jsonData) => {
            foreach (JsonData item in jsonData)
            {
                SiginInJson temp = JsonMapper.ToObject<SiginInJson>(JsonMapper.ToJson(item));
                signInJsons.Add(temp.Id, temp);
            }
        });
        ReadConfig(shopInfoJsonPath, (jsonData) => {
            foreach (JsonData item in jsonData)
            {
                ShopConfig temp = JsonMapper.ToObject<ShopConfig>(JsonMapper.ToJson(item));
                shops.Add(temp.Id, temp);
            }
        });
        ReadConfig(propInfoJsonPath, (jsonData) => {
            foreach (JsonData item in jsonData)
            {
                PropConfig temp = JsonMapper.ToObject<PropConfig>(JsonMapper.ToJson(item));
                props.Add(temp.Id, temp);
            }
        });
    }

    /// <summary>
    /// 获取商店配置
    /// </summary>
    /// <param name="shopId"></param>
    /// <returns></returns>
    public static ShopConfig GetShopProp(string shopId)
    {
        if (shops.ContainsKey(shopId))
            return shops[shopId];
        else
            return null;
    }

    /// <summary>
    /// 获取指定道具配置
    /// </summary>
    /// <param name="prop"></param>
    /// <returns></returns>
    public static PropConfig GetProp(string propId)
    {
        if (props.ContainsKey(propId))
            return props[propId];
        else
            return null;
    }

    public static PropInfo GetSiginJson(int signId)
    {
        SiginInJson temp = GetSigin(signId);
        PropInfo prop = new PropInfo();
        prop.id = Convert.ToInt32(temp.Id);
        switch (TimeHelper.Month)
        {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                {
                    
                    prop.propId = Convert.ToInt32(temp.Prop31);
                    prop.num = Convert.ToInt32(temp.num31);
                    break;
                }
            case 2:
                {
                    prop.propId = Convert.ToInt32(temp.Prop28);
                    prop.num = Convert.ToInt32(temp.num28);
                    break;
                }
            case 4:
            case 6:
            case 9:
            case 11:
                {
                    prop.propId = Convert.ToInt32(temp.Prop30);
                    prop.num = Convert.ToInt32(temp.num30);
                    break;
                }
            default:
                break;
        }
        return prop;
    }

    private static SiginInJson GetSigin(int signId)
    {
        if(signInJsons.ContainsKey(signId.ToString()))
        {
            return signInJsons[signId.ToString()];
        }
        return null;
    }


}
