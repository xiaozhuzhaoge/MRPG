using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void OnPercentEventHandler (float percent, string message);
public delegate void OnResourceDoneEventHandler ();

public class ResourceMgr : MonoSingleton<ResourceMgr> {

    public event OnPercentEventHandler OnPercent;
    public event OnResourceDoneEventHandler OnDone;

    /// <summary>
    /// 服务器网址
    /// </summary>
    public string Url = "http://172.16.12.11/";
    AssetBundleManifest abm;
    public Dictionary<string, AssetBundle> cahces_资源包缓存 = new Dictionary<string, AssetBundle> ();
    public Dictionary<string, AssetInfo> assetInfos_资源包加载缓存 = new Dictionary<string, AssetInfo> (); //key GUI/A value assetInfo
    /// <summary>
    /// 配置文件名字
    /// </summary>
    public string MainfestName;
    /// <summary>
    /// 要替换字符串
    /// </summary>
    public string ext = "assets/rpgproject/abassets/resources/";

    // Use this for initialization
    protected override void Initialize () {

        base.Initialize ();

    }

    void Start () {
        if (OnPercent != null) {
            OnPercent (0f, "开始读取资源包");
        }
        if (!IsLoadLocal) {
            StartCoroutine (DownLoadManifest (Url, MainfestName));
        } else {
            if (OnPercent != null)
                OnPercent (1f, "正在读取本地配置");
            ConfigInfo.Instance.ReadConfig ();
            if (OnDone != null)
                OnDone ();
        }

    }

    /// <summary>
    /// 下载配置文件 基于配置文件批量下载资源包
    /// </summary>
    /// <param name="Url"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    IEnumerator DownLoadManifest (string Url, string assetName) {
        if (OnPercent != null)
            OnPercent (0.1f, "开始分析资源包");

        WWW www = new WWW (Url + assetName);
        yield return www;
        if (www.isDone) {
            AssetBundle ab = www.assetBundle;

            abm = ab.LoadAsset<AssetBundleManifest> ("assetbundlemanifest");
            int count = abm.GetAllAssetBundles ().Length;
            int index = 0;
            foreach (var st in abm.GetAllAssetBundles ()) {
                if (OnPercent != null)
                    OnPercent (0.9f * Utility.CTFloat (index) / Utility.CTFloat (count), "正在下载资源" + st);
                yield return StartCoroutine (DownLoadAssetBundle (Url, st, abm.GetAssetBundleHash (st)));
                yield return new WaitForEndOfFrame ();
                index++;
            }
            //下载完所有AB包之后
            //指定AB包中的所有资源路径
            //加载配置文件
            //专场

            //LuaConfigInfo.Instance.LoadLuas();
            if (OnPercent != null)
                OnPercent (1f, "正在读取本地配置");

            ConfigInfo.Instance.ReadConfig ();

            if (OnDone != null)
                OnDone ();

        }
    }

    /// <summary>
    /// 下载指定资源包
    /// </summary>
    /// <param name="Url"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    IEnumerator DownLoadAssetBundle (string Url, string assetName, Hash128 hasdCode) {
        Debug.LogError ("开始下载指定资源包" + Url + assetName + "  " + hasdCode);

        WWW www = WWW.LoadFromCacheOrDownload (Url + assetName, hasdCode); //!!!!

        yield return www;
        if (www.isDone) {
            AssetBundle temp = www.assetBundle;

            if (!cahces_资源包缓存.ContainsKey (temp.name))
                cahces_资源包缓存.Add (temp.name, temp);

            foreach (var data in www.assetBundle.GetAllAssetNames ()) {
                string t = data.Replace (ext, "");
                t = t.Remove (t.LastIndexOf ("."));
                if (assetInfos_资源包加载缓存.ContainsKey (t))
                    continue;
                assetInfos_资源包加载缓存.Add (t, new AssetInfo (temp.name, data));
            }

        }

        //cahces_资源包缓存[].LoadAsset<>() 这个资源属于哪个资源包 这个资源的实际地址

    }

    public struct AssetInfo {
        public string AssetBundleName_资源包名称;
        public string AssetPath_实际资源地址;
        public AssetInfo (string abn, string ap) {
            this.AssetBundleName_资源包名称 = abn;
            this.AssetPath_实际资源地址 = ap;
        }
    }

    public bool IsLoadLocal = true;

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T Load<T> (string path) where T : Object {
        if (Instance.IsLoadLocal) {

            return Resources.Load<T> (path); //使用Reousrce.Load加载
        }

        ///通过AB包加载资源
        string lowerpath = path.ToLower (); //把大写转小写
        //foreach(var data in Instance.assetInfos_资源包加载缓存){
        //    Debug.Log(data);
        //}

        if (Instance.assetInfos_资源包加载缓存.ContainsKey (lowerpath)) {

            AssetInfo info = Instance.assetInfos_资源包加载缓存[lowerpath];
            AssetBundle ab = Instance.cahces_资源包缓存[info.AssetBundleName_资源包名称];
            return ab.LoadAsset<T> (info.AssetPath_实际资源地址); //指定某个AB包进行资源加载
        }
        ///通过Resource.Load加载
        else {
            //Debug.Log("????????" + path);
            return Resources.Load<T> (path); //使用Reousrce.Load加载
        }

    }

    public static Object Load (string path) {
        if (Instance.IsLoadLocal) {

            return Resources.Load<Object> (path); //使用Reousrce.Load加载
        }

        ///通过AB包加载资源
        string lowerpath = path.ToLower (); //把大写转小写
        //foreach(var data in Instance.assetInfos_资源包加载缓存){
        //    Debug.Log(data);
        //}

        if (Instance.assetInfos_资源包加载缓存.ContainsKey (lowerpath)) {

            AssetInfo info = Instance.assetInfos_资源包加载缓存[lowerpath];
            AssetBundle ab = Instance.cahces_资源包缓存[info.AssetBundleName_资源包名称];
            return ab.LoadAsset<Object> (info.AssetPath_实际资源地址); //指定某个AB包进行资源加载
        }
        ///通过Resource.Load加载
        else {
            //Debug.Log("????????" + path);
            return Resources.Load<Object> (path); //使用Reousrce.Load加载
        }

    }

    public static Object[] LoadAll (string path) {
        if (Instance.IsLoadLocal) {
            return Resources.LoadAll (path); //使用Reousrce.Load加载
        } ///通过AB包加载资源
        string lowerpath = path.ToLower (); //把大写转小写
        //foreach(var data in Instance.assetInfos_资源包加载缓存){
        //    Debug.Log(data);
        //}

        if (Instance.assetInfos_资源包加载缓存.ContainsKey (lowerpath)) {

            AssetInfo info = Instance.assetInfos_资源包加载缓存[lowerpath];
            AssetBundle ab = Instance.cahces_资源包缓存[info.AssetBundleName_资源包名称];
            return ab.LoadAllAssets<Object> (); //指定某个AB包进行资源加载
        }
        ///通过Resource.Load加载
        else {
            //Debug.Log("????????" + path);
            return Resources.LoadAll<Object> (path); //使用Reousrce.Load加载
        }

    }
    /// <summary>
    /// 生成指定道具
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static GameObject CreateObj (string path) {
        var res = ResourceMgr.Load<GameObject> (path);
        if (res == null)
            return null;
        return GameObject.Instantiate<GameObject> (res);
    }

    /// <summary>
    /// 创建UI对象到指定父类下面
    /// </summary>
    /// <param name="path"></param>
    /// <param name="target"></param>
    /// <param name="worldStay"></param>
    /// <returns></returns>
    public static GameObject CreateUIPrefab (string path, Transform target, bool worldStay = false) {
        var go = CreateObj (path);
        go.transform.SetParent (target, worldStay);
        return go;
    }

    /// <summary>
    /// 创建特效
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="target"></param>
    /// <param name="worldStay"></param>
    /// <returns></returns>
    public static GameObject CreateEffect (string effectName, Transform target = null, bool worldStay = false) {
        var go = CreateObj ("Effects/" + effectName);
        go.transform.SetParent (target, worldStay);
        return go;
    }

    public static GameObject CreateEffect (string effectName, Vector3 worldPos) {
        var go = CreateObj ("Effects/" + effectName);
        go.transform.position = worldPos;
        return go;
    }

    /// <summary>
    /// 加载指定图集中的图片
    /// </summary>
    /// <param name="index"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Sprite LoadSpriteFromAtals (int index, string path) {
        return ResourceMgr.LoadAll (path) [index] as Sprite;
    }

}